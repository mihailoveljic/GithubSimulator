import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { IssueService } from 'src/app/services/issue_service.service';
import { DatePipe } from '@angular/common';
import { UserRepositoryService } from 'src/app/services/user-repository.service';

@Component({
  selector: 'app-issue-details',
  templateUrl: './issue-details.component.html',
  styleUrls: ['./issue-details.component.scss'],
})
export class IssueDetailsComponent implements OnInit {
  issueTitleEdited: string = '';

  issueDetails: any = {};

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private issueService: IssueService,
    private userRepositoryService: UserRepositoryService,
    private datePipe: DatePipe
  ) {}

  repoOwnerName: string = '';
  repoName: string = '';
  repoUserRole: number = -1;

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      const issueId = params['id'];
      this.repoOwnerName = params['userName'];
      this.repoName = params['repositoryName'];

      if (this.repoOwnerName === undefined || this.repoName === undefined) {
        let url = this.route.snapshot.url
        this.repoOwnerName = url[0].path
        this.repoName = url[1].path
      }

      this.userRepositoryService
        .getAuthenticatedUserRepositoryRole(this.repoName)
        .subscribe((resR: any) => {
          this.repoUserRole = resR;
        });

      this.issueService.getIssueById(issueId).subscribe((res) => {
        this.issueDetails = res;

        this.issueTitleEdited = this.issueDetails.title;
      });
    });
  }

  commentNum: number = 0;

  isEditing = false;

  startEditing() {
    this.isEditing = true;
  }

  cancelEditing() {
    this.isEditing = false;
    this.issueTitleEdited = this.issueDetails.title;
  }

  confirmEditing() {
    this.isEditing = false;
    this.issueDetails.title = this.issueTitleEdited;

    this.issueService
      .updateIssueTitle(this.issueDetails.id, this.issueDetails.title)
      .subscribe((res: any) => {
        this.issueDetails.title = res.title;
      });
  }

  getFormatedDate(unformatedDateInput: any) {
    if (unformatedDateInput === null || unformatedDateInput === undefined)
      return;
    const unformatedDate = new Date(unformatedDateInput);
    return this.datePipe.transform(unformatedDate, 'dd-MM-yyyy HH:mm');
  }

  openOrCloseIssue(id: string, isOpen: boolean) {
    this.issueService.openOrCloseIssue(id, isOpen).subscribe((res) => {
      this.issueDetails.isOpen = isOpen;
    });
  }

  goToNewIssuePage() {
    this.router.navigate([
      this.repoOwnerName + '/' + this.repoName + '/issues/new',
    ]);
  }
}
