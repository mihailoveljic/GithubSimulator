import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login-page',
  templateUrl: './page.component.html',
  styleUrls: ['./page.component.scss']
})
export class PageComponent implements OnInit {

  constructor(private router: Router) { }

  ngOnInit() {
  }
  logIn(){
    this.router.navigate(['home-page'])
  }
}
