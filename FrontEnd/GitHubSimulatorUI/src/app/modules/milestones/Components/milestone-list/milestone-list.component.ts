import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MilestoneService } from 'src/app/services/milestone.service';

@Component({
  selector: 'app-milestone-list',
  templateUrl: './milestone-list.component.html',
  styleUrls: ['./milestone-list.component.scss'],
})
export class MilestoneListComponent implements OnInit {
  constructor(
    private milestoneService: MilestoneService,
    private datePipe: DatePipe
  ) {}

  // TODO Promeni ovo
  ngOnInit(): void {
    this.milestoneService
      .getMilestonesForRepo('fc43c5e2-362d-49bf-80ad-1dfa5c86308e')
      .subscribe((res) => {
        this.allMilestonesForRepo = res;
        this.getOpenAndClosedMilestonesNum();

        // Fetch progress for each milestone
        res.forEach((milestone: any, index: any) => {
          this.milestoneService
            .getMilestoneProgress(milestone.id)
            .subscribe((progress: any) => {
              this.milestoneProgressList[index] = {
                openIssueCounter: progress.openIssueCounter,
                closedIssueCounter: progress.closedIssueCounter,
                progress: Math.round(progress.progress),
              };
            });
        });
      });
  }
  allMilestonesForRepo: any = [];
  milestoneProgressList: any = [];
  openMilestoneNum = 0;
  closedMilestoneNum = 0;

  deleteMilestone(milestoneId: string) {
    if (milestoneId === undefined || milestoneId === null) return;

    this.milestoneService.deleteMilestone(milestoneId).subscribe((res) => {
      let indexToRemove = this.allMilestonesForRepo.findIndex(
        (obj: any) => obj.id === milestoneId
      );

      if (indexToRemove !== -1) {
        this.allMilestonesForRepo.splice(indexToRemove, 1);
      }
    });
  }

  getFormatedDate(milestone: any) {
    const unformatedDate = new Date(milestone.dueDate);
    return this.datePipe.transform(unformatedDate, 'MMMM dd, yyyy');
  }

  reopenOrCloseMilestone(milestoneId: string, state: number) {
    this.milestoneService
      .reopenOrCloseMilestone(milestoneId, state)
      .subscribe((res: any) => {
        let updatedMilestone = this.allMilestonesForRepo.find(
          (ms: any) => ms.id === res.id
        );
        updatedMilestone.state = res.state;

        this.getOpenAndClosedMilestonesNum();
      });
  }

  getOpenAndClosedMilestonesNum() {
    this.openMilestoneNum = this.allMilestonesForRepo.filter((ms: any) => {
      return ms.state === 0;
    }).length;

    this.closedMilestoneNum = this.allMilestonesForRepo.filter((ms: any) => {
      return ms.state === 1;
    }).length;
  }

  // TODO promeni ovo
  getOpenMilestones(state: number) {
    this.milestoneService
      .getOpenOrClosedMilestones('fc43c5e2-362d-49bf-80ad-1dfa5c86308e', state)
      .subscribe((res) => {
        this.allMilestonesForRepo = res;
      });
  }

  getFormattedMilestoneTitle(title: string) {
    return title.replace(/\s/g, '_');
  }
}
