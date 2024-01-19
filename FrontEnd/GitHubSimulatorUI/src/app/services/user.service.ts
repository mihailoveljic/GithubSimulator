import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environment/environment';
import { UserDto } from '../dto/userDto';
import { UpdatePasswordDto } from '../dto/updatePasswordDto';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseAddress: string = environment.API_BASE_URL;
  headers: HttpHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });

  constructor(private http: HttpClient) { }

  getUser(): Observable<UserDto> {
    return this.http.get<UserDto>(this.baseAddress + '/User');
  }

  updateUser(userDto: UserDto): Observable<UserDto> {
    return this.http.put<UserDto>(this.baseAddress + '/User', userDto);
  }

  updatePassword(updatePasswordDto: UpdatePasswordDto): Observable<UserDto> {
    return this.http.put<UserDto>(this.baseAddress + '/User/updatePassword', updatePasswordDto);
  }
}
