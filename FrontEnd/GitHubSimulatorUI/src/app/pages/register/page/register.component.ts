import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { RegisterDto } from 'src/app/dto/registerDto';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  username: string = '';
  name: string = '';
  surname: string = '';
  email: string = '';
  password: string = '';
  repeatPassword: string = '';

  constructor(private authService: AuthService, private router: Router, private toastr: ToastrService) { }

  ngOnInit() {
  }

  passwordMismatch: boolean = false;

  checkPasswordMatch() {
    this.passwordMismatch = this.password !== this.repeatPassword;
  }

  register() {
    const registerDto = new RegisterDto(this.name, this.surname, this.email, this.username, this.password);

    this.authService.register(registerDto).subscribe(
      response => {
        this.authService.storeToken(response);
        this.toastr.success('You are registered successfully. Login now!');
        this.router.navigate(['login-page'])
      },
      error => {
        if (error.status === 401 || error.status === 400) {
          console.log(error);
          this.toastr.error('Wrong data');
        } 
      }
    )
  }

  logIn(){
    this.authService.login({email: this.username, password: this.password}).subscribe(
      response => {
        this.authService.storeToken(response);
        this.toastr.success('You are logged in');
        this.router.navigate(['home-page'])
      },
      error => {
        if (error.status === 401) {
          console.log(error);
          this.toastr.error('Wrong username or password');
        } 
      }
    )
  }
}