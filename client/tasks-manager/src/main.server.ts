import { bootstrapApplication } from '@angular/platform-browser';
import { TaskListComponent } from './app/components/task-list/task-list.component';
import { provideHttpClient } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';
import { importProvidersFrom } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

export default () => bootstrapApplication(TaskListComponent, {
    providers: [
      provideHttpClient(),
      provideAnimations(),
      importProvidersFrom(ReactiveFormsModule)
    ]
  });
  
