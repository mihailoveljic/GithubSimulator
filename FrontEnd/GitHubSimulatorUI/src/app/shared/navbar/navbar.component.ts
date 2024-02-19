import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { filter } from 'rxjs';
import { AuthService } from 'src/app/services/auth.service';
import { SearchEngineService } from 'src/app/services/search-engine.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  page: string = '';
  username: string = '';
  showRepoOptions: boolean = false;

  pathUserName: string = '';
  pathRepositoryName: string = '';

  searchTerm: string = '';
  onSearchTermChange() {
    this.router.navigate(['search-engine'])
    this.searchEngineService.setSearchTerm(this.searchTerm);
  }
  constructor(
    private router: Router,
    private authService: AuthService,
    private toastr: ToastrService,
    private searchEngineService: SearchEngineService,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(() => {
      this.page = window.location.pathname.split('/')[1];
      this.pathUserName = window.location.pathname.split('/')[2];
      this.pathRepositoryName = window.location.pathname.split('/')[3];

      if(this.page === 'code' || 
        this.page === 'issues' || 
        this.page === 'pull-requests' || 
        this.page === 'settings' || 
        this.page == 'milestones' || 
        this.page == 'labels-page')
      {
        this.showRepoOptions = true;
      }
      else{
        this.showRepoOptions = false;
      }
    });
    this.username = this.authService.getUserName();
  }

  goToLogin() {
    this.router.navigate(['access-control'])
  }

  goToIsssues(){
    this.router.navigate(['issues', this.pathUserName, this.pathRepositoryName]);
  }

  goToPullRequest(){
    this.router.navigate(['pull-requests', this.pathUserName, this.pathRepositoryName]);
  }

  goToHome(){
    this.router.navigate(['home-page'])
  }

  goToPersonProfile(){
    this.router.navigate(['repositories', this.authService.getUserName()])
  }

  goToActions(){}

  goToRepositories(){
    this.router.navigate(['repositories', this.authService.getUserName()])}

  goToLoginPage(){
    this.authService.logout();
    this.toastr.success('You are logged out');
    this.router.navigate(['login-page'])
  }

  goToNotificationsPage() {
    this.router.navigate(['notification-page']);
  }

  goToCode(){
    this.router.navigate(['code', this.pathUserName, this.pathRepositoryName, 'branch', 'main']);
  }

  goToSettings(){
    this.router.navigate(['settings', this.pathUserName, this.pathRepositoryName]);
  }

  goToHomePage(){
    this.router.navigate([''])
  }
}
