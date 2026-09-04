import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { Project } from '../../core/models/project.model';
import { ProjectService } from '../../core/services/project.service';

@Component({
  selector: 'app-projects',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './projects.component.html',
  styleUrl: './projects.component.css'
})
export class ProjectsComponent implements OnInit {

  private readonly projectService = inject(ProjectService);
  private readonly router = inject(Router);

  searchTerm = '';
  selectedStatus = 'All';

  projects: Project[] = [];

  isLoading = false;
  errorMessage = '';

  ngOnInit(): void {
    this.loadProjects();
  }

  loadProjects(): void {

    this.isLoading = true;
    this.errorMessage = '';

    this.projectService.getProjects().subscribe({

      next: (projects) => {

        this.projects = projects;

        this.isLoading = false;

        console.log('Projects loaded:', projects);

      },

      error: (error) => {

        console.error(
          'Error loading projects:',
          error
        );

        this.errorMessage =
          'Unable to load projects. Please try again.';

        this.isLoading = false;

      }

    });

  }

  get filteredProjects(): Project[] {

    return this.projects.filter(project => {

      const search =
        this.searchTerm
          .toLowerCase()
          .trim();

      const matchesSearch =
        project.name
          .toLowerCase()
          .includes(search) ||
        project.description
          .toLowerCase()
          .includes(search);

      const matchesStatus =
        this.selectedStatus === 'All' ||
        project.status === this.selectedStatus;

      return matchesSearch && matchesStatus;

    });

  }

  get totalProjects(): number {
    return this.projects.length;
  }

  get activeProjects(): number {
    return this.projects.filter(
      project => project.status === 'Active'
    ).length;
  }

  get completedProjects(): number {
    return this.projects.filter(
      project => project.status === 'Completed'
    ).length;
  }

  get onHoldProjects(): number {
    return this.projects.filter(
      project => project.status === 'On Hold'
    ).length;
  }

  get projectsThisMonth(): number {
  const now = new Date();

  return this.projects.filter(project => {
    const startDate = new Date(project.startDate);

    return (
      startDate.getMonth() === now.getMonth() &&
      startDate.getFullYear() === now.getFullYear()
    );
  }).length;
}

get dueSoonProjects(): number {
  const today = new Date();
  today.setHours(0, 0, 0, 0);

  const sevenDaysFromNow = new Date(today);
  sevenDaysFromNow.setDate(today.getDate() + 7);

  return this.projects.filter(project => {
    if (!project.dueDate) {
      return false;
    }

    const dueDate = new Date(project.dueDate);
    dueDate.setHours(0, 0, 0, 0);

    return dueDate >= today && dueDate <= sevenDaysFromNow;
  }).length;
}

get overdueProjects(): number {
  const today = new Date();
  today.setHours(0, 0, 0, 0);

  return this.projects.filter(project => {
    if (!project.dueDate) {
      return false;
    }

    const dueDate = new Date(project.dueDate);
    dueDate.setHours(0, 0, 0, 0);

    return dueDate < today && project.status !== 'Completed';
  }).length;
}

get averageProgress(): number {
  return 0;
}

  createProject(): void {
    this.router.navigate(['/projects/create']);
  }

  viewProject(project: Project): void {
    this.router.navigate(['/projects', project.id]);
  }

  editProject(project: Project): void {
  console.log('EDIT CLICKED:', project);
  console.log('EDIT ID:', project.id);

  this.router.navigate(['/projects/edit', project.id]);
}

  deleteProject(project: Project): void {

    const confirmed = confirm(
      `Are you sure you want to delete "${project.name}"?`
    );

    if (!confirmed) {
      return;
    }

    this.projectService
      .deleteProject(project.id)
      .subscribe({

        next: () => {

          this.projects = this.projects.filter(
            p => p.id !== project.id
          );

          console.log(
            'Project deleted:',
            project
          );

        },

        error: (error) => {

          console.error(
            'Error deleting project:',
            error
          );

          alert(
            'Unable to delete the project. Please try again.'
          );

        }

      });

  }

}