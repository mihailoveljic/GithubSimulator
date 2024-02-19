import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DatePipe } from '@angular/common';
import { PullRequestService } from 'src/app/services/pull-request.service';

@Component({
  selector: 'app-issue-details',
  templateUrl: './pr-details.component.html',
  styleUrls: ['./pr-details.component.scss'],
})
export class PRDetailsComponent implements OnInit {
  pullTitleEdited: string = '';

  pullDetails: any = {};
  pullRemote: any ={};

  transformData(inputData: any): any {
    return {
      source: inputData.source,
      target: inputData.target,
      assignee: inputData.assignee,
      base: inputData.base,
      body: inputData.body,
      head: inputData.head,
      title: inputData.title,
      repoName: inputData.repoName,
      assignees:inputData.assignees,
      issueId: inputData.issueId,
      milestoneId: inputData.milestoneId,
      repositoryId: inputData.repositoryId,
      isOpen: inputData.isOpen,
      number: inputData.number,
      events: inputData.events,
      labelIds: inputData.labels.map((label: any) => label.id)
    };
  }
  

  constructor(
    private route: ActivatedRoute,
    private pullRequestService: PullRequestService,
    private datePipe: DatePipe
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      const pullId = params['id'];
      this.pullRequestService.getPullRequestById(pullId).subscribe((res) => {
        this.pullDetails = res;
        console.log("Ovde se nesto desilo", this.pullDetails)
        this.pullRequestService.getPullRequestByIndex(this.pullDetails.repoName, this.pullDetails.number).subscribe((res)=>{
          this.pullRemote = res;
        });
        localStorage.setItem("pullId", this.pullDetails.id);
        this.pullTitleEdited = this.pullDetails.title;
      });
      console.log("Ovde se nesto desilo", this.pullDetails)
      
    });
  }

  commentNum: number = 0;

  isEditing = false;

  startEditing() {
    this.isEditing = true;
  }

  cancelEditing() {
    this.isEditing = false;
    this.pullTitleEdited = this.pullDetails.title;
  }

  confirmEditing() {
    this.isEditing = false;
    this.pullDetails.title = this.pullTitleEdited;

    this.pullRequestService
      .updatePullRequest(this.pullDetails.id, this.transformData(this.pullDetails))
      .subscribe((res) => {
      });
  }

  getFormatedDate(unformatedDateInput: any) {
    if (unformatedDateInput === null || unformatedDateInput === undefined)
      return;
    const unformatedDate = new Date(unformatedDateInput);
    return this.datePipe.transform(unformatedDate, 'dd-MM-yyyy HH:mm');
  }

  openOrCloseIssue(id: string, isOpen: boolean) {
    this.pullDetails.isOpen = isOpen
    this.pullRequestService.updatePullRequest(id, this.transformData(this.pullDetails)).subscribe((res) => {
    });
  }
}
