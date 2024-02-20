import { Component, Input, OnInit } from '@angular/core';
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

  @Input() repositories : Observable<Repository[]> = of([]);;
  @Input() showDescription: boolean = true;
  @Input() showOwner: boolean = true;
  @Input() showSearchBar: boolean = true;
  @Input() showStarButton: boolean = true;
  @Input() fontSize: string = '24px';
  @Input() fontColor: string = '#2F81F7';
  @Input() repoPadding: string = '20px 0px';
  @Input() titleFontWeight: string = '500';

  public Visibility = Visibility;
  toggleValue: any;
  searchTerm: any;
  userName: string = '';


  constructor(
    public toastr: ToastrService,
    private router: Router) {}

  ngOnInit() {}

  openDialog(repository: Repository | undefined) {
    this.router.navigate(['/new-repository']);
  }

  openRepo(repository: Repository) {
    if(repository.owner.username != null) {
      this.router.navigate(['code', repository.owner.username, repository.name, 'branch', 'main']);
    }
    else{
      this.router.navigate(['code', repository.owner, repository.name, 'branch', 'main']);
    }
  }
}
