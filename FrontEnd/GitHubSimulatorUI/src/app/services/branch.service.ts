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

  getBranches(): Observable<BranchDto[]> {
    return this.http.get<BranchDto[]>(this.baseAddress + '/Branch');
  }

  createBranch(dto: any): Observable<any> {
    return this.http.post<any>('https://localhost:7103/Branch', dto);
  }

}
