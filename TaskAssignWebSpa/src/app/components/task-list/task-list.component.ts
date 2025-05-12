import { Component, computed, HostListener, OnInit } from '@angular/core';
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
import { CdkDragDrop, CdkDropList, DragDropModule, transferArrayItem, moveItemInArray } from '@angular/cdk/drag-drop';
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
    ListItemComponent
  ],
  templateUrl: './task-list.component.html',
  styleUrl: './task-list.component.scss'
})
export class TaskListComponent implements OnInit {
    // if (this.selectedUser == null && this.users && this.users.length)
    //   this.selectedUser = this.users[0];

  readonly assignedTaskCount = computed(() => this.tasksService.assignedTaskCount());
  readonly availableTaskCount = computed(() => this.tasksService.availableTaskCount());
  readonly availableTasks = computed(() => [...this.tasksService.availableTasks()]);
  readonly assignedTasks = computed(() => [...this.tasksService.assignedTasks()]);

  users = computed(() => this.usersService.users());
  selectedUser: User | null = null;
  currentAssignedTaskPageIndex: number = 0;
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
      this.tasksService.movedTasks.set({ assignTasks: [], availableTasks: [] });

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
      const movedItem = event.item.data;

      this.tasksService.movedTasks.update(value => {
        if (assign) {
          return {
            // ...value,
            // assignTasks: [...value.assignTasks, movedItem]
            availableTasks: value.availableTasks.filter(task => task.id !== movedItem.id),
            assignTasks: [...value.assignTasks, movedItem].filter(assignedTask => !this.tasksService.assignedTasks().some(task => task.id == assignedTask.id))
          };
        } else {
          // console.log([...value.availableTasks, movedItem]);
          // console.log(value.availableTasks)
          // console.log([...value.availableTasks, movedItem].filter(availableTask => !this.availableTasks().some(task => task.id == availableTask.id)));

          return {
            // ...value,
            // availableTasks: [...value.availableTasks, movedItem]
            assignTasks: value.assignTasks.filter(task => task.id !== movedItem.id),
            availableTasks: [...value.availableTasks, movedItem].filter(availableTask => !this.tasksService.availableTasks().some(task => task.id == availableTask.id))
          };
        }
      });

      // console.log(this.tasksService.availableTasks());
      // console.log(this.tasksService.movedTasks().assignTasks, this.tasksService.movedTasks().availableTasks);


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

  isTaskDirty = (id: number, assign: boolean): boolean => (assign ? this.tasksService.movedTasks().assignTasks : this.tasksService.movedTasks().availableTasks).map(task => task.id).includes(id);

  submitTasks(): void {
    this.tasksService.updateAssignedUsers({
      userId: this.selectedUser?.id ?? 0,
      assignTaskIds: this.tasksService.movedTasks().assignTasks.map(task => task.id),
      unAssignTaskIds: this.tasksService.movedTasks().availableTasks.map(task => task.id)
    } as UpdateTasks).subscribe({
      next: () => {
        this.tasksService.movedTasks.update(value => ({ ...value, assignTasks: [], availableTasks: [] }));
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
