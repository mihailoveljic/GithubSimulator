import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
  SimpleChanges,
} from '@angular/core';
import { IssueService } from 'src/app/services/issue_service.service';
import { MilestoneService } from 'src/app/services/milestone.service';

@Component({
  selector: 'app-issue-assign',
  templateUrl: './issue-assign.component.html',
  styleUrls: ['./issue-assign.component.scss'],
})
export class IssueAssignComponent implements OnChanges {
  constructor(private milestoneService: MilestoneService, private issueService: IssueService) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['issueDetails'] && changes['issueDetails'].currentValue) {
      this.getMilestone();
      this.getMilestonesForRepo(this.issueDetails.repositoryId);
    }
  }

  participantNum: number = 2;

  @Input() issueDetails: any;
  @Output() issueDetailsChange = new EventEmitter<any>();

  updatedIssue: any = {};
  milestoneInfo: any = {};

  allMilestonesForRepo: any = [];

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
        console.log('All mile for repo:');
        console.log(this.allMilestonesForRepo);
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
    this.issueService.updateIssueMilestone(this.issueDetails.id, milestoneId).subscribe((res) => {
      this.issueDetails.milestoneId = res;
      this.getMilestone()
      console.log('Updated milestone id:')
      console.log(res)
    });
  }
}
