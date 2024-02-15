import { Injectable } from '@angular/core';
import { environment } from 'src/environment/environment';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class MilestoneService {
  baseAddress: string = environment.API_BASE_URL;

  constructor(private http: HttpClient) {}

  getAll(): Observable<any> {
    return this.http.get(this.baseAddress + '/Milestone/All');
  }

  getMilestoneById(id: string): Observable<any> {
    return this.http.get(this.baseAddress + '/Milestone/?id=' + id);
  }

  getMilestonesForRepo(repoId: string): Observable<any> {
    return this.http.get(
      this.baseAddress + '/Milestone/AllForRepo/?repoId=' + repoId
    );
  }

  getMilestoneProgress(milestoneId: string) {
    return this.http.get(
      this.baseAddress + '/Milestone/getProgress/?milestoneId=' + milestoneId
    );
  }

  deleteMilestone(milestoneId: string) {
    return this.http.delete(
      this.baseAddress + '/Milestone/?id=' + milestoneId
    );
  }

  reopenOrCloseMilestone(milestoneId: string, state: number) {
    return this.http.put(
      this.baseAddress + '/Milestone/reopenOrClose', { id: milestoneId, state: state}
    );
  }

  getOpenOrClosedMilestones(repoId: string, state: number) {
    return this.http.post(
      this.baseAddress +
        '/Milestone/getOpenOrClosed',
        { id: repoId, state: state }
    );
  }

  createMilestone(newMilestoneDto: any) {
    return this.http.post(this.baseAddress + '/Milestone', newMilestoneDto);
  }

  updateMilestone(updateMilestoneDto: any) {
    return this.http.put(this.baseAddress + '/Milestone', updateMilestoneDto);
  }
}
