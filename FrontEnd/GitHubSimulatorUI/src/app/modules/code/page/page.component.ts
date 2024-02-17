import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, startWith, map } from 'rxjs';
import { AuthService } from 'src/app/services/auth.service';



@Component({
  selector: 'app-code-page',
  templateUrl: './page.component.html',
  styleUrls: ['./page.component.scss'], 
})
export class PageComponent{
  constructor(private router: Router, private authService: AuthService, private toastr: ToastrService) { }
  
  goToBranches(){
    this.router.navigate(['branches-page'])}

  
}
