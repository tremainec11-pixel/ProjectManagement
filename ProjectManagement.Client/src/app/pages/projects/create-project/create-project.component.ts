import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { ProjectService } from '../../../core/services/project.service';

@Component({
selector: 'app-create-project',
standalone: true,
imports: [
CommonModule,
FormsModule
],
templateUrl: './create-project.component.html',
styleUrl: './create-project.component.css'
})
export class CreateProjectComponent {

private readonly projectService = inject(ProjectService);
private readonly router = inject(Router);

project = {
name: '',
description: '',
status: 'Active',
startDate: '',
dueDate: '',
ownerId: 1
};

isSaving = false;
errorMessage = '';

cancel(): void {
this.router.navigate(['/projects']);
}

createProject(): void {

this.errorMessage = '';

console.log('FORM DATA:', this.project);

if (
!this.project.name.trim() ||
!this.project.description.trim() ||
!this.project.startDate
) {
this.errorMessage =
'Please complete the project name, description and start date.';

return;


}

this.isSaving = true;

const projectData = {
name: this.project.name.trim(),
description: this.project.description.trim(),
status: this.project.status,
startDate: this.project.startDate,
dueDate: this.project.dueDate || undefined,
ownerId: this.project.ownerId
};

console.log('SENDING TO API:', projectData);

this.projectService.createProject(projectData).subscribe({


next: (createdProject) => {

  console.log(
    'PROJECT CREATED SUCCESSFULLY:',
    createdProject
  );

  this.isSaving = false;

  this.router.navigate(['/projects']);

},

error: (error) => {

  console.error(
    'ERROR CREATING PROJECT:',
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
    error?.error?.message ||
    'Unable to create project. Please try again.';

  this.isSaving = false;

}


});

}

}