import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { TaskService } from '../../../core/services/task.service';
import { ProjectService } from '../../../core/services/project.service';
import { UserService, User } from '../../../core/services/user.service';

import { Task, UpdateTask } from '../../../core/models/task.model';
import { Project } from '../../../core/models/project.model';

@Component({
  selector: 'app-edit-task',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './edit-task.component.html',
  styleUrl: './edit-task.component.css'
})
export class EditTaskComponent implements OnInit {

  private readonly taskService = inject(TaskService);
  private readonly projectService = inject(ProjectService);
  private readonly userService = inject(UserService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  taskId!: number;

  task: UpdateTask = {
    title: '',
    description: '',
    status: 'Todo',
    priority: 'Medium',
    dueDate: null,
    projectId: 0,
    assignedToId: null
  };

  projects: Project[] = [];
  users: User[] = [];

  isLoading = true;
  isSaving = false;
  errorMessage = '';

  ngOnInit(): void {

    const id = Number(
      this.route.snapshot.paramMap.get('id')
    );

    if (!id) {
      this.errorMessage = 'Invalid task ID.';
      this.isLoading = false;
      return;
    }

    this.taskId = id;

    this.loadData();
  }

  private loadData(): void {

    this.isLoading = true;
    this.errorMessage = '';

    this.taskService
      .getTask(this.taskId)
      .subscribe({
        next: (task: Task) => {

          this.task = {
            title: task.title,
            description: task.description,
            status: task.status,
            priority: task.priority,
            dueDate: task.dueDate,
            projectId: task.projectId,
            assignedToId: task.assignedToId
          };

          this.loadProjects();
          this.loadUsers();
        },

        error: (error) => {

          console.error(
            'Error loading task:',
            error
          );

          this.errorMessage =
            'Unable to load the task.';

          this.isLoading = false;
        }
      });
  }

  private loadProjects(): void {

    this.projectService
      .getProjects()
      .subscribe({
        next: (projects) => {
          this.projects = projects;
          this.isLoading = false;
        },

        error: (error) => {

          console.error(
            'Error loading projects:',
            error
          );

          this.errorMessage =
            'Unable to load projects.';

          this.isLoading = false;
        }
      });
  }

  private loadUsers(): void {

    this.userService
      .getUsers()
      .subscribe({
        next: (users) => {
          this.users = users;
        },

        error: (error) => {

          console.error(
            'Error loading users:',
            error
          );
        }
      });
  }

  updateTask(): void {

    if (
      !this.task.title.trim() ||
      !this.task.description.trim() ||
      !this.task.projectId
    ) {
      this.errorMessage =
        'Please complete all required fields.';

      return;
    }

    this.isSaving = true;
    this.errorMessage = '';

    this.taskService
      .updateTask(
        this.taskId,
        this.task
      )
      .subscribe({

        next: () => {

          this.isSaving = false;

          this.router.navigate([
            '/tasks'
          ]);
        },

        error: (error) => {

          console.error(
            'Error updating task:',
            error
          );

          this.errorMessage =
            error?.error?.detail ||
            'Unable to update the task.';

          this.isSaving = false;
        }
      });
  }

  cancel(): void {

    this.router.navigate([
      '/tasks'
    ]);
  }

}