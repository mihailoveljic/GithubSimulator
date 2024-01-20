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
  styleUrls: ['./labels.component.css']
})
export class LabelsComponent implements OnInit {

  selectedColor: string = '#ffffff'
  name: string = '';
  description: string = '';
  labels: Label[] = [];
  id: string = '';

  constructor(private labelService: LabelService, private toastr: ToastrService) { }

  ngOnInit() {
    this.labelService.getAllLabels().pipe(
      catchError(error => {
        this.toastr.error('Something went wrong while fetching labels');
        return of([]); // Return an empty array or appropriate fallback value
      })
    ).subscribe(labels => {
      this.labels = labels;
    });
  }
  openColorPicker(event: any): void {
    event.preventDefault();
  }

  onUpsertLabelClick(event: any) {
    event.preventDefault();

    if (!this.id) {
      const insertRequest = this.createInsertLabelRequest();
      this.labelService.createLabel(insertRequest).pipe(
        tap(label => {
          if (label) {
            this.labels.push(label);
            this.clearForm();
            this.toastr.success('Label created successfully');
          }
        }),
        catchError(error => {
          this.clearForm();
          this.toastr.error('Something went wrong while creating the label');
          return of(null); // Return a null or appropriate fallback value
        })
      ).subscribe();
    } else {
      const updateRequest = this.createUpdateLabelRequest();
      this.labelService.updateLabel(updateRequest).pipe(
        tap(label => {
          if (label) {
            const index = this.labels.findIndex(l => l.id === this.id);
            if (index !== -1) {
              this.labels[index] = label;
            }
            this.clearForm();
            this.toastr.success('Label updated successfully');
          }
        }),
        catchError(error => {
          this.clearForm();
          this.toastr.error('Something went wrong while updating the label');
          return of(null); // Return a null or appropriate fallback value
        })
      ).subscribe();
    }
  }
  onCancelClick(event: any) {
    event.preventDefault();
    this.clearForm();
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
    return new InsertLabelRequest(this.name, this.description, this.selectedColor);
  }
  private createUpdateLabelRequest(): UpdateLabelRequest {
    return new UpdateLabelRequest(this.id, this.name, this.description, this.selectedColor);
  }
  onEditLabelClicked(label: Label) {
    this.id = label.id;
    this.name = label.name;
    this.description = label.description;
    this.selectedColor = label.color;
  }
  onDeleteLabelClicked(label: Label) {
    this.labelService.deleteLabel(label.id).pipe(
      tap(deleted => {
        if (deleted) {
          this.labels = this.labels.filter(l => l.id !== label.id);
          this.toastr.success('Label deleted successfully');
        }
      }),
      catchError(error => {
        this.toastr.error('Something went wrong while deleting the label');
        return of(false); // Return a fallback value indicating failure
      })
    ).subscribe();
  }
}
