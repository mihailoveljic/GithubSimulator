import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-pr-history',
  templateUrl: './pr-history.component.html',
  styleUrls: ['./pr-history.component.scss']
})
export class PRHistoryComponent {
  @Input() description = ''
  @Input() author = ''
  @Input() createdAt: any = ''
  @Input() issueEvents: any = []
}
