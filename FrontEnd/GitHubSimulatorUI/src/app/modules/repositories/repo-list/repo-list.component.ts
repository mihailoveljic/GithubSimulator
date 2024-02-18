import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
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
  userName: string = '';


  constructor(
    private repositoryService: RepositoryService,
    public toastr: ToastrService,
    private router: Router,
    private route: ActivatedRoute) {}

  ngOnInit() {
    this.route.params.subscribe((params: Params) => {
      this.userName = params['userName'];
      this.repositories = this.repositoryService.getAllRepositories(this.userName, 1, 20);
      console.log(this.repositories)
    });
  }

  openDialog(repository: Repository | undefined) {
    this.router.navigate(['/new-repository']);
  }

  openRepo(repository: Repository) {
    this.router.navigate(['code', this.userName, repository.name, 'branch', 'main']);
  }
}
