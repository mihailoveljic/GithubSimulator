import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { IssueService } from 'src/app/services/issue_service.service';
import { MilestoneService } from 'src/app/services/milestone.service';

@Component({
  selector: 'app-milestone-details',
  templateUrl: './milestone-details.component.html',
  styleUrls: ['./milestone-details.component.scss'],
})
export class MilestoneDetailsComponent implements OnInit {
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private milestoneService: MilestoneService,
    private issueService: IssueService,
    private datePipe: DatePipe
  ) {}

  repoOwnerName: string = '';
  repoName: string = '';

  ngOnInit(): void {
    const queryParams = this.route.snapshot.queryParams;
    this.milestoneId = queryParams['id'];
    
    this.route.params.subscribe((params: any) => {
      this.repoOwnerName = params['userName'];
      this.repoName = params['repositoryName'];
    });

    if (this.milestoneId) {
      this.milestoneService
        .getMilestoneById(this.milestoneId)
        .subscribe((res) => {
          this.milestoneInfo = res;

          this.milestoneService
            .getMilestoneProgress(this.milestoneId)
            .subscribe((res1: any) => {
              this.milestoneProgress = res1;
              this.milestoneProgress.progress = Math.round(
                this.milestoneProgress.progress
              );
            });

          this.issueService
            .searchIssues(this.repoName, 'milestone:' + this.milestoneInfo.title)
            .subscribe((res3) => {
              console.log(res3)
              this.milestoneIssues = res3
            });
        });
    }
  }

  milestoneId: any = '';
  milestoneInfo: any = {};
  milestoneProgress: any;
  milestoneIssues: any = [];

  displayedColumns: string[] = ['title', 'assignee'];

  getFormatedDate(milestone: any) {
    if (!milestone.dueDate) return;

    const unformatedDate = new Date(milestone.dueDate);
    return this.datePipe.transform(unformatedDate, 'MMMM dd, yyyy');
  }

  searchIssues(queryString: string) {
    this.issueService
      .searchIssues(this.repoName, queryString)
      .subscribe((res) => {
        this.milestoneIssues = res;
      });
  }

  convertToRGBA(hexColor: string, opacity: number): string {
    let hex = hexColor.replace('#', '');
    if (hex.length === 3) {
      hex = hex[0] + hex[0] + hex[1] + hex[1] + hex[2] + hex[2];
    }
    const r = parseInt(hex.substring(0, 2), 16);
    const g = parseInt(hex.substring(2, 4), 16);
    const b = parseInt(hex.substring(4, 6), 16);

    return `rgba(${r}, ${g}, ${b}, ${opacity})`;
  }

  getIssueNumber(issue: any) {
    return this.milestoneIssues.indexOf(issue, 0);
  }

  getAuthor(issue: any) {
    return issue.author.email;
  }

  filterByAuthor(issue: any) {
    this.router.navigate(['issues', this.repoOwnerName, this.repoName], {
      queryParams: { author: issue.author.email },
    });
  }

  goToMilestonesPage() {
    this.router.navigate(['milestones', this.repoOwnerName, this.repoName])
  }
}
