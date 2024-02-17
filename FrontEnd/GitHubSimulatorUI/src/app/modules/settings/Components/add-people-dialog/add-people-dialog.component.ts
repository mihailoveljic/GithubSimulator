import { Component, Inject, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-add-people-dialog',
  templateUrl: './add-people-dialog.component.html',
  styleUrls: ['./add-people-dialog.component.scss'],
})
export class AddPeopleDialogComponent{
  constructor(
    public dialogRef: MatDialogRef<AddPeopleDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: {},
    private userService: UserService
  ) {}

  searchString: string = '';
  users: any = []

  onCloseClick(): void {
    this.dialogRef.close({ test: this.searchString });
  }
}
