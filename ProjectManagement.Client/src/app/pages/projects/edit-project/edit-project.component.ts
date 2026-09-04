import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { ProjectService } from '../../../core/services/project.service';
import { Project } from '../../../core/models/project.model';

@Component({
  selector: 'app-edit-project',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './edit-project.component.html',
  styleUrl: './edit-project.component.css'
})
export class EditProjectComponent implements OnInit {

  private readonly projectService = inject(ProjectService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  projectId = 0;

  project: Partial<Project> = {
    name: '',
    description: '',
    status: 'Active',
    startDate: '',
    dueDate: '',
    ownerId: 0,
    ownerName: '',
    memberCount: 0
  };

  isLoading = false;
  isSaving = false;
  errorMessage = '';

  ngOnInit(): void {

    const id = Number(
      this.route.snapshot.paramMap.get('id')
    );

    if (!id) {
      this.errorMessage = 'Invalid project ID.';
      return;
    }

    this.projectId = id;

    this.loadProject();
  }

  loadProject(): void {

    this.isLoading = true;
    this.errorMessage = '';

    this.projectService
      .getProject(this.projectId)
      .subscribe({

        next: (project) => {

          this.project = {
            ...project,

            startDate: this.formatDate(project.startDate),

            dueDate: project.dueDate
              ? this.formatDate(project.dueDate)
              : ''
          };

          this.isLoading = false;

        },

        error: (error) => {

          console.error(
            'Error loading project:',
            error
          );

          this.errorMessage =
            'Unable to load project. Please try again.';

          this.isLoading = false;

        }

      });

  }

  saveProject(): void {

    if (
      !this.project.name ||
      !this.project.description ||
      !this.project.startDate
    ) {

      this.errorMessage =
        'Please complete all required fields.';

      return;
    }

    this.isSaving = true;
    this.errorMessage = '';

    const updatedProject: Partial<Project> = {

      ...this.project,

      id: this.projectId,

      startDate: this.project.startDate,

      dueDate: this.project.dueDate || null
    };

    this.projectService
      .updateProject(
        this.projectId,
        updatedProject
      )
      .subscribe({

        next: () => {

          console.log(
            'Project updated:',
            this.projectId
          );

          this.isSaving = false;

          this.router.navigate(['/projects']);

        },

        error: (error) => {

          console.error(
            'Error updating project:',
            error
          );

          this.errorMessage =
            'Unable to update project. Please try again.';

          this.isSaving = false;

        }

      });

  }

  cancel(): void {
    this.router.navigate(['/projects']);
  }

  private formatDate(date: string): string {

    if (!date) {
      return '';
    }

    return date.substring(0, 10);
  }

}