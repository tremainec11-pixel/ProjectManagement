import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { ProjectService } from '../../core/services/project.service';
import { ProjectMemberService } from '../../core/services/project-member.service';
import { UserService, User } from '../../core/services/user.service';

import { Project } from '../../core/models/project.model';
import { ProjectMember } from '../../core/models/project-member.model';

@Component({
  selector: 'app-team',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './team.component.html',
  styleUrl: './team.component.css'
})
export class TeamComponent implements OnInit {

  private readonly projectService = inject(ProjectService);
  private readonly projectMemberService = inject(ProjectMemberService);
  private readonly userService = inject(UserService);

  // Projects
  projects: Project[] = [];
  selectedProjectId: number = 0;

  // Users
  users: User[] = [];
  selectedUserId: number = 0;

  // Team members
  members: ProjectMember[] = [];

  // Form
  selectedRole: string = 'Member';

  // Loading states
  isLoadingProjects = false;
  isLoadingUsers = false;
  isLoadingMembers = false;
  isAddingMember = false;

  // Messages
  successMessage = '';
  errorMessage = '';

  ngOnInit(): void {
    this.loadProjects();
    this.loadUsers();
  }

  // =========================
  // LOAD PROJECTS
  // =========================

  loadProjects(): void {

    this.isLoadingProjects = true;
    this.errorMessage = '';

    this.projectService.getProjects().subscribe({

      next: (projects) => {

        console.log(
          'PROJECTS LOADED FOR TEAM:',
          projects
        );

        this.projects = projects;

        this.isLoadingProjects = false;
      },

      error: (error) => {

        console.error(
          'ERROR LOADING PROJECTS:',
          error
        );

        this.errorMessage =
          'Unable to load projects. Please try again.';

        this.isLoadingProjects = false;
      }

    });
  }

  // =========================
  // LOAD USERS
  // =========================

  loadUsers(): void {

    this.isLoadingUsers = true;

    this.userService.getUsers().subscribe({

      next: (users) => {

        console.log(
          'USERS LOADED FOR TEAM:',
          users
        );

        this.users = users;

        this.isLoadingUsers = false;
      },

      error: (error) => {

        console.error(
          'ERROR LOADING USERS:',
          error
        );

        this.errorMessage =
          'Unable to load users. Please try again.';

        this.isLoadingUsers = false;
      }

    });
  }

  // =========================
  // PROJECT CHANGE
  // =========================

  onProjectChange(): void {

    this.successMessage = '';
    this.errorMessage = '';

    this.members = [];

    this.selectedUserId = 0;
    this.selectedRole = 'Member';

    if (!this.selectedProjectId) {
      return;
    }

    console.log(
      'SELECTED PROJECT:',
      this.selectedProjectId
    );

    this.loadMembers();
  }

  // =========================
  // LOAD MEMBERS
  // =========================

  loadMembers(): void {

    if (!this.selectedProjectId) {
      return;
    }

    this.isLoadingMembers = true;
    this.errorMessage = '';

    this.projectMemberService
      .getProjectMembers(this.selectedProjectId)
      .subscribe({

        next: (members) => {

          console.log(
            'PROJECT MEMBERS LOADED:',
            members
          );

          this.members = members;

          this.isLoadingMembers = false;
        },

        error: (error) => {

          console.error(
            'ERROR LOADING PROJECT MEMBERS:',
            error
          );

          this.errorMessage =
            'Unable to load team members. Please try again.';

          this.isLoadingMembers = false;
        }

      });
  }

  // =========================
  // ADD MEMBER
  // =========================

  addMember(): void {

    this.successMessage = '';
    this.errorMessage = '';

    if (!this.selectedProjectId) {

      this.errorMessage =
        'Please select a project.';

      return;
    }

    if (!this.selectedUserId) {

      this.errorMessage =
        'Please select a team member.';

      return;
    }

    this.isAddingMember = true;

    const memberData = {

      projectId: this.selectedProjectId,

      userId: this.selectedUserId,

      role: this.selectedRole

    };

    console.log(
      'ADDING PROJECT MEMBER:',
      memberData
    );

    this.projectMemberService
      .createProjectMember(memberData)
      .subscribe({

        next: (member) => {

          console.log(
            'PROJECT MEMBER ADDED:',
            member
          );

          this.successMessage =
            'Team member added successfully.';

          this.isAddingMember = false;

          this.selectedUserId = 0;

          this.selectedRole = 'Member';

          this.loadMembers();
        },

        error: (error) => {

          console.error(
            'ERROR ADDING PROJECT MEMBER:',
            error
          );

          this.errorMessage =
            error?.error?.detail ||
            error?.error?.message ||
            'Unable to add team member. Please try again.';

          this.isAddingMember = false;
        }

      });
  }

  // =========================
  // REMOVE MEMBER
  // =========================

  removeMember(member: ProjectMember): void {

    const confirmed = confirm(
      `Are you sure you want to remove "${member.userName}" from this project?`
    );

    if (!confirmed) {
      return;
    }

    this.successMessage = '';
    this.errorMessage = '';

    this.projectMemberService
      .deleteProjectMember(member.id)
      .subscribe({

        next: () => {

          console.log(
            'PROJECT MEMBER REMOVED:',
            member.id
          );

          this.successMessage =
            'Team member removed successfully.';

          this.members = this.members.filter(
            m => m.id !== member.id
          );
        },

        error: (error) => {

          console.error(
            'ERROR REMOVING PROJECT MEMBER:',
            error
          );

          this.errorMessage =
            'Unable to remove team member. Please try again.';
        }

      });
  }

}

