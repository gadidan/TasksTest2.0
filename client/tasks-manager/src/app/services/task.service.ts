import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Task } from '../Models/task.model';
import { LogService } from './log.service';

@Injectable({ providedIn: 'root' })
export class TaskService {
  //private apiUrl = 'https://localhost:5001/api/tasks';
  private apiUrl = 'https://localhost:44369/api/tasks';
  
  constructor(private http: HttpClient, private log: LogService ) {}

  private getHeaders(): HttpHeaders {
    return new HttpHeaders({ 'Name-Developer': 'Amit Cohen' });
  }

  getTasks(): Observable<Task[]> {
    this.log.logDebug("getTastks");
    return this.http.get<Task[]>(this.apiUrl, {
      headers: this.getHeaders()
    });

  }

  addTask(task: Task): Observable<Task> {
    this.log.logDebug("addTask");

    return this.http.post<Task>(this.apiUrl, task, {
      headers: this.getHeaders()
    });
  }

  updateTask(task: Task): Observable<Task> {
    this.log.logDebug("updateTask");

    return this.http.put<Task>(`${this.apiUrl}/${task.id}`, task, {
      headers: this.getHeaders()
    });
  }

  deleteTask(id: number): Observable<void> {
    this.log.logDebug("deleteTask");

    return this.http.delete<void>(`${this.apiUrl}/${id}`, {
      headers: this.getHeaders()
    });
  }
}