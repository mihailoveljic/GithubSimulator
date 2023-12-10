import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Repository } from '../../model/Repository';

@Component({
  selector: 'app-repository_details_dialog',
  templateUrl: './repository_details_dialog.component.html',
  styleUrls: ['./repository_details_dialog.component.css']
})
export class RepositoryDetailsDialogComponent implements OnInit {

  selectedVisibility: any;
  enteredDescription : any;

  constructor(@Inject(MAT_DIALOG_DATA) public data: Repository | undefined) { }

  ngOnInit() {
    if(this.data){
      this.enteredDescription = this.data.description;
      this.selectedVisibility = this.data.visibility;
    }
  }

}
