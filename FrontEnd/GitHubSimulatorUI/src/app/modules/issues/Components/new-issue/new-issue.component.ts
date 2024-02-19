import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { IssueService } from 'src/app/services/issue_service.service';
import { UserRepositoryService } from 'src/app/services/user-repository.service';

@Component({
  selector: 'app-new-issue',
  templateUrl: './new-issue.component.html',
  styleUrls: ['./new-issue.component.scss'],
})
export class NewIssueComponent implements OnInit {
  constructor(
    private issueService: IssueService,
    private userRepositoryService: UserRepositoryService,
    private router: Router,
    private route: ActivatedRoute,
    private toastr: ToastrService
  ) {}

  repoOwnerName: string = '';
  repoName: string = '';
  repoUserRole: number = -1;

  ngOnInit(): void {
    this.route.params.subscribe((params: any) => {
      this.repoOwnerName = params['userName'];
      this.repoName = params['repositoryName'];

      this.userRepositoryService
        .getAuthenticatedUserRepositoryRole(this.repoName)
        .subscribe((resR: any) => {
          this.repoUserRole = resR;

          if (this.repoUserRole < 1 || this.repoUserRole > 4) {
            this.router.navigate(['/home-page']);
          }
        });
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
    this.issueDetails.repositoryName = this.repoName;

    this.issueService.createIssue(this.issueDetails).subscribe((res) => {
      this.toastr.success('Issue Created Successfully!');
      this.router.navigate([
        this.repoOwnerName + '/' + this.repoName + '/issues',
      ]);
    });
  }
}
