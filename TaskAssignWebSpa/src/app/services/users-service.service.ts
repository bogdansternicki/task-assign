import { Injectable, signal } from '@angular/core';
import { User } from '../interfaces/user';
import { HttpClient } from '@angular/common/http';
import { catchError, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  private readonly apiUrl = 'https://localhost:5201/TaskAssignWebApi/users';

  users = signal<User[]>([]);

  constructor(private http: HttpClient) { }

  loadUsers(): void {
    this.http.get(this.apiUrl).pipe(catchError(() => of([]))).subscribe(users => this.users.set(users as User[]));
  }
}
