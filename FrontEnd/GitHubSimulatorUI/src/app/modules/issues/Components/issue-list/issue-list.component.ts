import { Component, OnInit } from '@angular/core';
import { IssueService } from 'src/app/services/issue_service.service';
import { DatePipe } from '@angular/common';
import { MilestoneService } from 'src/app/services/milestone.service';
import { Observable, of, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-issue-list',
  templateUrl: './issue-list.component.html',
  styleUrls: ['./issue-list.component.scss'],
})
export class IssueListComponent implements OnInit {
  constructor(
    private issueService: IssueService,
    private milestoneService: MilestoneService,
    private datePipe: DatePipe,
    private userService: UserService
  ) {}

  // TODO promeni da se vracaju samo issue-i od repozitorijuma
  ngOnInit(): void {
    this.issueService.getAllIssues().subscribe((res) => {
      this.allIssues = res;
      console.log(res);

      this.allIssues.forEach((issue) => {
        this.issueMilestones[issue.id] = this.getMilestone(issue);
      });
    });

    this.userService.getAllUsers().subscribe((res) => {
      this.allUsers = res;
      this.filteredUsers = this.allUsers;
    });

    // TODO promeni ovo
    this.getMilestonesForRepo('9bfc3a6f-870b-4050-afad-7361569fbe99');
  }

  displayedColumns: string[] = ['title', 'assignee'];

  allIssues: any[] = [];

  issueMilestones: { [key: string]: Observable<string> } = {};

  authorUsername: string = '';

  /////////////////USERS
  allUsers: any = [];
  filteredUsers: any = [];
  userFilter: string = '';

  //user.email.email
  filterUsers(): void {
    if (!this.userFilter.trim() || this.userFilter === '') {
      this.filteredUsers = this.allUsers;
      return;
    }
    const userFilterLower = this.userFilter.toLowerCase();

    this.filteredUsers = this.allUsers.filter((user: any) => {
      return user.email.email.toLowerCase().includes(userFilterLower);
    });
  }

  resetUserFilter() {
    this.userFilter = '';
    this.filterUsers();
  }
  //////////////////////

  /////////////////MILESTONES

  allMilestonesForRepo: any = [];
  filteredMilestones: any = [];
  milestoneFilter: string = '';

  private getMilestonesForRepo(repoId: string) {
    this.milestoneService.getMilestonesForRepo(repoId).subscribe(
      (res) => {
        this.allMilestonesForRepo = res;
        this.filteredMilestones = this.allMilestonesForRepo;
      },
      (err) => {
        if (err.status === 404) {
          console.log(err);
        }
      }
    );
  }

  filterMilestones(): void {
    if (!this.milestoneFilter.trim() || this.milestoneFilter === '') {
      this.filteredMilestones = this.allMilestonesForRepo;
      return;
    }

    const milestoneFilterLower = this.milestoneFilter.toLowerCase();

    this.filteredMilestones = this.allMilestonesForRepo.filter(
      (milestone: any) => {
        return milestone.title.toLowerCase().includes(milestoneFilterLower);
      }
    );
  }

  searchMilestone(milestoneTitle: string) {}
  ///////////////////////////

  /////////////////SEARCH
  searchString: string = '';

  searchIssues(searchString: string) {
    this.searchString = searchString;
    console.log(this.searchString);
  }
  ///////////////////////

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
    if (issue.milestoneId === null) return of('Not assigned to any milestone');

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

  getOpenIssuesCount() {
    let counter = 0;
    for (const issue of this.allIssues) {
      if (issue.isOpen) counter++;
    }

    return counter;
  }

  getClosedIssuesCount() {
    let counter = 0;
    for (const issue of this.allIssues) {
      if (!issue.isOpen) counter++;
    }

    return counter;
  }
}
