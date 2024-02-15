import { Component, OnInit } from '@angular/core';
import { LabelService } from 'src/app/services/label.service';
import { Label } from '../model/Label';
import { InsertLabelRequest } from '../model/dtos/InsertLabelRequest';
import { UpdateLabelRequest } from '../model/dtos/UpdateLabelRequest';
import { ToastrService } from 'ngx-toastr';
import { catchError, of, tap } from 'rxjs';

@Component({
  selector: 'app-labels',
  templateUrl: './labels.component.html',
  styleUrls: ['./labels.component.css'],
})
export class LabelsComponent implements OnInit {
  selectedColor: string = '#ffffff';
  name: string = '';
  description: string = '';
  labels: Label[] = [];
  id: string = '';

  searchInput: string = '';

  isCreatingLabel: boolean = false;
  isEditingLabelList: any = [];
  isEditingAllowed: boolean = true;

  isLabelNameFormatCorrect: boolean = true;

  labelCopy: any = { name: '', description: '', color: '' };

  constructor(
    private labelService: LabelService,
    private toastr: ToastrService
  ) {}

  ngOnInit() {
    this.labelService
      .getAllLabels()
      .pipe(
        catchError((error) => {
          this.toastr.error('Something went wrong while fetching labels');
          return of([]); // Return an empty array or appropriate fallback value
        })
      )
      .subscribe((labels) => {
        this.labels = labels;
        for (const label of this.labels) {
          this.isEditingLabelList.push({ id: label.id, isEditing: false });
        }

        this.randomizeLabelColor();
      });
  }
  openColorPicker(event: any): void {
    event.preventDefault();
    this.randomizeLabelColor();
  }

  getRandomHexColor(): string {
    // Generate random values for red, green, and blue components
    const red = Math.floor(Math.random() * 256); // Random integer between 0 and 255
    const green = Math.floor(Math.random() * 256);
    const blue = Math.floor(Math.random() * 256);

    // Convert each component to hexadecimal and concatenate them
    const hexRed = red.toString(16).padStart(2, '0'); // Convert to hexadecimal and ensure 2 digits
    const hexGreen = green.toString(16).padStart(2, '0');
    const hexBlue = blue.toString(16).padStart(2, '0');

    // Concatenate the components into a single hexadecimal string
    const hexColor = `#${hexRed}${hexGreen}${hexBlue}`;

    return hexColor;
  }

  randomizeLabelColor() {
    this.selectedColor = this.getRandomHexColor();
  }

  setLabelColor(color: string) {
    this.selectedColor = color;
  }

  uptadeLabelColor(label: any, color: string) {
    label.color = color;
  }

  updateLabelColorRandomize(event: any, label: any) {
    event.preventDefault();

    label.color = this.getRandomHexColor();
  }

  updateLabel(event: any, updatedLabel: any) {
    event.preventDefault();

    this.isLabelNameFormatCorrect = true

    if (updatedLabel.name.includes('_') || updatedLabel.name === '') {
      this.isLabelNameFormatCorrect = false;
      return
    }

    let updateRequest = new UpdateLabelRequest(
      updatedLabel.id,
      updatedLabel.name,
      updatedLabel.description,
      updatedLabel.color
    );

    this.labelService.updateLabel(updateRequest).subscribe(() => {
      this.isEditingLabelList[updatedLabel.id] = false;
      this.isEditingAllowed = true;
    });
  }

  onUpsertLabelClick(event: any) {
    event.preventDefault();

    this.isLabelNameFormatCorrect = true;

    if (this.name.includes('_') || this.name === '') {
      this.isLabelNameFormatCorrect = false;
      return;
    }

    if (!this.id) {
      const insertRequest = this.createInsertLabelRequest();
      this.labelService
        .createLabel(insertRequest)
        .pipe(
          tap((label) => {
            if (label) {
              this.labels.push(label);
              this.clearForm();
              this.toastr.success('Label created successfully');
            }
          }),
          catchError((error) => {
            this.clearForm();
            this.toastr.error('Something went wrong while creating the label');
            return of(null); // Return a null or appropriate fallback value
          })
        )
        .subscribe(() => (this.isCreatingLabel = false));
    } else {
      const updateRequest = this.createUpdateLabelRequest();
      this.labelService
        .updateLabel(updateRequest)
        .pipe(
          tap((label) => {
            if (label) {
              const index = this.labels.findIndex((l) => l.id === this.id);
              if (index !== -1) {
                this.labels[index] = label;
              }
              this.clearForm();
              this.toastr.success('Label updated successfully');
            }
          }),
          catchError((error) => {
            this.clearForm();
            this.toastr.error('Something went wrong while updating the label');
            return of(null); // Return a null or appropriate fallback value
          })
        )
        .subscribe(() => (this.isCreatingLabel = false));
    }
  }
  onCancelClick(event: any) {
    event.preventDefault();
    this.clearForm();
    this.isCreatingLabel = false;
  }
  private clearForm() {
    this.id = '';
    this.name = '';
    this.description = '';
    this.selectedColor = '#ffffff';
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

  private createInsertLabelRequest(): InsertLabelRequest {
    return new InsertLabelRequest(
      this.name,
      this.description,
      this.selectedColor
    );
  }
  private createUpdateLabelRequest(): UpdateLabelRequest {
    return new UpdateLabelRequest(
      this.id,
      this.name,
      this.description,
      this.selectedColor
    );
  }
  onEditLabelClicked(label: Label) {
    this.id = label.id;
    this.name = label.name;
    this.description = label.description;
    this.selectedColor = label.color;
  }

  editLabel(label: any) {
    this.labelCopy.name = label.name;
    this.labelCopy.description = label.description;
    this.labelCopy.color = label.color;

    this.isEditingLabelList[label.id] = true;
    this.isEditingAllowed = false;
  }

  cancelEditLabel(event: any, label: any) {
    event.preventDefault();

    label.name = this.labelCopy.name;
    label.description = this.labelCopy.description;
    label.color = this.labelCopy.color;

    this.isEditingLabelList[label.id] = false;
    this.isEditingAllowed = true;
  }

  onDeleteLabelClicked(label: Label) {
    this.labelService
      .deleteLabel(label.id)
      .pipe(
        tap((deleted) => {
          if (deleted) {
            this.labels = this.labels.filter((l) => l.id !== label.id);
            this.toastr.success('Label deleted successfully');
          }
        }),
        catchError((error) => {
          this.toastr.error('Something went wrong while deleting the label');
          return of(false); // Return a fallback value indicating failure
        })
      )
      .subscribe();
  }

  searchIssues() {
    this.labelService.searchlabels(this.searchInput).subscribe((res) => {
      this.labels = res;
    });
  }

  startCreatingLabel() {
    this.isCreatingLabel = !this.isCreatingLabel;
  }
}
