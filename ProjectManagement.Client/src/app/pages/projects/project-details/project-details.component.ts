import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';

import { Project } from '../../../core/models/project.model';
import { ProjectService } from '../../../core/services/project.service';

@Component({
  selector: 'app-project-details',
  standalone: true,
  imports: [
    CommonModule
  ],
  templateUrl: './project-details.component.html',
  styleUrl: './project-details.component.css'
})
export class ProjectDetailsComponent implements OnInit {

  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly projectService = inject(ProjectService);

  project: Project | null = null;

  isLoading = false;
  errorMessage = '';

  ngOnInit(): void {

    const projectId = Number(
      this.route.snapshot.paramMap.get('id')
    );

    if (!projectId) {

      this.errorMessage =
        'Invalid project ID.';

      return;

    }

    this.loadProject(projectId);

  }

  loadProject(id: number): void {

    this.isLoading = true;
    this.errorMessage = '';

    this.projectService.getProject(id).subscribe({

      next: (project) => {

        this.project = project;

        this.isLoading = false;

        console.log(
          'Project details loaded:',
          project
        );

      },

      error: (error) => {

        console.error(
          'Error loading project details:',
          error
        );

        this.errorMessage =
          'Unable to load project details. Please try again.';

        this.isLoading = false;

      }

    });

  }

  goBack(): void {

    this.router.navigate(['/projects']);

  }

}