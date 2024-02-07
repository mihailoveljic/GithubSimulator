import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { MilestoneService } from 'src/app/services/milestone.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-issue-assign-new-issue',
  templateUrl: './issue-assign-new-issue.component.html',
  styleUrls: ['./issue-assign-new-issue.component.scss'],
})
export class IssueAssignNewIssueComponent implements OnInit {
  constructor(
    private milestoneService: MilestoneService,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    this.userService.getAllUsers().subscribe((res) => {
      this.allUsers = res;
    });

    this.userService.getUser().subscribe((res) => {
      this.loggedInUser = res;
    });

    // TODO promeni ovo
    this.getMilestonesForRepo('0551f295-f939-43bc-a307-98dc1f039f36');
  }

  @Output() childIssueDataEvent = new EventEmitter<any>();

  issueDetails: any = {
    title: '',
    description: '',
    assigne: { email: null },
    // TODO promeni ovo
    repositoryId: '0551f295-f939-43bc-a307-98dc1f039f36',
    milestoneId: null,
  };

  loggedInUser: any = {};
  allUsers: any = [];
  allMilestonesForRepo: any = [];
  milestoneInfo: any = {};

  private getMilestonesForRepo(repoId: string) {
    //if (!this.issueDetails.repositoryId) return;

    this.milestoneService.getMilestonesForRepo(repoId).subscribe(
      (res) => {
        this.allMilestonesForRepo = res;
        console.log('Milestones for repo: ');
        console.log(this.allMilestonesForRepo);
      },
      (err) => {
        if (err.status === 404) {
          console.error(err);
        }
      }
    );
  }

  clearAssignee() {
    this.issueDetails.assigne.email = null;
    this.sendDataToParent();
  }

  assignUser(email: any) {
    this.issueDetails.assigne.email = email;
    this.sendDataToParent();
  }

  clearMilestone() {
    this.issueDetails.milestoneId = null;
    this.milestoneInfo = {};
    this.sendDataToParent();
  }

  assignMilestone(milestoneId: any) {
    this.issueDetails.milestoneId = milestoneId;
    this.milestoneInfo = this.allMilestonesForRepo.find(
      (item: { id: any }) => item.id === milestoneId
    );

    this.sendDataToParent();
  }

  sendDataToParent() {
    this.childIssueDataEvent.emit(this.issueDetails);
  }
}
