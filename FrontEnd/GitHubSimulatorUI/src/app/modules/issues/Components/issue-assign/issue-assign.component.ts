import { Component } from '@angular/core';

@Component({
  selector: 'app-issue-assign',
  templateUrl: './issue-assign.component.html',
  styleUrls: ['./issue-assign.component.scss'],
})
export class IssueAssignComponent {
  participantNum: number = 2;
  areaProgress: number = 51;
}
