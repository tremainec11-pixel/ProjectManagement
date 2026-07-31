import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

import { TaskService } from '../../core/services/task.service';
import { ProjectService } from '../../core/services/project.service';
import { UserService, User } from '../../core/services/user.service';

import { Task } from '../../core/models/task.model';
import { Project } from '../../core/models/project.model';

@Component({
selector: 'app-tasks',
standalone: true,
imports: [
CommonModule
],
templateUrl: './tasks.component.html',
styleUrl: './tasks.component.css'
})
export class TasksComponent implements OnInit {

private readonly taskService = inject(TaskService);
private readonly projectService = inject(ProjectService);
private readonly userService = inject(UserService);
private readonly router = inject(Router);

tasks: Task[] = [];
projects: Project[] = [];
users: User[] = [];

isLoading = false;
errorMessage = '';

ngOnInit(): void {
this.loadData();
}

loadData(): void {


this.isLoading = true;
this.errorMessage = '';

this.taskService.getTasks().subscribe({

  next: (tasks) => {

    console.log('TASKS LOADED:', tasks);

    this.tasks = tasks;

    this.loadProjects();
    this.loadUsers();

  },

  error: (error) => {

    console.error(
      'ERROR LOADING TASKS:',
      error
    );

    this.errorMessage =
      'Unable to load tasks. Please try again.';

    this.isLoading = false;

  }

});


}

private loadProjects(): void {


this.projectService.getProjects().subscribe({

  next: (projects) => {

    console.log(
      'PROJECTS LOADED:',
      projects
    );

    this.projects = projects;

    this.isLoading = false;

  },

  error: (error) => {

    console.error(
      'ERROR LOADING PROJECTS:',
      error
    );

    this.errorMessage =
      'Unable to load projects.';

    this.isLoading = false;

  }

});


}

private loadUsers(): void {


this.userService.getUsers().subscribe({

  next: (users) => {

    console.log(
      'USERS LOADED:',
      users
    );

    this.users = users;

  },

  error: (error) => {

    console.error(
      'ERROR LOADING USERS:',
      error
    );

  }

});


}

getProjectName(projectId: number): string {


const project = this.projects.find(
  p => p.id === projectId
);

return project?.name || `Project #${projectId}`;


}

getUserName(
assignedToId: number | null,
assignedToName: string | null
): string {


if (!assignedToId) {
  return 'Unassigned';
}

const user = this.users.find(
  u => u.id === assignedToId
);

if (user) {
  return `${user.firstName} ${user.lastName}`;
}

if (
  assignedToName &&
  assignedToName !== 'string string'
) {
  return assignedToName;
}

return 'Unassigned';


}

createTask(): void {


this.router.navigate([
  '/tasks/create'
]);


}

editTask(task: Task): void {


this.router.navigate([
  '/tasks/edit',
  task.id
]);


}

deleteTask(task: Task): void {


const confirmed = confirm(
  `Are you sure you want to delete "${task.title}"?`
);

if (!confirmed) {
  return;
}

this.taskService.deleteTask(task.id).subscribe({

  next: () => {

    console.log(
      'TASK DELETED:',
      task.id
    );

    this.tasks = this.tasks.filter(
      t => t.id !== task.id
    );

  },

  error: (error) => {

    console.error(
      'ERROR DELETING TASK:',
      error
    );

    this.errorMessage =
      'Unable to delete task. Please try again.';

  }

});


}

}
