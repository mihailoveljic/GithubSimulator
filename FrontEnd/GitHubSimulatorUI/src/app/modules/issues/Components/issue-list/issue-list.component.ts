import { Component } from '@angular/core';

@Component({
  selector: 'app-issue-list',
  templateUrl: './issue-list.component.html',
  styleUrls: ['./issue-list.component.scss'],
})
export class IssueListComponent {
  displayedColumns: string[] = ['name', 'icon'];

  dataSource = [
    {
      id: '1',
      name: 'GS-26_Milestone_Frontend',
    },
    {
      id: '2',
      name: 'GS-27_Branch_Frontend',
    },
    {
      id: '3',
      name: 'GS-28_Global_Search',
    },
  ];
}
