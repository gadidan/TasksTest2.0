import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Task } from '../Models/task.model';


@Injectable({
  providedIn: 'root'
})
export class LogService {

  private apiUrl = 'https://localhost:44369/api/log';
  
  constructor(private http: HttpClient) {}

  private getHeaders(): HttpHeaders {
    return new HttpHeaders({ 'Name-Developer': 'Amit Cohen' });
  }

  logDebug(message: string) : string {
    let ms = new Date().toISOString();
    console.log(ms, message);
    return message;
  }
}
