import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-login-page',
  templateUrl: './page.component.html',
  styleUrls: ['./page.component.scss']
})
export class PageComponent implements OnInit {

  username: string = '';
  password: string = '';

  constructor(private authService: AuthService, private router: Router, private toastr: ToastrService) { }

  ngOnInit() {
  }
  logIn(){
    this.authService.login({email: this.username, password: this.password}).subscribe(
      response => {
        this.authService.storeToken(response);
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
