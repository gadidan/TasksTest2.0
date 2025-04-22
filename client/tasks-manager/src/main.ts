// src/main.ts
import { bootstrapApplication } from '@angular/platform-browser';
import { TaskListComponent } from './app/components/task-list/task-list.component';
import { provideHttpClient } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';
import { importProvidersFrom } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { Directionality } from '@angular/cdk/bidi';
import { MatNativeDateModule } from '@angular/material/core';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';

bootstrapApplication(TaskListComponent, {
  providers: [
    provideHttpClient(),
    provideAnimations(),    
    importProvidersFrom(
      ReactiveFormsModule,
      MatNativeDateModule // needed for datepicker
    ), provideAnimationsAsync()
  ]
}).catch(err => console.error(err));
