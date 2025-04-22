// קומפוננטת טופס להוספת/עריכת משימה
// src/app/components/task-form.component.ts
import { Component, inject, Input, Output, EventEmitter,  OnChanges,  SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Task } from '../../Models/task.model';
import { MatNativeDateModule } from "@angular/material/core";
import { MatFormFieldModule } from '@angular/material/form-field'
import { MatInputModule, } from '@angular/material/input'
import { MatDatepickerModule } from '@angular/material/datepicker'
import { MatButtonModule } from '@angular/material/button'

@Component({
  selector: 'app-task-form',
  standalone: true,
  // imports: [CommonModule, ReactiveFormsModule],
  imports: [
    CommonModule,
    ReactiveFormsModule,    
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatButtonModule
  ],
  
  
  template: `
  <<form [formGroup]="form" (ngSubmit)="onSubmit()" class="task-form">
  <mat-form-field appearance="fill" class="full-width">
    <mat-label>כותרת</mat-label>
    <input matInput formControlName="title" />
    <mat-error *ngIf="form.controls.title.invalid && form.controls.title.touched">
      שדה חובה
    </mat-error>
  </mat-form-field>

  <mat-form-field appearance="fill" class="full-width">
    <mat-label>תיאור</mat-label>
    <textarea matInput formControlName="description"></textarea>
  </mat-form-field>

  <mat-form-field appearance="fill" class="full-width">
    <mat-label>תאריך יעד</mat-label>
    <input matInput [matDatepicker]="picker" formControlName="dueDate" />
    <mat-datepicker-toggle matSuffix [for]="picker"></mat-datepicker-toggle>
    <mat-datepicker #picker></mat-datepicker>
    <mat-error *ngIf="form.controls.dueDate.invalid && form.controls.dueDate.touched">
      תאריך לא תקין
    </mat-error>
  </mat-form-field>

  <button mat-raised-button color="primary" type="submit" [disabled]="form.invalid">
    שמור
  </button>
</form>
`

})
export class TaskFormComponent {

  constructor() {
    console.log('TaskFormComponent created');
  }

  private fb = inject(FormBuilder);
  @Input() task: Task | null = null;
  
  @Output() save = new EventEmitter<Task>();

  form = this.fb.nonNullable.group({
    title: ['', Validators.required],
    description: [''],
    dueDate: ['', [Validators.required, this.futureDateValidator]]
  });

  ngOnChanges(changes: SimpleChanges) {
    if (changes['task'] && this.task) {
      this.form.setValue({
        title: this.task.title,
        description: this.task.description || '',
        dueDate: this.task.dueDate
      }
    );
    console.log('Received task ngOnChanges:', this.task);
    }
  }

  ngOnInit() {
    if (this.task) {
      console.log("this.task " +this.task);
      this.form.setValue({
        title: this.task.title,
        description: this.task.description || '',
        dueDate: this.task.dueDate
      });
    }
    else {
      console.log("this.task is empty");
      
    }
    console.log('Received task ngOnInit: ', this.task);
  }

  onSubmit() {
    if (this.form.valid) {
      const value = this.form.getRawValue();
      //const formValue = this.form.value;

      const rawDate = new Date(value.dueDate);
      const dueDate = rawDate.toLocaleDateString('sv-SE');
      const newTask: Task = {
        id: this.task?.id ?? 0,
        title: value.title,
        description: value.description,
        dueDate: dueDate
      };
      this.save.emit(newTask);
    }
  }

  futureDateValidator(control: any) {
    const inputDate = new Date(control.value);
    const today = new Date();
    today.setHours(0,0,0,0);
    return inputDate >= today ? null : { pastDate: true };
  }
}
