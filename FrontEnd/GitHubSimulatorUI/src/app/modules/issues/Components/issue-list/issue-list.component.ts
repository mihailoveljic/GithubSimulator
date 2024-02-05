import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { IssueService } from 'src/app/services/issue_service.service';
import { DatePipe } from '@angular/common';
import { MilestoneService } from 'src/app/services/milestone.service';
import { Observable, of, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

@Component({
  selector: 'app-issue-list',
  templateUrl: './issue-list.component.html',
  styleUrls: ['./issue-list.component.scss'],
})
export class IssueListComponent implements OnInit {
  constructor(
    private issueService: IssueService,
    private milestoneService: MilestoneService,
    private datePipe: DatePipe
  ) {}

  ngOnInit(): void {
    this.issueService.getAllIssues().subscribe((res) => {
      this.allIssues = res;
      console.log(res);

      this.allIssues.forEach((issue) => {
        this.issueMilestones[issue.id] = this.getMilestone(issue);
      });
    });
  }

  displayedColumns: string[] = ['title', 'assignee'];

  allIssues: any[] = [];

  issueMilestones: { [key: string]: Observable<string> } = {};

  authorUsername: string = '';

  isDone: boolean = true;

  getIssueNumber(issue: any) {
    return this.allIssues.indexOf(issue, 0);
  }

  getFormatedDate(issue: any) {
    const unformatedDate = new Date(issue.createdAt);
    return this.datePipe.transform(unformatedDate, 'dd-MM-yyyy HH:mm');
  }

  getAuthor(issue: any) {
    return issue.author.email;
  }

  getMilestone(issue: any): Observable<string> {
    return this.milestoneService.getMilestoneById(issue.milestoneId).pipe(
      map((res) => res.title),
      catchError((err) => {
        if (err.status === 404) {
          return of('Not assigned to any milestone');
        } else {
          console.error(err.message);
          return throwError(err);
        }
      })
    );
  }

  filterUser() {}
  filterMilestone() {}
}
