import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';

import { StatCardComponent } from '../../shared/components/stat-card/stat-card.component';

import { UserService, User } from '../../core/services/user.service';
import { ProjectService } from '../../core/services/project.service';
import { TaskService } from '../../core/services/task.service';
import { ActivityService } from '../../core/services/activity.service';

import { Project } from '../../core/models/project.model';
import { Task } from '../../core/models/task.model';
import { Activity } from '../../core/models/activity.model';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    MatIconModule,
    StatCardComponent
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {

  private readonly projectService = inject(ProjectService);
  private readonly taskService = inject(TaskService);
  private readonly userService = inject(UserService);
  private readonly activityService = inject(ActivityService);

  readonly router = inject(Router);

  // ================================
  // Dashboard Data
  // ================================

  projects: Project[] = [];
  tasks: Task[] = [];
  users: User[] = [];
  activities: Activity[] = [];

  recentProjects: Project[] = [];
  recentTasks: Task[] = [];
  recentActivities: Activity[] = [];

  // ================================
  // Loading & Error State
  // ================================

  isLoading = true;
  errorMessage = '';

  // ================================
  // Project Statistics
  // ================================

  totalProjects = 0;
  activeProjects = 0;
  completedProjects = 0;

  // ================================
  // Task Statistics
  // ================================

  totalTasks = 0;
  activeTasks = 0;
  completedTasks = 0;

  // ================================
  // User Statistics
  // ================================

  totalUsers = 0;

  // ================================
  // Lifecycle
  // ================================

  ngOnInit(): void {
    this.loadDashboard();
  }

  // ================================
  // Load Dashboard
  // ================================

  loadDashboard(): void {

    this.isLoading = true;
    this.errorMessage = '';

    this.loadProjects();
    this.loadUsers();
    this.loadActivities();

  }

  // ================================
  // Load Projects
  // ================================

  private loadProjects(): void {

    this.projectService.getProjects().subscribe({

      next: (projects: Project[]) => {

        this.projects = projects ?? [];

        this.calculateProjectStats();

        this.recentProjects =
          this.getRecentProjects();

        // Once projects are loaded,
        // load tasks
        this.loadTasks();

      },

      error: (error: unknown) => {

        console.error(
          'Error loading projects:',
          error
        );

        this.errorMessage =
          'Unable to load dashboard data.';

        this.isLoading = false;

      }

    });

  }

  // ================================
  // Load Tasks
  // ================================

  private loadTasks(): void {

    this.taskService.getTasks().subscribe({

      next: (tasks: Task[]) => {

        this.tasks = tasks ?? [];

        this.calculateTaskStats();

        this.recentTasks =
          this.getRecentTasks();

        this.isLoading = false;

      },

      error: (error: unknown) => {

        console.error(
          'Error loading tasks:',
          error
        );

        this.errorMessage =
          'Unable to load dashboard data.';

        this.isLoading = false;

      }

    });

  }

  // ================================
  // Load Users
  // ================================

  private loadUsers(): void {

    this.userService.getUsers().subscribe({

      next: (users: User[]) => {

        this.users = users ?? [];

        this.totalUsers =
          this.users.length;

      },

      error: (error: unknown) => {

        console.error(
          'Error loading users:',
          error
        );

        // We don't stop the entire dashboard
        // if users fail to load.
        this.totalUsers = 0;

      }

    });

  }

  // ================================
  // Load Activities
  // ================================

  private loadActivities(): void {

    this.activityService.getActivities().subscribe({

      next: (activities: Activity[]) => {

        this.activities = activities ?? [];

        this.recentActivities =
          this.getRecentActivities();

      },

      error: (error: unknown) => {

        console.error(
          'Error loading activities:',
          error
        );

        // Activities should not break
        // the entire dashboard.
        this.activities = [];
        this.recentActivities = [];

      }

    });

  }

  // ================================
  // Project Statistics
  // ================================

  private calculateProjectStats(): void {

    this.totalProjects =
      this.projects.length;

    this.activeProjects =
      this.projects.filter(project =>
        project.status?.toLowerCase() === 'active'
      ).length;

    this.completedProjects =
      this.projects.filter(project =>
        project.status?.toLowerCase() === 'completed'
      ).length;

  }

  // ================================
  // Task Statistics
  // ================================

  private calculateTaskStats(): void {

    this.totalTasks =
      this.tasks.length;

    this.activeTasks =
      this.tasks.filter(task =>
        task.status?.toLowerCase() === 'in progress'
      ).length;

    this.completedTasks =
      this.tasks.filter(task =>
        task.status?.toLowerCase() === 'completed'
      ).length;

  }

  // ================================
  // Recent Projects
  // ================================

  private getRecentProjects(): Project[] {

    return [...this.projects]
      .sort((a, b) =>
        new Date(b.createdAt).getTime() -
        new Date(a.createdAt).getTime()
      )
      .slice(0, 5);

  }

  // ================================
  // Recent Tasks
  // ================================

  private getRecentTasks(): Task[] {

    return [...this.tasks]
      .sort((a, b) =>
        new Date(b.createdAt).getTime() -
        new Date(a.createdAt).getTime()
      )
      .slice(0, 5);

  }

  // ================================
  // Recent Activities
  // ================================

  private getRecentActivities(): Activity[] {

    return [...this.activities]
      .sort((a, b) =>
        new Date(b.createdAt).getTime() -
        new Date(a.createdAt).getTime()
      )
      .slice(0, 5);

  }

  // ================================
  // Navigation
  // ================================

  createProject(): void {

    this.router.navigate([
      '/projects/create'
    ]);

  }

  viewProject(project: Project): void {

    this.router.navigate([
      '/projects',
      project.id
    ]);

  }

  // ================================
  // Project Helpers
  // ================================

  getProjectStatus(project: Project): string {

    return project.status || 'Unknown';

  }

}

