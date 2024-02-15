import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environment/environment';
import { License } from '../modules/repositories/model/License';

@Injectable({
  providedIn: 'root'
})
export class RemoreRepoService {
  baseAddress: string = environment.GITEA_BASE_URL;
  //headers: HttpHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });

  constructor(private http: HttpClient) { }

  getGitignoreTemplates(): Observable<string[]> {
    return this.http.get<string[]>(this.baseAddress + '/gitignore/templates');
  }

  getLicencesTemplates(): Observable<License[]> {
    return this.http.get<License[]>(this.baseAddress + '/licenses');
  }
}
