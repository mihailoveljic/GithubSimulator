import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  constructor(private router: Router) { }

  ngOnInit() {
  }

  goToLogin() {
    this.router.navigate(['access-control'])
  }

  goToIsssues(){
    this.router.navigate(['issues-page'])
  }

  goToPullRequest(){
    this.router.navigate(['pull-requests-page'])
  }
  goToProjects(){
    this.router.navigate(['projects-page'])
  }
  goToHome(){
    this.router.navigate(['home-page'])
  }

  goToPersonProfile(){
    this.router.navigate(['your-profile-page'])
  }

  goToActions(){}

  goToRepositories(){
    this.router.navigate(['repositories-page'])}

  goToLoginPage(){    
    this.router.navigate(['login-page'])}


  goToCode(){
    this.router.navigate(['code-page'])
  }

  goToSettings(){
    this.router.navigate(['settings-page'])
  }
}
