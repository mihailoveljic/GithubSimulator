import { Component, OnInit } from '@angular/core';
import { LabelService } from 'src/app/services/label.service';
import { Label } from '../model/Label';
import { InsertLabelRequest } from '../model/dtos/InsertLabelRequest';
import { UpdateLabelRequest } from '../model/dtos/UpdateLabelRequest';
import { ToastrService } from 'ngx-toastr';

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
    this.labelService.getAllLabels().subscribe(labels => {
      this.labels = labels
    });
  }
  openColorPicker(event: any): void {
    event.preventDefault();
  }

  onUpsertLabelClick(event: any) {
    event.preventDefault();
    if(!this.id) {
    const request = this.createInsertLabelRequest();
    this.labelService.createLabel(request).subscribe(label => {
      if(label) {
        this.labels.push(label);
        this.toastr.success('Label created successfully');
      }
    })
  } else {
    const request = this.createUpdateLabelRequest();
    this.labelService.createLabel(request).subscribe(label => {
      if(label) {
        const index = this.labels.findIndex(label => label.id === this.id);
        if (index !== -1) {
            this.labels[index] = label;
        }
        this.toastr.success('Label updated successfully');
      }
    })
  }
}
  onCancelClick(event: any) {
    event.preventDefault();
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
    this.labelService.deleteLabel(label.id).subscribe(deleted => {
      if(deleted) {
        this.labels = this.labels.filter(l => l.id !== label.id);
        this.toastr.success('Label deleted successfully');
      }
    })
  }
}
