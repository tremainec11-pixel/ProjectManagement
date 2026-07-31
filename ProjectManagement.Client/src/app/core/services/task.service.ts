import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { Task } from '../models/task.model';

@Injectable({
  providedIn: 'root'
})
export class TaskService {

  private readonly http = inject(HttpClient);

  private readonly apiUrl =
    `${environment.apiUrl}/Tasks`;

  /**
   * Get all tasks
   */
  getTasks(): Observable<Task[]> {
    return this.http.get<Task[]>(
      this.apiUrl
    );
  }

  /**
   * Get task by id
   */
  getTask(id: number): Observable<Task> {
    return this.http.get<Task>(
      `${this.apiUrl}/${id}`
    );
  }

  /**
   * Create task
   */
  createTask(
    task: Partial<Task>
  ): Observable<Task> {

    return this.http.post<Task>(
      this.apiUrl,
      task
    );
  }

  /**
   * Update task
   */
  updateTask(
    id: number,
    task: Partial<Task>
  ): Observable<void> {

    return this.http.put<void>(
      `${this.apiUrl}/${id}`,
      task
    );
  }

  /**
   * Delete task
   */
  deleteTask(
    id: number
  ): Observable<void> {

    return this.http.delete<void>(
      `${this.apiUrl}/${id}`
    );
  }

}