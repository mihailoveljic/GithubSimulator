import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-issue-list',
  templateUrl: './issue-list.component.html',
  styleUrls: ['./issue-list.component.scss'],
})
export class IssueListComponent {
  constructor(private router: Router) {}

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

  isDone: boolean = true;

  getRecord(row: any) {
    console.log(row)
    this.router.navigate(['/']);
  }
}
