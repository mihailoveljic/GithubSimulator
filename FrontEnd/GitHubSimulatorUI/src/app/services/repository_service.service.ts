import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environment/environment';
import { Repository } from '../modules/repositories/model/Repository';

@Injectable({
  providedIn: 'root'
})
export class RepositoryService {
  baseAddress: string = environment.API_BASE_URL;

  constructor(private http: HttpClient) { }

  getAllRepositories(): Observable<Repository[]> {
    return this.http.get<Repository[]>('https://localhost:7103/Repository');
  }
  //TODO: connect with BackEnd all CRUD
}
