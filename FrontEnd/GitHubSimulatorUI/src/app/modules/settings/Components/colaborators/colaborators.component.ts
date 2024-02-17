import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AddPeopleDialogComponent } from '../add-people-dialog/add-people-dialog.component';

@Component({
  selector: 'app-colaborators',
  templateUrl: './colaborators.component.html',
  styleUrls: ['./colaborators.component.scss']
})
export class ColaboratorsComponent {
  constructor(private dialog: MatDialog) {}

  openDialog() : void {
    const dialogRef = this.dialog.open(AddPeopleDialogComponent, {
      data: {},
      position: { top: '100px' },
      width: '700px',
      disableClose: false,
      panelClass: 'custom-dialog-container',
    });

    dialogRef.afterClosed().subscribe((res) => {
      if (res) {
        console.log('Result after closing the dialog: ');
        console.log(res.test);
      }
    })
  }
}
