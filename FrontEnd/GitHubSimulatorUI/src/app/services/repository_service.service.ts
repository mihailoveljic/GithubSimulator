import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environment/environment';
import { Repository } from '../modules/repositories/model/Repository';
import { InsertRepositoryRequest } from '../modules/repositories/model/dtos/InsertRepositoryRequest';
import { UpdateRepositoryRequest } from '../modules/repositories/model/dtos/UpdateRepositoryRequest';

@Injectable({
  providedIn: 'root'
})
export class RepositoryService {
  baseAddress: string = environment.API_BASE_URL;

  constructor(private http: HttpClient) { }

  getAllRepositories(): Observable<Repository[]> {
    return this.http.get<Repository[]>('https://localhost:7103/Repository');
  }

  getRepositoryById(id: string): Observable<Repository> {
    return this.http.get<Repository>('https://localhost:7103/Repository/${id}');
  }

  createRepository(dto: InsertRepositoryRequest): Observable<Repository> {
    return this.http.post<Repository>('https://localhost:7103/Repository', dto);
  }

  updateRepository(dto: UpdateRepositoryRequest): Observable<Repository> {
    return this.http.put<Repository>('https://localhost:7103/Repository', dto);
  }

  deleteRepository(id: string): Observable<boolean> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      // Add any other headers if needed
    });
    return this.http.delete<boolean>('https://localhost:7103/Repository/'+ id,
    {headers: headers, responseType: 'json'});
  }
}
