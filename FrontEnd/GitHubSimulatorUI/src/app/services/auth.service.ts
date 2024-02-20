import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environment/environment';
import { LoginDto } from '../dto/loginDto';
import { RegisterDto } from '../dto/registerDto';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseAddress: string = environment.API_BASE_URL;
  headers: HttpHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });

  constructor(private http: HttpClient, private router: Router) { }

  login(loginDto: LoginDto): Observable<string> {
    return this.http.post(this.baseAddress + '/User/login', loginDto, { responseType: 'text' });
  }

  register(registerDto: RegisterDto): Observable<string> {
    return this.http.post(this.baseAddress + '/User/register', registerDto, { responseType: 'text' });
  }

  storeToken(token: string) {
    localStorage.setItem('token', token);
  }

  getToken(){
    return localStorage.getItem('token');
  }


  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  logout() {
    localStorage.clear();
    this.router.navigate(['login-page']);
  }

  getUserName(): string {
    const token = this.getToken();
    if(token == null) {
      return '';
    }
    const payload = this.parseJwt(token);
    return payload['unique_name']; 
  }

  private parseJwt(token: string) {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(atob(base64).split('').map(function(c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
};
}
