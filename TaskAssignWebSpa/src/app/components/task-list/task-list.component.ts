import { Component, computed, effect, HostListener, OnInit, signal } from '@angular/core';
import { User } from '../../interfaces/user';
import { CommonTask } from '../../interfaces/common-task';
import { UsersService } from '../../services/users-service.service';
import { TasksService } from '../../services/tasks-service.service';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatListModule } from '@angular/material/list';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatCardModule } from '@angular/material/card';
import { CdkDragDrop, CdkDropList, DragDropModule, transferArrayItem, moveItemInArray, CdkDrag } from '@angular/cdk/drag-drop';
import { UserType } from '../../enums/user-type';
import { UnsavedChangesGuard } from '../../guards/unsaved-changes.guard';
import { ListItemComponent } from '../list-item/list-item.component';
import { UpdateTasks } from '../../interfaces/update-tasks';

@Component({
  selector: 'app-task-list',
  imports: [
    MatFormFieldModule,
    MatSelectModule,
    MatListModule,
    MatButtonModule,
    DragDropModule,
    MatCardModule,
    MatSnackBarModule,
    MatPaginatorModule,
    CdkDropList,
    CdkDrag,
    ListItemComponent
  ],
  templateUrl: './task-list.component.html',
  styleUrl: './task-list.component.scss'
})
export class TaskListComponent implements OnInit {
  private usersEffect = effect(() => this.users = this.usersService.users());

  private availableTasksEffect = effect(() => {
    this.availableTasks = this.tasksService.availableTasks();

    this.updateTasks().assignTaskIds.forEach(id => {
      this.availableTasks = this.availableTasks.filter(task => task.id !== id);
    });
  });

  private assignedTasksEffect = effect(() => {
    this.assignedTasks = this.tasksService.assignedTasks();

    this.updateTasks().unAssignTaskIds.forEach(id => {
      this.assignedTasks = this.assignedTasks.filter(task => task.id !== id);
    });
  });

  readonly assignedTaskCount = computed(() => this.tasksService.assignedTaskCount());
  readonly availableTaskCount = computed(() => this.tasksService.availableTaskCount());

  readonly updateTasks = signal<UpdateTasks>({
    userId: 0,
    assignTaskIds: [],
    unAssignTaskIds: []
  });

  users: User[] = [];
  selectedUser: User | null = null;

  assignedTasks: CommonTask[] = [];
  currentAssignedTaskPageIndex: number = 0;

  availableTasks: CommonTask[] = [];
  currentAvailableTaskPageIndex: number = 0;

  isDirty: boolean = false;

  constructor(
    private usersService: UsersService,
    private tasksService: TasksService,
    private unsavedChangesGuard: UnsavedChangesGuard,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.usersService.loadUsers();
  }

  async userChanged(): Promise<void> {
    if (await this.unsavedChangesGuard.canDeactivate(this)) {
      this.currentAssignedTaskPageIndex = 0;
      this.currentAvailableTaskPageIndex = 0;
      this.updateTasks.set({ userId: this.selectedUser?.id ?? 0, assignTaskIds: [], unAssignTaskIds: [] })

      this.tasksService.loadAssignedTasks(this.currentAssignedTaskPageIndex, this.selectedUser?.id ?? 0);
      this.tasksService.loadAvailableTasks(this.currentAvailableTaskPageIndex, this.selectedUser?.id ?? 0);

      this.isDirty = false;
    }
  }

  onAssignedListPageChange(event: PageEvent): void {
    this.currentAssignedTaskPageIndex = event.pageIndex;
    this.tasksService.loadAssignedTasks(this.currentAssignedTaskPageIndex, this.selectedUser?.id ?? 0);
  }

  onAvailableListPageChange(event: PageEvent): void {
    this.currentAvailableTaskPageIndex = event.pageIndex;
    this.tasksService.loadAvailableTasks(this.currentAvailableTaskPageIndex, this.selectedUser?.id ?? 0);
  }

  drop(event: CdkDragDrop<CommonTask[]>, assign: boolean) {
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      this.isDirty = true;

      if (assign) {
        this.updateTasks.update(value => ({
          ...value,
          unAssignTaskIds: value.unAssignTaskIds.filter(id => id !== event.item.data.id),
          assignTaskIds: [...value.assignTaskIds, event.item.data.id]
        }));
      } else {
        this.updateTasks.update(value => ({
          ...value,
          assignTaskIds: value.assignTaskIds.filter(id => id !== event.item.data.id),
          unAssignTaskIds: [...value.unAssignTaskIds, event.item.data.id]
        }));
      }

      transferArrayItem(
        event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex,
      );
    }
  }

  getUserTypeName = (type: UserType): string => UserType[type];

  @HostListener('window:beforeunload', ['$event'])
  async unloadNotification($event: BeforeUnloadEvent): Promise<void> {
    if (!await this.unsavedChangesGuard.canDeactivate(this))
      $event.preventDefault();
  }

  submitTasks(): void {
    this.tasksService.updateAssignedUsers(this.updateTasks()).subscribe({
      next: () => {
        this.tasksService.loadAssignedTasks(this.currentAssignedTaskPageIndex, this.selectedUser?.id ?? 0);
        this.tasksService.loadAvailableTasks(this.currentAvailableTaskPageIndex, this.selectedUser?.id ?? 0);
        this.isDirty = false;
        this.snackBar.open('Tasks updated successfully!', 'Close', {
          duration: 3000
        });
      },
      error: (error) => {
        this.snackBar.open(error.error, 'Close', {
          duration: 3000
        });
      }
    });
  }
}
