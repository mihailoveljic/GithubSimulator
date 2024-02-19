import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environment/environment';
import { Repository } from '../modules/repositories/model/Repository';
import { InsertRepositoryRequest } from '../modules/repositories/model/dtos/InsertRepositoryRequest';
import { UpdateRepositoryRequest } from '../modules/repositories/model/dtos/UpdateRepositoryRequest';
import { RepoDocument } from '../modules/code/model/RepoDocument';

@Injectable({
  providedIn: 'root',
})
export class RepositoryService {
  baseAddress: string = environment.API_BASE_URL;

  constructor(private http: HttpClient) {}

  getAllRepositories(owner: string, page: number, limit: number): Observable<Repository[]> {
    return this.http.get<Repository[]>(`${this.baseAddress}/Repository/${owner}?page=${page}&limit=${limit}`);
  }

  getRepositoryById(id: string): Observable<Repository> {
    return this.http.get<Repository>('https://localhost:7103/Repository/${id}');
  }

  getRepository(owner: string, repoName: string): Observable<Repository> {
    return this.http.get<Repository>(`${this.baseAddress}/Repository/${owner}/${repoName}`);
  }

  createRepository(dto: InsertRepositoryRequest): Observable<Repository> {
    return this.http.post<Repository>('https://localhost:7103/Repository', dto);
  }

  getRepositoryByName(repoName: string) {
    return this.http.get(
      'https://localhost:7103/Repository/GetByName/' + repoName
    );
  }

  updateRepository(dto: UpdateRepositoryRequest): Observable<Repository> {
    return this.http.put<Repository>('https://localhost:7103/Repository', dto);
  }

  updateRepositoryName(dto: any) {
    return this.http.put('https://localhost:7103/Repository/UpdateName', dto);
  }

  updateRepositoryVisibility(dto: any) {
    return this.http.put(
      'https://localhost:7103/Repository/UpdateVisibility',
      dto
    );
  }

  updateRepositoryArchivedState(dto: any) {
    return this.http.put(
      'https://localhost:7103/Repository/UpdateArchivedState',
      dto
    );
  }

  updateRepositoryOwner(dto: any) {
    return this.http.post('https://localhost:7103/Repository/UpdateOwner', dto);
  }

  deleteRepository(name: string): Observable<boolean> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      // Add any other headers if needed
    });
    return this.http.delete<boolean>(
      'https://localhost:7103/Repository/' + name,
      { headers: headers, responseType: 'json' }
    );
  }

  getRepositoryContent(repositoryName: string, path: string, branchName: string): Observable<RepoDocument[]> {
    return this.http.get<RepoDocument[]>(`${this.baseAddress}/Repository/${repositoryName}/content/${path}?branchName=${branchName}`);
  }

  getUserRepositoryContent(owner: string, repositoryName: string, path: string, branchName: string): Observable<RepoDocument[]> {
    return this.http.get<RepoDocument[]>(`${this.baseAddress}/Repository/${owner}/${repositoryName}/content/${path}?branchName=${branchName}`);
  }

  starRepository(owner: string, repositoryName: string): Observable<any> {
    return this.http.put<any>(`${this.baseAddress}/Repository/star/${owner}/${repositoryName}`, {});
  }

  isRepositoryStarred(owner: string, repositoryName: string): Observable<boolean> {
    return this.http.get<boolean>(`${this.baseAddress}/Repository/star/${owner}/${repositoryName}`);
  }

  unstarRepository(owner: string, repositoryName: string): Observable<any> {
    return this.http.delete<any>(`${this.baseAddress}/Repository/star/${owner}/${repositoryName}`);
  }

  watchRepository(owner: string, repositoryName: string): Observable<any> {
    return this.http.put<any>(`${this.baseAddress}/Repository/watch/${owner}/${repositoryName}`, {});
  }

  isUserWatchingRepository(owner: string, repositoryName: string): Observable<boolean> {
    return this.http.get<boolean>(`${this.baseAddress}/Repository/watch/${owner}/${repositoryName}`);
  }

  unwatchRepository(owner: string, repositoryName: string): Observable<any> {
    return this.http.delete<any>(`${this.baseAddress}/Repository/watch/${owner}/${repositoryName}`);
  }

  forkRepository(owner: string, repositoryName: string, forkName: string): Observable<Repository> {
    return this.http.post<Repository>(`${this.baseAddress}/Repository/fork/${owner}/${repositoryName}?forkName=${forkName}`, {});
  }
}
