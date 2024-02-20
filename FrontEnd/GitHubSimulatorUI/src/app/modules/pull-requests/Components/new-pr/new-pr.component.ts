import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError, of, throwError } from 'rxjs';
import { IssueService } from 'src/app/services/issue_service.service';
import { PullRequestService } from 'src/app/services/pull-request.service';
import { UserRepositoryService } from 'src/app/services/user-repository.service';

@Component({
  selector: 'app-new-issue',
  templateUrl: './new-pr.component.html',
  styleUrls: ['./new-pr.component.scss'],
})
export class NewPRComponent {
  constructor(private pullRequestService: PullRequestService, private userRepositoryService: UserRepositoryService,
    private router: Router,
    private route: ActivatedRoute,
    private toastr: ToastrService) {}

  repoOwnerName: string = '';
  repoName: string = '';
  repoUserRole: number = -1;

  ngOnInit(): void {
    this.route.params.subscribe((params: any) => {
      this.repoOwnerName = params['userName'];
      this.repoName = params['repositoryName'];

      this.userRepositoryService
        .getAuthenticatedUserRepositoryRole(this.repoName)
        .subscribe((resR: any) => {
          this.repoUserRole = resR;

          if (this.repoUserRole < 1 || this.repoUserRole > 4) {
            this.router.navigate(['/home-page']);
          }
        });
    });
  }

  title: string = '';
  description: string = '';
  branches: any[]=[{"name":"main"},{"name":"grana2"}];
  base: string='';
  head: string='';
  repo:string="first"

  issueDetails: any = {
    title: '',
    description: '',
    assignee: null,
    repositoryId: '',
    milestoneId: null,
    labelIds: null
  };

  updateDataFromChild(data: any) {
    this.issueDetails = data
  }

  submitNewPullRequest() {
    this.issueDetails.title = this.title
    this.issueDetails.description = this.description

    let pom = this.transformData(this.issueDetails)
    this.pullRequestService.createPullRequest(pom, this.repoName).pipe(
      catchError(error => {
        this.toastr.error('Cannot create a pull request');
        this.router.navigate(['pull-requests',this.repoOwnerName, this.repoName]);
        return throwError("");
      })
    )
    .subscribe((res) => {
      this.toastr.success('Pull Request Created Successfully!');
      this.router.navigate(['pull-requests',this.repoOwnerName, this.repoName]);
    });

  }

  transformData(inputData: any): any {
    return {
      source: null,
      target: null,
      assignee: null,
      base: this.base,
      body: inputData.description,
      head: this.head,
      title: inputData.title,
      repoName: this.repoName,
      assignees:null,
      issueId: inputData.issueId,
      milestoneId: inputData.milestoneId,
      repositoryId: inputData.repositoryId,
      isOpen: true,
      number: 0,
      events: null,
      labelIds: inputData.labelIds
    };
  }
  }
