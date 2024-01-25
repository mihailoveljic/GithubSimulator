import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { BranchDto } from 'src/app/dto/branchDto';
import { BranchService } from 'src/app/services/branch.service';
import { catchError, of, tap } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { NewDialogComponent } from '../new-dialog/new-dialog.component';

export interface DialogData {
  animal: string;
  name: string;
}

@Component({
  selector: 'app-page',
  templateUrl: './page.component.html',
  styleUrls: ['./page.component.scss']
})
export class PageComponent  implements OnInit {
  constructor(private branchService: BranchService, private toastr: ToastrService, public dialog: MatDialog){}

  name: string = '';
  branches: BranchDto[] = [];
  id: string = '';
  dataSource: BranchDto[] = [];


  ngOnInit(): void {
    this.branchService.getBranches().pipe(
      catchError(error => {
        this.toastr.error('Something went wrong while fetching branches');
        console.log(this.branches)

        return of([]); // Return an empty array or appropriate fallback value
      })
    ).subscribe(branches => {
      this.branches = branches;
    });
    console.log(this.branches)

    this.dataSource = this.branches;
  }
  displayedColumns: string[] = ['name', 'checkstatus', 'pullrequest', 'symbol'];


  animal: string="";


  openDialog(): void {
    const dialogRef = this.dialog.open(NewDialogComponent, {
      data: { title: 'Create a branch', message: 'This is my dialog message.', branches:this.branches, }
    });
  }
}
