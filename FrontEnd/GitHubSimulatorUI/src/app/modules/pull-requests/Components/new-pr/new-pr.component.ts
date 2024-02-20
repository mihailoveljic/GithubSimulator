import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError, of, throwError } from 'rxjs';
import { IssueService } from 'src/app/services/issue_service.service';
import { PullRequestService } from 'src/app/services/pull-request.service';

@Component({
  selector: 'app-new-issue',
  templateUrl: './new-pr.component.html',
  styleUrls: ['./new-pr.component.scss'],
})
export class NewPRComponent {
  constructor(private pullRequestService: PullRequestService, private router: Router, private toastr: ToastrService) {}

  title: string = '';
  description: string = '';
  branches: any[]=[{"name":"main"},{"name":"branchGrana"}];
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
    console.log("Ovo je pom za kreiranje", pom)
    this.pullRequestService.createPullRequest(pom, this.repo).pipe(
      catchError(error => {
        this.toastr.error('Cannot create a pull request');
        this.router.navigate(['pull-requests-page']);
        return throwError("");
      })
    )
    .subscribe((res) => {
      // Ovo će se izvršiti ako nema greške
      this.toastr.success('Pull Request Created Successfully!');
      this.router.navigate(['pull-requests-page']);
    });

    console.log("Ovo je nesto sto ima od podataka", this.issueDetails)
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
      repoName: this.repo,
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
