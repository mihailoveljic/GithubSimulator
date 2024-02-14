import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { LabelService } from 'src/app/services/label.service';
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
    private userService: UserService,
    private labelService: LabelService,
    private route: ActivatedRoute
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
    this.milestoneService
      .getMilestonesForRepo('1490c28e-ebf5-4ad4-810b-8a6540566ef2')
      .subscribe(
        (res) => {
          this.allMilestonesForRepo = res;
          this.filteredMilestones = this.allMilestonesForRepo;

          const queryParams = this.route.snapshot.queryParams;
          let milestoneId = queryParams['milestoneId'];

          if (milestoneId) {
            this.assignMilestone(milestoneId);
          }
        },
        (err) => {
          if (err.status === 404) {
            console.error(err);
          }
        }
      );

    this.labelService.getAllLabels().subscribe((res) => {
      this.allLabels = res;
      this.filteredLabels = this.allLabels;
    });
  }

  @Output() childIssueDataEvent = new EventEmitter<any>();

  issueDetails: any = {
    title: '',
    description: '',
    assigne: { email: null },
    // TODO promeni ovo
    repositoryId: '1490c28e-ebf5-4ad4-810b-8a6540566ef2',
    milestoneId: null,
    labelIds: null,
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

    console.log(this.allMilestonesForRepo);
    this.sendDataToParent();
  }

  ////////////LABELS
  allLabels: any[] = [];
  labelFilter: string = '';
  filteredLabels: any = [];
  assignedLabels: any = [];

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

  addLabels(event: any, label: any) {
    event.stopPropagation();

    const foundLabel = this.assignedLabels.find(
      (item: any) => item.id === label.id
    );

    if (foundLabel) {
      // If the object exists, toggle its isClicked property
      foundLabel.isClicked = !foundLabel.isClicked;
    } else {
      // If the object does not exist, add a new object with the provided ID and isClicked set to true
      this.assignedLabels.push({
        id: label.id,
        name: label.name,
        color: label.color,
        isClicked: true,
      });
    }
  }

  isLabelClicked(label: any) {
    if (label === null || label === undefined) return false;

    const foundLabel = this.assignedLabels.find(
      (item: any) => item.id === label.id
    );

    if (foundLabel) {
      return foundLabel.isClicked;
    }

    return false;
  }

  assignLabels() {
    let assignedLabels = this.assignedLabels
      .filter((label: any) => {
        return label.isClicked;
      })
      .map((label: any) => label.id);

    this.issueDetails.labelIds = assignedLabels;
    this.sendDataToParent();
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
  //////////////////

  sendDataToParent() {
    this.childIssueDataEvent.emit(this.issueDetails);
  }
}
