import { Routes } from '@angular/router';
import { TaskListComponent } from './components/task-list/task-list.component';
import { UnsavedChangesGuard } from './guards/unsaved-changes.guard';

export const routes: Routes = [
  { path: 'task-list', component: TaskListComponent, canDeactivate: [UnsavedChangesGuard]},
  { path: '', redirectTo: 'task-list', pathMatch: 'full'}
];
