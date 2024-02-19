import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { IssueService } from 'src/app/services/issue_service.service';

@Component({
  selector: 'app-new-issue',
  templateUrl: './new-pr.component.html',
  styleUrls: ['./new-pr.component.scss'],
})
export class NewPRComponent {
  constructor(private issueService: IssueService, private router: Router, private toastr: ToastrService) {}

  title: string = '';
  description: string = '';

  issueDetails: any = {
    title: '',
    description: '',
    assignee: { email: null },
    repositoryId: '',
    milestoneId: null,
    labelIds: null
  };

  updateDataFromChild(data: any) {
    this.issueDetails = data
  }

  submitNewIssue() {
    this.issueDetails.title = this.title
    this.issueDetails.description = this.description

    this.issueService.createIssue(this.issueDetails).subscribe((res) => {
      this.toastr.success(
        'Issue Created Successfully!'
      );
      this.router.navigate(['issues-page']);
    })
  }
}
