import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router, Event as RouterEvent } from '@angular/router';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent{
  title = 'GitHubSimulatorUI';
  showNavbar: boolean = true;
  
  constructor(private router: Router, private activatedRoute: ActivatedRoute) {
    this.router.events
      .pipe(filter((event: RouterEvent): event is NavigationEnd => event instanceof NavigationEnd))
      .subscribe((event: NavigationEnd) => {
        this.showNavbar = !(this.isLoginRoute(this.activatedRoute) || this.isRegisterRoute(this.activatedRoute));
      });
  }
  private isLoginRoute(route: ActivatedRoute): boolean {
    while (route.firstChild) {
      route = route.firstChild;
    }
    return route.snapshot.routeConfig?.path === 'login-page';
  }

  private isRegisterRoute(route: ActivatedRoute): boolean {
    while (route.firstChild) {
      route = route.firstChild;
    }
    return route.snapshot.routeConfig?.path === 'register-page';
  }

}
