import { Component, OnInit } from '@angular/core';
import { Observable, of } from 'rxjs';
import { RepoDocument } from '../model/RepoDocument';
import { RepositoryService } from 'src/app/services/repository_service.service';
import { ActivatedRoute, Params, Router } from '@angular/router';

@Component({
  selector: 'app-directory-table',
  templateUrl: './directory-table.component.html',
  styleUrls: ['./directory-table.component.scss']
})
export class DirectoryTableComponent implements OnInit{

  documents : Observable<RepoDocument[]> = of([]);

  repositoryName: string = '';
  branchName: string = '';
  path: string = '';

  constructor(private repositoryService: RepositoryService, private route: ActivatedRoute, private router: Router) { }

  ngOnInit(): void {

    this.route.params.subscribe((params: Params) => {
      if(params['path'] == undefined) {
        this.path = '_home_';
      }
      else {
        this.path = params['path'];
      }
      this.repositoryName = params['repositoryName'];
      this.branchName = params['branchName'];
      this.documents = this.repositoryService.getRepositoryContent(this.repositoryName, this.path, this.branchName);
    });
  }

  openDocument(document: RepoDocument) {
    if(this.path == '_home_') {
      this.router.navigate(['code', this.repositoryName, 'branch', this.branchName, document.name]);
      return;
    }
    this.router.navigate(['code', this.repositoryName, 'branch', this.branchName, this.path, document.name]);
  }

}
