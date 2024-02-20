import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { Observable, of } from 'rxjs';
import { Repository } from 'src/app/modules/repositories/model/Repository';
import { AuthService } from 'src/app/services/auth.service';
import { RepositoryService } from 'src/app/services/repository_service.service';

@Component({
  selector: 'app-home-page',
  templateUrl: './page.component.html',
  styleUrls: ['./page.component.scss']
})
export class PageComponent implements OnInit{

  repositories : Observable<Repository[]> = of([]);
  userRepositories : Observable<Repository[]> = of([]);

  constructor(private route: ActivatedRoute, private repositoryService: RepositoryService, private authService: AuthService) { }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.repositories = this.repositoryService.getPublicRepositories(1, 20);
      this.userRepositories = this.repositoryService.getAllRepositories(this.authService.getUserName(), 1, 20);
    });
  }
  
}
