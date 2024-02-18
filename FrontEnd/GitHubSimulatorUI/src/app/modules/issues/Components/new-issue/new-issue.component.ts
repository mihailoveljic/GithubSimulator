import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { IssueService } from 'src/app/services/issue_service.service';

@Component({
  selector: 'app-new-issue',
  templateUrl: './new-issue.component.html',
  styleUrls: ['./new-issue.component.scss'],
})
export class NewIssueComponent implements OnInit {
  constructor(
    private issueService: IssueService,
    private router: Router,
    private route: ActivatedRoute,
    private toastr: ToastrService
  ) {}

  repoOwnerName: string = '';
  repoName: string = '';

  ngOnInit(): void {
    this.route.params.subscribe((params: any) => {
      this.repoOwnerName = params['userName'];
      this.repoName = params['repositoryName'];
    });
  }

  title: string = '';
  description: string = '';

  issueDetails: any = {
    title: '',
    description: '',
    assignee: { email: null },
    repositoryName: '',
    milestoneId: null,
    labelIds: null,
  };

  updateDataFromChild(data: any) {
    this.issueDetails = data;
  }

  submitNewIssue() {
    this.issueDetails.title = this.title;
    this.issueDetails.description = this.description;
    this.issueDetails.repositoryName = this.repoName

    this.issueService.createIssue(this.issueDetails).subscribe((res) => {
      this.toastr.success('Issue Created Successfully!');
      this.router.navigate([this.repoOwnerName + '/' + this.repoName + '/issues']);
    });
  }
}
