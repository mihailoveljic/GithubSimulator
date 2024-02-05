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
}
