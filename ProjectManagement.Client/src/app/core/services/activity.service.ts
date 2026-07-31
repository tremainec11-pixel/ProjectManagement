import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { Activity } from '../models/activity.model';

@Injectable({
  providedIn: 'root'
})
export class ActivityService {

  private readonly http = inject(HttpClient);

  private readonly apiUrl =
    `${environment.apiUrl}/Activities`;

  /**
   * Get all activities
   */
  getActivities(): Observable<Activity[]> {
    return this.http.get<Activity[]>(
      this.apiUrl
    );
  }

  /**
   * Get activity by id
   */
  getActivity(id: number): Observable<Activity> {
    return this.http.get<Activity>(
      `${this.apiUrl}/${id}`
    );
  }

  /**
   * Create activity
   */
  createActivity(
    activity: Partial<Activity>
  ): Observable<Activity> {

    return this.http.post<Activity>(
      this.apiUrl,
      activity
    );
  }

  /**
   * Delete activity
   */
  deleteActivity(
    id: number
  ): Observable<void> {

    return this.http.delete<void>(
      `${this.apiUrl}/${id}`
    );
  }

}