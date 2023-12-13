import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Repository } from '../../model/Repository';
import { InsertRepositoryRequest } from '../../model/dtos/InsertRepositoryRequest';
import { UpdateRepositoryRequest } from '../../model/dtos/UpdateRepositoryRequest';

@Component({
  selector: 'app-repository_details_dialog',
  templateUrl: './repository_details_dialog.component.html',
  styleUrls: ['./repository_details_dialog.component.css']
})
export class RepositoryDetailsDialogComponent implements OnInit {

  selectedVisibility: any;
  enteredDescription : any;
  enteredName : any;


  constructor(@Inject(MAT_DIALOG_DATA) public data: Repository | undefined,
              public dialogRef: MatDialogRef<RepositoryDetailsDialogComponent>) { }

  ngOnInit() {
    if(this.data){
      this.enteredDescription = this.data.description;
      this.selectedVisibility = this.data.visibility;
      this.enteredName = this.data.name;
    }
  }

  save() {
    if(this.data) {
      let dto : UpdateRepositoryRequest = {
        id: this.data.id,
        name: this.enteredName,
        description: this.enteredDescription,
        visibility: this.selectedVisibility,
      }
      this.dialogRef.close(dto);
    } else {
      let dto : InsertRepositoryRequest = {
        name: this.enteredName,
        description: this.enteredDescription,
        visibility: this.selectedVisibility,
      }
      this.dialogRef.close(dto);
    }
  }

  delete(id: string) {
    this.dialogRef.close(id);
  }

}
