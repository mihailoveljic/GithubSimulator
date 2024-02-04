import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-issue-details',
  templateUrl: './issue-details.component.html',
  styleUrls: ['./issue-details.component.scss'],
})
export class IssueDetailsComponent implements OnInit {
  issueName: string = '';
  issueNameEdited: string = ''

  ngOnInit(): void {
    this.issueName = 'GS-24_Label_FrontEnd';
    this.issueNameEdited = this.issueName
  }

  issueNumber: number = 54;
  author: string = 'Munja200';
  isClosed: boolean = true;
  commentNum: number = 0;

  isEditing = false;

  startEditing() {
    this.isEditing = true;
  }

  cancelEditing() {
    this.isEditing = false;
    this.issueNameEdited = this.issueName;
  }

  confirmEditing() {
    this.isEditing = false;
    this.issueName = this.issueNameEdited;
  }
}
