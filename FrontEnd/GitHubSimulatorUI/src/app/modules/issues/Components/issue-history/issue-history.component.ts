import { Component } from '@angular/core';

@Component({
  selector: 'app-issue-history',
  templateUrl: './issue-history.component.html',
  styleUrls: ['./issue-history.component.scss']
})
export class IssueHistoryComponent {
  eventList: string[] = [
    "Munja added this to the Finalna odbrana milestone 2 weeks ago",
    "Munja200 assigned Munja200 and mihajloveljic and unassigned Munja200 2 weeks ago",
    "mihajloveljic closed this as completed 2 weeks ago"
  ]
}
