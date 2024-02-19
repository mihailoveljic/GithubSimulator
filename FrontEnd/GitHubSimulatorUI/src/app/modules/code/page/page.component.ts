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

  isUserWatchingRepository: boolean = false;
  isRepositoryStarred: boolean = false;

  constructor(private route: ActivatedRoute, private router: Router, private repoService: RepositoryService) { }

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.repositoryName = params['repositoryName'];
      this.userName = params['userName'];
      
      this.repoService.getRepository(this.userName, this.repositoryName).subscribe((data) => {
        this.repository = data;
        this.repoVisibility = Visibility[this.repository.visibility];
      });

      this.repoService.isUserWatchingRepository(this.userName, this.repositoryName).subscribe((data) => {
        this.isUserWatchingRepository = data;
      });

      this.repoService.isRepositoryStarred(this.userName, this.repositoryName).subscribe((data) => {
        this.isRepositoryStarred = data;
      });
    });
  }

  changeWatchStatus() {
    if(this.isUserWatchingRepository) {
      this.repoService.unwatchRepository(this.userName, this.repositoryName).subscribe(
        response =>{
          this.isUserWatchingRepository = false;
          this.repository.watchers_Count--;
        },
        error => {
          if(error.status != 200){
            this.isUserWatchingRepository = true;
          }
        });
    }
    else {
      this.repoService.watchRepository(this.userName, this.repositoryName).subscribe(
        response =>{
          this.isUserWatchingRepository = true;
          this.repository.watchers_Count++;
        },
        error => {
          if(error.status != 200){
            this.isUserWatchingRepository = false;
          }
        });
    }
  }

  changeStarStatus() {
    if(this.isRepositoryStarred) {
      this.repoService.unstarRepository(this.userName, this.repositoryName).subscribe(
        response =>{
          this.isRepositoryStarred = false;
          this.repository.stars_Count--;
        },
        error => {
          if(error.status != 200){
            this.isRepositoryStarred = true;
          }
        });
    }
    else {
      this.repoService.starRepository(this.userName, this.repositoryName).subscribe(
        response =>{
          this.isRepositoryStarred = true;
          this.repository.stars_Count++;
        },
        error => {
          if(error.status != 200){
            this.isRepositoryStarred = false;
          }
        });
    }
  }

  forkRepo(){
    this.router.navigate(['fork', this.userName, this.repositoryName]);
  }

}
