import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environment/environment';

@Injectable({
  providedIn: 'root',
})
export class UserRepositoryService {
  baseAddress: string = environment.API_BASE_URL;

  constructor(private http: HttpClient) {}

  getUserRepositoriesByRepositoryName(dto: any) {
    return this.http.post(
      `${this.baseAddress}/UserRepository/GetByRepoName`,
      dto
    );
  }

  getUserRepositoriesByRepositoryNameAlt(dto: any) {
    return this.http.post(
      `${this.baseAddress}/UserRepository/GetByRepoNameAlt`,
      dto
    );
  }

  addUserToRepository(dto: any) {
    return this.http.post(
      `${this.baseAddress}/UserRepository/AddUserToRepo`,
      dto
    );
  }

  removeUserFromRepository(dto: any) {
    return this.http.post(
      `${this.baseAddress}/UserRepository/RemoveUserFromRepo`,
      dto
    );
  }

  changeUserRole(dto: any) {
    return this.http.put(
      `${this.baseAddress}/UserRepository/ChangeUserRole`,
      dto
    );
  }
}
