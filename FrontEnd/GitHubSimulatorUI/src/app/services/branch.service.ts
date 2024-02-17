import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environment/environment';
import { UserDto } from '../dto/userDto';
import { UpdatePasswordDto } from '../dto/updatePasswordDto';
import { BranchDto } from '../dto/branchDto';

@Injectable({
  providedIn: 'root'
})
export class BranchService {
  baseAddress: string = environment.API_BASE_URL;
  headers: HttpHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });

  constructor(private http: HttpClient) { }

  getBranches(repo: string, page: number, limit: number): Observable<BranchDto[]> {
    return this.http.get<BranchDto[]>(`${this.baseAddress}/Branch/RepoBranches?repo=${repo}&page=${page}&limit=${limit}`);
  }
  createBranch(dto: any ,repo: string): Observable<any> {
    return this.http.post<any>(`${this.baseAddress}/Branch?repo=${repo}`, dto);
  }

  deleteBranch(id: string): Observable<any> {
    return this.http.delete<any>(this.baseAddress+'/Branch/'+ id,
    {headers: this.headers, responseType: 'json'});
  }


  deleteBranchGitea(repo: string, branch: string): Observable<any> {
    return this.http.delete<any>(`${this.baseAddress}/Branch/Delete/${repo}/${branch}`);
  }
 
  updateBranch(dto: any): Observable<any> {
    return this.http.put<any>('https://localhost:7103/Branch', dto);
  }

}
