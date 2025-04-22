import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TaskService } from '../../services/task.service.spec';
import { Task } from '../../Models/task.model';
import { TaskFormComponent } from '../task-form/task-form.component';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDividerModule } from '@angular/material/divider';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [
    CommonModule,
    TaskFormComponent,
    MatListModule,
    MatIconModule,
    MatButtonModule,
    MatTooltipModule,
    MatDividerModule
  ],
  template: `
    <h2>רשימת משימות</h2>
    <app-task-form [task]="editedTask" (save)="handleSave($event)"></app-task-form>

    <mat-list>
      <mat-divider></mat-divider>
      <mat-list-item *ngFor="let task of tasks" class="task-row">
        <div class="row-content">
        <div class="col title">{{ task.title }}</div>
        <div class="col date">{{ task.dueDate | date:'yyyy-MM-dd' }}</div>
        <div class="col desc">{{ task.description || '\u00A0' }}</div>
        <div class="col actions">
          <button mat-icon-button color="primary" matTooltip="ערוך" (click)="editTask(task)">
            <mat-icon>edit</mat-icon>
          </button>
          <button mat-icon-button color="warn" matTooltip="מחק" (click)="deleteTask(task.id)">
            <mat-icon>delete</mat-icon>
          </button>
        </div>

        </div>
      </mat-list-item>
      <mat-divider></mat-divider>
    </mat-list>
  `,
  styleUrls: ['./task-list.component.css']
})
export class TaskListComponent {
  taskService = inject(TaskService);
  tasks: Task[] = [];
  editedTask: Task | null = null;

  ngOnInit() {
    this.loadTasks();
  }

  loadTasks() {
    this.taskService.getTasks().subscribe({
      next: data => this.tasks = data,
      error: () => console.log('שגיאה בטעינת משימות')
    });
  }

  handleSave(task: Task) {
    if (task.id) {
      this.taskService.updateTask(task).subscribe({
        next: () => this.loadTasks(),
        error: () => alert('שגיאה בעדכון משימה')
      });
    } else {
      this.taskService.addTask(task).subscribe({
        next: () => this.loadTasks(),
        error: () => alert('שגיאה בהוספת משימה')
      });
    }
    this.editedTask = null;
  }

  deleteTask(id: number) {
    this.taskService.deleteTask(id).subscribe({
      next: () => this.loadTasks(),
      error: () => alert('שגיאה במחיקת משימה')
    });
  }

  editTask(task: Task) {
    this.editedTask = { ...task };
  }
} 
