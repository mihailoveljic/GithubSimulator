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
      this.filteredUsers = this.allUsers;
    });

    this.userService.getUser().subscribe((res) => {
      this.loggedInUser = res;
    });

    // TODO promeni ovo
    this.getMilestonesForRepo('9bfc3a6f-870b-4050-afad-7361569fbe99');
  }

  @Output() childIssueDataEvent = new EventEmitter<any>();

  issueDetails: any = {
    title: '',
    description: '',
    assigne: { email: null },
    // TODO promeni ovo
    repositoryId: '9bfc3a6f-870b-4050-afad-7361569fbe99',
    milestoneId: null,
  };

  loggedInUser: any = {};
  allUsers: any = [];
  filteredUsers: any = [];
  userFilter: string = '';

  allMilestonesForRepo: any = [];
  milestoneInfo: any = {};
  filteredMilestones: any = [];
  milestoneFilter: string = '';

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

  private getMilestonesForRepo(repoId: string) {
    //if (!this.issueDetails.repositoryId) return;

    this.milestoneService.getMilestonesForRepo(repoId).subscribe(
      (res) => {
        this.allMilestonesForRepo = res;
        this.filteredMilestones = this.allMilestonesForRepo;
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
