import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { TaskService } from '../../../core/services/task.service';
import { ProjectService } from '../../../core/services/project.service';
import { UserService, User } from '../../../core/services/user.service';

import { Project } from '../../../core/models/project.model';
import { CreateTask } from '../../../core/models/task.model';

@Component({
selector: 'app-create-task',
standalone: true,
imports: [
CommonModule,
FormsModule
],
templateUrl: './create-task.component.html',
styleUrl: './create-task.component.css'
})
export class CreateTaskComponent implements OnInit {

private readonly taskService = inject(TaskService);
private readonly projectService = inject(ProjectService);
private readonly userService = inject(UserService);
private readonly router = inject(Router);

projects: Project[] = [];
users: User[] = [];

task: CreateTask = {
title: '',
description: '',
status: 'Todo',
priority: 'Medium',
dueDate: null,
projectId: 0,
assignedToId: null
};

isSaving = false;
isLoading = true;
errorMessage = '';

ngOnInit(): void {
this.loadFormData();
}

loadFormData(): void {


this.isLoading = true;
this.errorMessage = '';

this.projectService.getProjects().subscribe({
  next: (projects) => {
    this.projects = projects;

    this.userService.getUsers().subscribe({
      next: (users) => {
        this.users = users;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading users:', error);
        this.errorMessage = 'Unable to load users.';
        this.isLoading = false;
      }
    });
  },
  error: (error) => {
    console.error('Error loading projects:', error);
    this.errorMessage = 'Unable to load projects.';
    this.isLoading = false;
  }
});


}

createTask(): void {


this.errorMessage = '';

if (!this.task.title.trim()) {
  this.errorMessage = 'Task title is required.';
  return;
}

if (!this.task.description.trim()) {
  this.errorMessage = 'Task description is required.';
  return;
}

if (!this.task.projectId || this.task.projectId === 0) {
  this.errorMessage = 'Please select a project.';
  return;
}

this.isSaving = true;

const taskData: CreateTask = {
  title: this.task.title.trim(),
  description: this.task.description.trim(),
  status: this.task.status,
  priority: this.task.priority,
  dueDate: this.task.dueDate || null,
  projectId: Number(this.task.projectId),
  assignedToId: this.task.assignedToId
    ? Number(this.task.assignedToId)
    : null
};

console.log('SENDING TASK TO API:', taskData);

this.taskService.createTask(taskData).subscribe({

  next: (createdTask) => {

    console.log(
      'TASK CREATED SUCCESSFULLY:',
      createdTask
    );

    this.isSaving = false;

    this.router.navigate(['/tasks']);

  },

  error: (error) => {

    console.error(
      'ERROR CREATING TASK:',
      error
    );

    console.error(
      'STATUS:',
      error.status
    );

    console.error(
      'ERROR BODY:',
      error.error
    );

    this.errorMessage =
      error?.error?.detail ||
      error?.error?.message ||
      'Unable to create task. Please try again.';

    this.isSaving = false;

  }

});


}

cancel(): void {
this.router.navigate(['/tasks']);
}

}
