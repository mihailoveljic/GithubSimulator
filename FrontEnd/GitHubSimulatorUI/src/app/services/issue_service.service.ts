import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environment/environment';

@Injectable({
  providedIn: 'root'
})
export class IssueService {
  baseAddress: string = environment.API_BASE_URL;

  constructor(private http: HttpClient) { }

  getAllIssues(): Observable<any> {
    return this.http.get(this.baseAddress + '/githubsimulator');
  }
}
