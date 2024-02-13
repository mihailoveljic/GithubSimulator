import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MatDatepickerInputEvent } from '@angular/material/datepicker';
import { ActivatedRoute, Router } from '@angular/router';
import { MilestoneService } from 'src/app/services/milestone.service';

@Component({
  selector: 'app-new-milestone',
  templateUrl: './new-milestone.component.html',
  styleUrls: ['./new-milestone.component.scss'],
})
export class NewMilestoneComponent implements OnInit {
  constructor(
    private milestoneService: MilestoneService,
    private router: Router,
    private datePipe: DatePipe,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    const queryParams = this.route.snapshot.queryParams;
    this.isEditMode = queryParams['edit'] === 'true';

    if (this.isEditMode) {
      this.editedMilestoneId = queryParams['id'];
      this.milestoneService
        .getMilestoneById(this.editedMilestoneId)
        .subscribe((res) => {
          this.newMilestoneTitle = res.title
          this.newMilestoneDueDate = res.dueDate
          this.newMilestoneDescription = res.description
          this.newMilestoneState = res.state
          this.newMilestoneRepositoryId = res.repositoryId
        });
    }
  }

  focusAndSetCursor(event: MouseEvent) {
    const textarea = event.target as HTMLTextAreaElement;
    textarea.value = textarea.value.trim();
  }

  isEditMode: boolean = false;
  editedMilestoneId = '';

  newMilestoneTitle: string = '';
  newMilestoneDueDate: any = '';
  newMilestoneDescription: string = '';
  newMilestoneState = 0;
  // TODO promeni ovo
  newMilestoneRepositoryId = 'fc43c5e2-362d-49bf-80ad-1dfa5c86308e';

  isTitleFormatCorrect: boolean = true;
  isDateFormatCorrect: boolean = true;

  createOrEditMilestone() {
    if (!this.validateInput()) return;

    if (this.isEditMode) {
      const updateMilestoneDto = {
        id: this.editedMilestoneId,
        title: this.newMilestoneTitle,
        description: this.newMilestoneDescription,
        dueDate: this.newMilestoneDueDate,
        state: this.newMilestoneState,
        repositoryId: this.newMilestoneRepositoryId,
      };

      this.milestoneService
        .updateMilestone(updateMilestoneDto)
        .subscribe(() => {
          this.router.navigate(['/milestones-page']);
        });
    }
    else {
      const newMilestoneDto = {
        title: this.newMilestoneTitle,
        description: this.newMilestoneDescription,
        dueDate: this.newMilestoneDueDate,
        state: this.newMilestoneState,
        repositoryId: this.newMilestoneRepositoryId,
      };
  
      this.milestoneService.createMilestone(newMilestoneDto).subscribe(() => {
        this.router.navigate(['/milestones-page']);
      });
    }

  }

  validateInput(): boolean {
    this.isDateFormatCorrect = true;
    this.isTitleFormatCorrect = true;

    let result = true;

    if (this.newMilestoneTitle.includes('_') || this.newMilestoneTitle === '') {
      this.isTitleFormatCorrect = false;
      result = false;
    }

    if (this.newMilestoneDueDate === '' || this.newMilestoneDueDate === null) {
      this.newMilestoneDueDate = null;
    } else {
      try {
        const parsedDate = this.datePipe.transform(
          this.newMilestoneDueDate,
          'mm/dd/yyyy'
        );
        if (parsedDate === null) {
          this.isDateFormatCorrect = false;
          result = false; // Parsing failed
        }
      } catch (error) {
        this.isDateFormatCorrect = false;
        result = false; // Error occurred during parsing
      }
    }
    return result;
  }

  closeOrReopenMilestone(state: any) {
    this.newMilestoneState = state
  }
}
