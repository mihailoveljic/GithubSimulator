import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { RepositoryService } from 'src/app/services/repository_service.service';

@Component({
  selector: 'app-fork-repository',
  templateUrl: './fork-repository.component.html',
  styleUrls: ['./fork-repository.component.scss']
})
export class ForkRepositoryComponent implements OnInit{
  forkName: string = '';
  description: string = '';

  repoName: string = '';
  owner: string = '';

  constructor(private route: ActivatedRoute, private router: Router, private repositoryService: RepositoryService, private toastr: ToastrService) { }

  ngOnInit() {
    this.route.params.subscribe((params) => {
      this.repoName = params['repositoryName'];
      this.owner = params['userName'];
      this.forkName = this.repoName;
    });
  }

  forkRepository() {

    this.repositoryService.forkRepository(this.owner, this.repoName, this.forkName).subscribe(
      response => {
        this.router.navigate(['code', this.owner, this.forkName, 'branch', 'main']);
      },
      error => {
        console.log("Error forking repository.");
        if(error.status == 409){
          this.toastr.error('Repository already exists.')
        }
      }
    );
  }
}
