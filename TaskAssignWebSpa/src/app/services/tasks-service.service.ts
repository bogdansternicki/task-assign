import { Injectable, signal } from "@angular/core";
import { CommonTask } from "../interfaces/common-task";
import { HttpClient, HttpParams } from "@angular/common/http";
import { catchError, Observable, of } from "rxjs";
import { UpdateTasks } from "../interfaces/update-tasks";
import { TaskList } from "../interfaces/task-list";
import { MatSnackBar } from "@angular/material/snack-bar";

@Injectable({
  providedIn: 'root'
})
export class TasksService {
  private readonly apiUrl = 'https://localhost:5201/TaskAssignWebApi/tasks';

  assignedTasks = signal<CommonTask[]>([]);
  availableTasks = signal<CommonTask[]>([]);
  assignedTaskCount = signal<number>(0);
  availableTaskCount = signal<number>(0);

  movedTasks = signal<{ assignTasks: CommonTask[], availableTasks: CommonTask[] }>({ assignTasks: [], availableTasks: []});

  constructor(
    private http: HttpClient,
    private snackBar: MatSnackBar
  ) { }

  loadAvailableTasks(pageIndex: number, userId: number): void {
    const params = new HttpParams()
      .set('pageIndex', pageIndex.toString())
      .set('userId', userId.toString());

    this.http.get<TaskList>(`${this.apiUrl}/available`, { params }).pipe(catchError(() => {
      this.snackBar.open('Failed to load available tasks', 'Close', { duration: 3000 });
      return of({ tasks: [], count: 0 } as TaskList);
    })).subscribe((taskList: TaskList) => {
      this.availableTasks.set([...taskList.tasks] as CommonTask[]);
      this.availableTaskCount.set(taskList.count);
    });
  }

  loadAssignedTasks(pageIndex: number, userId: number): void {
    const params = new HttpParams()
      .set('pageIndex', pageIndex.toString())
      .set('userId', userId.toString());

    this.http.get<TaskList>(`${this.apiUrl}/assigned`, { params }).pipe(catchError(() => {
      this.snackBar.open('Failed to load assigned tasks', 'Close', { duration: 3000 });
      return of({ tasks: [], count: 0 } as TaskList);
    })).subscribe((taskList: TaskList) => {
      this.assignedTasks.set([...taskList.tasks] as CommonTask[]);
      this.assignedTaskCount.set(taskList.count);
    });
  }

  updateAssignedUsers(updateTasks: UpdateTasks): Observable<any> {
    return this.http.put(this.apiUrl, updateTasks);
  }
}
