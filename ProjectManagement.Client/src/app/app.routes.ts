import { Routes } from '@angular/router';

import { AppLayoutComponent } from './layout/app-layout/app-layout.component';

import { DashboardComponent } from './pages/dashboard/dashboard.component';

import { ProjectsComponent } from './pages/projects/projects.component';
import { CreateProjectComponent } from './pages/projects/create-project/create-project.component';
import { ProjectDetailsComponent } from './pages/projects/project-details/project-details.component';

import { TasksComponent } from './pages/tasks/tasks.component';
import { CreateTaskComponent } from './pages/tasks/create-task/create-task.component';
import { EditTaskComponent } from './pages/tasks/edit-task/edit-task.component';

import { TeamComponent } from './pages/team/team.component';
import { MembersComponent } from './pages/members/members.component';
import { SettingsComponent } from './pages/settings/settings.component';


export const routes: Routes = [

  {
    path: '',
    component: AppLayoutComponent,

    children: [

      // Dashboard

      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full'
      },

      {
        path: 'dashboard',
        component: DashboardComponent
      },


      // Projects

      {
        path: 'projects',
        component: ProjectsComponent
      },

      {
        path: 'projects/create',
        component: CreateProjectComponent
      },

      {
        path: 'projects/:id',
        component: ProjectDetailsComponent
      },


      // Tasks

      {
        path: 'tasks',
        component: TasksComponent
      },

      {
        path: 'tasks/create',
        component: CreateTaskComponent
      },

      {
        path: 'tasks/edit/:id',
        component: EditTaskComponent
      },


      // Team

      {
        path: 'team',
        component: TeamComponent
      },


      // Members

      {
        path: 'members',
        component: MembersComponent
      },


      // Settings

      {
        path: 'settings',
        component: SettingsComponent
      }

    ]
  },


  // Unknown routes

  {
    path: '**',
    redirectTo: 'dashboard'
  }

];