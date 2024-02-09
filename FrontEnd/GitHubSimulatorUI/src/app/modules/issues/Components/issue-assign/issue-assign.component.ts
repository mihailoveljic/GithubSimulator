import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output,
  SimpleChanges,
} from '@angular/core';
import { IssueService } from 'src/app/services/issue_service.service';
import { MilestoneService } from 'src/app/services/milestone.service';
import { UserService } from 'src/app/services/user.service';
@Component({
  selector: 'app-issue-assign',
  templateUrl: './issue-assign.component.html',
  styleUrls: ['./issue-assign.component.scss'],
})
export class IssueAssignComponent implements OnChanges, OnInit {
  constructor(
    private milestoneService: MilestoneService,
    private issueService: IssueService,
    private userService: UserService
  ) {}

  milestoneProgress: any = 0

  ngOnInit(): void {
    this.userService.getAllUsers().subscribe((res) => {
      this.allUsers = res;
      this.filteredUsers = this.allUsers;
    });

    this.userService.getUser().subscribe((res) => {
      this.loggedInUser = res;
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['issueDetails'] && changes['issueDetails'].currentValue) {
      this.getMilestone();
      this.getMilestonesForRepo(this.issueDetails.repositoryId);
      this.getMilestoneProgress(this.issueDetails.milestoneId);
    }
  }

  allUsers: any = [];
  filteredUsers: any = [];
  userFilter: string = '';

  loggedInUser: any = {};

  @Input() issueDetails: any;
  @Output() issueDetailsChange = new EventEmitter<any>();

  updatedIssue: any = {};
  milestoneInfo: any = {};

  allMilestonesForRepo: any = [];
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

  private getMilestone() {
    if (!this.issueDetails.milestoneId) return;

    this.milestoneService
      .getMilestoneById(this.issueDetails.milestoneId)
      .subscribe(
        (res) => {
          this.milestoneInfo = res;
        },
        (err) => {
          if (err.status === 404) {
            this.milestoneInfo = {};
          } else console.error(err);
        }
      );
  }

  private getMilestonesForRepo(repoId: string) {
    if (!this.issueDetails.repositoryId) return;

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

  updateIssue() {
    this.issueDetailsChange.emit(this.updatedIssue);
  }

  assignMilestone(milestoneId: string) {
    if (milestoneId === null || milestoneId === undefined) return

    this.issueService
      .updateIssueMilestone(this.issueDetails.id, milestoneId)
      .subscribe((res) => {
        this.issueDetails.milestoneId = res;
        this.getMilestone();
      });
  }

  assignUser(assignee: string) {
    if (assignee === null || assignee === undefined) return;

    this.issueService
      .updateIssueAssignee(this.issueDetails.id, { email: assignee })
      .subscribe((res) => {
        this.issueDetails.assigne.email = assignee;
      });
  }

  clearAssignee() {
    this.issueService
      .updateIssueAssignee(this.issueDetails.id, null)
      .subscribe((res) => {
        this.issueDetails.assigne.email = null;
      });
  }

  clearMilestone() {
    this.issueService
      .updateIssueMilestone(this.issueDetails.id, null)
      .subscribe((res) => {
        this.issueDetails.milestoneId = null;
        this.milestoneInfo = {};
      });
  }

  getParticipantNum() {
    return this.issueDetails.assigne === undefined ||
      this.issueDetails.assigne?.email === null
      ? 1
      : this.issueDetails.assigne.email === this.issueDetails.author.email
      ? 1
      : 2;
  }

  getParticipants() {
    if (this.issueDetails.author === undefined) return;

    return this.issueDetails.assigne === undefined ||
      this.issueDetails.assigne?.email === null
      ? [{ email: this.issueDetails.author.email }]
      : this.issueDetails.assigne.email === this.issueDetails.author.email
      ? [{ email: this.issueDetails.author.email }]
      : [
          { email: this.issueDetails.author.email },
          { email: this.issueDetails.assigne.email },
        ];
  }

  getMilestoneProgress(milestoneId: string) {
    if (milestoneId === undefined || milestoneId === null) return;

    this.milestoneService.getMilestoneProgress(milestoneId).subscribe((res) => {
      this.milestoneProgress = res;
    });
  }
}
