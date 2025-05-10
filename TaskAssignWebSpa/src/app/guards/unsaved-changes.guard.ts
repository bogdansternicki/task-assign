import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { Observable } from 'rxjs';
import { TaskListComponent } from '../components/task-list/task-list.component';

@Injectable({
  providedIn: 'root'
})
export class UnsavedChangesGuard implements CanDeactivate<TaskListComponent> {
  canDeactivate(component: TaskListComponent): Observable<boolean> | Promise<boolean> | boolean {
    if (component.isDirty) {
      return window.confirm('Changes you made may not be saved.');
    }
    return true;
  }
}
