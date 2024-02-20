import { Component, OnInit } from '@angular/core';
import { RepoListComponent } from '../repo-list/repo-list.component';
import { Repository } from '../model/Repository';
import { Observable, of } from 'rxjs';
import { RepositoryService } from 'src/app/services/repository_service.service';
import { ActivatedRoute, Params } from '@angular/router';

@Component({
  selector: 'app-page',
  templateUrl: './page.component.html',
  styleUrls: ['./page.component.scss']
})
export class PageComponent implements OnInit {

  userName: string = '';
  repositories : Observable<Repository[]> = of([]);

  constructor(private route: ActivatedRoute, private repositoryService: RepositoryService) { }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.userName = params['userName'];
      this.repositories = this.repositoryService.getAllRepositories(this.userName, 1, 20);
    });
  }
}
