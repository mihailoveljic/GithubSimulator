import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MilestoneService } from 'src/app/services/milestone.service';

@Component({
  selector: 'app-milestone-list',
  templateUrl: './milestone-list.component.html',
  styleUrls: ['./milestone-list.component.scss'],
})
export class MilestoneListComponent implements OnInit {
  constructor(
    private milestoneService: MilestoneService,
    private datePipe: DatePipe,
    private route: ActivatedRoute
  ) {}

  repoOwnerName: string = '';
  repoName: string = '';

  ngOnInit(): void {
    this.route.params.subscribe((params: any) => {
      this.repoOwnerName = params['userName'];
      this.repoName = params['repositoryName'];
    });

    this.milestoneService
      .getMilestonesForRepo(this.repoName)
      .subscribe((res) => {
        this.allMilestonesForRepo = res;
        this.allMilestonesForRepoCopy = res;
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
  allMilestonesForRepoCopy: any = [];
  milestoneProgressList: any = [];
  openMilestoneNum = 0;
  closedMilestoneNum = 0;

  openOrClosedSearch: number = -1;

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

        let updatedMilestoneCopy = this.allMilestonesForRepoCopy.find(
          (ms: any) => ms.id === res.id
        );
        updatedMilestoneCopy.state = res.state;

        this.getOpenAndClosedMilestonesNum();

        if (this.openOrClosedSearch !== -1) {
          this.getOpenOrClosedMilestones(this.openOrClosedSearch);
        }
      });
  }

  getOpenAndClosedMilestonesNum() {
    this.openMilestoneNum = this.allMilestonesForRepoCopy.filter((ms: any) => {
      return ms.state === 0;
    }).length;

    this.closedMilestoneNum = this.allMilestonesForRepoCopy.filter(
      (ms: any) => {
        return ms.state === 1;
      }
    ).length;
  }

  getOpenOrClosedMilestones(state: number) {
    this.openOrClosedSearch = state;

    this.milestoneService
      .getOpenOrClosedMilestones(this.repoName, state)
      .subscribe((res) => {
        this.allMilestonesForRepo = res;
      });
  }

  getFormattedMilestoneTitle(title: string) {
    return title.replace(/\s/g, '_');
  }
}
