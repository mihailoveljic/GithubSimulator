import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IssueService } from 'src/app/services/issue_service.service';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-issue-details',
  templateUrl: './issue-details.component.html',
  styleUrls: ['./issue-details.component.scss'],
})
export class IssueDetailsComponent implements OnInit {
  issueTitleEdited: string = '';

  //issueDetails: any = {'author': {'email': ''}, 'assigne': {'email': ''}}
  issueDetails: any = {};

  constructor(
    private route: ActivatedRoute,
    private issueService: IssueService,
    private datePipe: DatePipe
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      const issueId = params['id'];
      this.issueService.getIssueById(issueId).subscribe((res) => {
        this.issueDetails = res;
        console.log('Issue details:');
        console.log(this.issueDetails);

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
      .subscribe((res) => {
        this.issueDetails = res;
      });
  }

  getFormatedDate(unformatedDateInput: any) {
    if (unformatedDateInput === null || unformatedDateInput === undefined)
      return;
    const unformatedDate = new Date(unformatedDateInput);
    return this.datePipe.transform(unformatedDate, 'dd-MM-yyyy HH:mm');
  }

  // TODO add events
  openOrCloseIssue(id: string, isOpen: boolean) {
    this.issueService.openOrCloseIssue(id, isOpen).subscribe((res) => {
      this.issueDetails.isOpen = isOpen
    });
  }
}
