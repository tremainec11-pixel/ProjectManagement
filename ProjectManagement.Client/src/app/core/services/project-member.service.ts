import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { ProjectMember } from '../models/project-member.model';

@Injectable({
  providedIn: 'root'
})
export class ProjectMemberService {

  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/ProjectMembers`;

  getProjectMembers(projectId: number): Observable<ProjectMember[]> {
    return this.http.get<ProjectMember[]>(
      `${this.apiUrl}/project/${projectId}`
    );
  }

  getProjectMember(id: number): Observable<ProjectMember> {
    return this.http.get<ProjectMember>(`${this.apiUrl}/${id}`);
  }

  createProjectMember(
    projectMember: Partial<ProjectMember>
  ): Observable<ProjectMember> {
    return this.http.post<ProjectMember>(
      this.apiUrl,
      projectMember
    );
  }

  deleteProjectMember(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}