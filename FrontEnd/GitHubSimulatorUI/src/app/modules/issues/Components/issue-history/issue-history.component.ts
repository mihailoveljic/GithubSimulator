import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-issue-history',
  templateUrl: './issue-history.component.html',
  styleUrls: ['./issue-history.component.scss']
})
export class IssueHistoryComponent {
  @Input() description = ''
  @Input() author = ''
  @Input() createdAt: any = ''
  @Input() issueEvents: any = []
}
