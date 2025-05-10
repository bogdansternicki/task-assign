import { Component, effect, HostListener, OnInit } from '@angular/core';
import { User } from '../../interfaces/user';
import { CommonTask } from '../../interfaces/common-task';
import { UsersService } from '../../services/users-service.service';
import { TasksService } from '../../services/tasks-service.service';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatListModule } from '@angular/material/list';
import { MatButtonModule } from '@angular/material/button';
import { CdkDragDrop, CdkDropList, DragDropModule, transferArrayItem, moveItemInArray } from '@angular/cdk/drag-drop';
import { MatCardModule } from '@angular/material/card';
import { UserType } from '../../enums/user-type';
import { TaskType } from '../../enums/task-type';
import { UnsavedChangesGuard } from '../../guards/unsaved-changes.guard';
import { ListItemComponent } from '../list-item/list-item.component';

@Component({
  selector: 'app-task-list',
  imports: [
    MatFormFieldModule,
    MatSelectModule,
    MatListModule,
    MatButtonModule,
    DragDropModule,
    MatCardModule,
    CdkDropList,
    ListItemComponent
  ],
  templateUrl: './task-list.component.html',
  styleUrl: './task-list.component.scss'
})
export class TaskListComponent implements OnInit {
  private usersEffect = effect(() => this.users = this.usersService.users());
  private tasksEffect = effect(() => {
    this.tasks = this.tasksService.tasks();
    this.availableTasks = this.tasks.filter(task => task.userId == null);
  });

  users: User[] = [];
  tasks: CommonTask[] = [];
  selectedUser: User | null = null;
  assignedTasks: CommonTask[] = [];
  availableTasks: CommonTask[] = [];
  isDirty: boolean = false;

  constructor(
    private usersService: UsersService,
    private tasksService: TasksService,
    private unsavedChangesGuard: UnsavedChangesGuard
  ) { }

  ngOnInit(): void {
    this.usersService.loadUsers();
    this.tasksService.loadTasks();
  }

  async userChanged(): Promise<void> {
    if (await this.unsavedChangesGuard.canDeactivate(this)) {
      this.assignedTasks = this.tasks.filter(task => task.userId === this.selectedUser?.id);
      this.availableTasks = this.tasks.filter(task => task.userId == null && (this.selectedUser?.type === UserType.DevOps || task.type === TaskType.Implementation));
      this.isDirty = false;
    }
  }

  drop(event: CdkDragDrop<CommonTask[]>) {
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      this.isDirty = true;

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

    // this.availableTasks = this.tasks.filter(task => task.user == null);
    this.isDirty = false;
  }
}
