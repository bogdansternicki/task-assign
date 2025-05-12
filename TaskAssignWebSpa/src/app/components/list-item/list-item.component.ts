import { Component, HostBinding, Input } from '@angular/core';
import { CommonTask } from '../../interfaces/common-task';
import { TaskType } from '../../enums/task-type';
import { DeploymentTask } from '../../interfaces/deployment-task';
import { MaintenanceTask } from '../../interfaces/maintenance-task';
import { ImplementationTask } from '../../interfaces/implementation-task';
import { MatCardModule } from '@angular/material/card';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-list-item',
  imports: [
    MatCardModule,
    DatePipe
  ],
  templateUrl: './list-item.component.html',
  styleUrl: './list-item.component.scss'
})
export class ListItemComponent {
  @Input() task!: CommonTask;

  getTaskTypeName = (type: TaskType): string => TaskType[type];

  isDeploymentTask = (task: CommonTask): task is DeploymentTask => task.type === TaskType.Deployment;

  isImplementationTask = (task: CommonTask): task is ImplementationTask => task.type === TaskType.Implementation;

  isMaintenanceTask = (task: CommonTask): task is MaintenanceTask => task.type === TaskType.Maintenance;

  getStars = (difficulty: 1 | 2 | 3 | 4 | 5): string => '⚙️'.repeat(difficulty);

  @HostBinding('cdkDragData') get cdkDragData() {
    return this.task;
  }
}
