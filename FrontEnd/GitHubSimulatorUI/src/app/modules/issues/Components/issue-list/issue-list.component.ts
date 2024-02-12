import { Component, OnInit } from '@angular/core';
import { IssueService } from 'src/app/services/issue_service.service';
import { DatePipe } from '@angular/common';
import { MilestoneService } from 'src/app/services/milestone.service';
import { Observable, of, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { UserService } from 'src/app/services/user.service';
import { ActivatedRoute, Router } from '@angular/router';
import { LabelService } from 'src/app/services/label.service';

@Component({
  selector: 'app-issue-list',
  templateUrl: './issue-list.component.html',
  styleUrls: ['./issue-list.component.scss'],
})
export class IssueListComponent implements OnInit {
  constructor(
    private issueService: IssueService,
    private milestoneService: MilestoneService,
    private userService: UserService,
    private labelService: LabelService,
    private datePipe: DatePipe,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  // TODO promeni da se vracaju samo issue-i od repozitorijuma
  ngOnInit(): void {
    this.route.params.subscribe((params: any) => {
      this.getAllIssues();
    });

    this.userService.getAllUsers().subscribe((res) => {
      this.allUsers = res;
      this.filteredUsers = this.allUsers;
    });

    this.labelService.getAllLabels().subscribe((res) => {
      this.allLabels = res;
      this.filteredLabels = this.allLabels
    });
    // TODO promeni ovo
    this.getMilestonesForRepo('c74dffe8-e0fb-459d-a48a-f719a709f365');
  }

  displayedColumns: string[] = ['title', 'assignee'];

  allIssues: any[] = [];

  ////////////////LABELS
  allLabels: any[] = [];
  labelFilter: string = '';
  filteredLabels: any = [];

  filterLabels(): void {
    if (!this.labelFilter.trim() || this.labelFilter === '') {
      this.filteredLabels = this.allLabels;
      return;
    }
    const labelFilterLower = this.labelFilter.toLowerCase();

    this.filteredLabels = this.allLabels.filter((label: any) => {
      return (
        label.name.toLowerCase().includes(labelFilterLower) ||
        label.description.toLowerCase().includes(labelFilterLower)
      );
    });
  }

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

  /////////////////MILESTONES
  issueMilestones: { [key: string]: Observable<string> } = {};

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

  /////////////////SEARCH
  searchString: string = '';

  searchIssues(searchString: string) {
    if (searchString.includes('undefined')) return;

    this.searchString = searchString;

    // Read existing query parameters
    const queryParams = { ...this.route.snapshot.queryParams };

    const newParams = this.searchString.split('+').map((param) => {
      const [key, value] = param.split(':');
      return { key, value };
    });

    newParams.forEach((param) => {
      if (param.key && param.value !== undefined) {
        queryParams[param.key] = param.value;
      }
    });

    this.router
      .navigate([], {
        relativeTo: this.route,
        queryParams: queryParams,
        queryParamsHandling: 'merge',
      })
      .then(() => {
        this.getAllIssues();
      });
  }

  // TODO onemoguci da se pravi milestone sa imenom koji sadrzi '_'
  getFormattedMilestoneTitle(title: string) {
    return title.replace(/\s/g, '_');
  }

  async handleMilestoneClick(id: string) {
    const milestoneTitle = await this.getFormattedMilestoneTitleObservable(id);
    if (milestoneTitle) {
      const searchQuery = `milestone:${milestoneTitle}`;
      console.log(searchQuery);
      this.searchIssues(searchQuery);
    }
  }

  getFormattedMilestoneTitleObservable(id: string): Promise<string> {
    return new Promise<string>((resolve) => {
      this.issueMilestones[id].subscribe((res) => {
        const milestoneTitle = this.getFormattedMilestoneTitle(res);
        console.log('MILESTONE:', milestoneTitle);
        resolve(milestoneTitle);
      });
    });
  }

  hasQueryParams(): boolean {
    return Object.keys(this.route.snapshot.queryParams).length > 0;
  }

  clearSearchParams(): void {
    this.router.navigate(['/issues-page']).then(() => {
      this.getAllIssues();
    });
  }

  getAllIssues() {
    console.log('Getting all issues...');
    const queryParams = this.route.snapshot.queryParams;
    const srchStr = Object.keys(queryParams)
      .map((key) => `${key}:${queryParams[key]}`)
      .join(' ');

    console.log('PARAMS:');
    console.log(srchStr);

    ///////////////////

    // this.issueService.getAllIssues().subscribe((res) => {
    //   this.allIssues = res;

    //   this.allIssues.forEach((issue) => {
    //     this.issueMilestones[issue.id] = this.getMilestone(issue);
    //   });
    // });

    this.issueService.searchIssues(srchStr).subscribe((res) => {
      this.allIssues = res;

      this.allIssues.forEach((issue) => {
        this.issueMilestones[issue.id] = this.getMilestone(issue);
      });
    });
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
