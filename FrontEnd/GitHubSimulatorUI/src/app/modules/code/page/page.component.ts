import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Repository } from '../../repositories/model/Repository';
import { RepositoryService } from 'src/app/services/repository_service.service';
import { Observable, of } from 'rxjs';
import { Visibility } from '../../repositories/model/Visibility';

@Component({
  selector: 'app-code-page',
  templateUrl: './page.component.html',
  styleUrls: ['./page.component.scss']
})
export class PageComponent implements OnInit {
  public Visibility = Visibility;
  repoVisibility: string = '';
  repositoryName: string = '';
  userName: string = '';
  repository: Repository = new Repository();

  constructor(private route: ActivatedRoute, private router: Router, private repoService: RepositoryService) { }

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.repositoryName = params['repositoryName'];
      this.userName = params['userName'];
      this.repoService.getRepository(this.userName, this.repositoryName).subscribe((data) => {
        this.repository = data;
        this.repoVisibility = Visibility[this.repository.visibility];
      });
    });
  }

}
