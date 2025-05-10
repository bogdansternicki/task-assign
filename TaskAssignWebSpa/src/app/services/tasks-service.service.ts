import { Injectable, signal } from "@angular/core";
import { CommonTask } from "../interfaces/common-task";
import { HttpClient } from "@angular/common/http";
import { catchError, of } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class TasksService {
  private readonly apiUrl = 'https://localhost:5201/TaskAssignWebApi/tasks';

  tasks = signal<CommonTask[]>([]);

  constructor(private http: HttpClient) { }

  loadTasks(): void {
    this.http.get(this.apiUrl).pipe(catchError(() => of([]))).subscribe(tasks => this.tasks.set(tasks as CommonTask[]));
  }
}
