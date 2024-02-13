import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, of } from 'rxjs';
import { Repository } from 'src/app/modules/repositories/model/Repository';
import { RepositoryService } from 'src/app/services/repository_service.service';
import { Visibility } from '../model/Visibility';



@Component({
  selector: 'app-repo-list',
  templateUrl: './repo-list.component.html',
  styleUrls: ['./repo-list.component.scss']
})
export class RepoListComponent implements OnInit{
  public Visibility = Visibility;
  repositories : Observable<Repository[]> = of([]);
  toggleValue: any;
  searchTerm: any;
  constructor(
    private repositoryService: RepositoryService,
    public toastr: ToastrService,
    private router: Router) {}

  ngOnInit() {
    this.repositories = this.repositoryService.getAllRepositories();
  }

  openDialog(repository: Repository | undefined) {
    this.router.navigate(['/new-repository']);
  }
}
