import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { BranchDto } from 'src/app/dto/branchDto';
import { BranchService } from 'src/app/services/branch.service';
import { catchError, of, tap, throwError } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { NewDialogComponent } from '../new-dialog/new-dialog.component';
import { RenameDialogComponent } from '../rename-dialog/rename-dialog.component';
import { FormControl } from '@angular/forms';

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

  searchControl: FormControl = new FormControl();
  name: string = '';
  id: string = '';
  dataSource: BranchDto[] = [];
  branches: BranchDto[] =[];


  ngOnInit(): void {
    this.branchService.getBranches().pipe(
      catchError(error => {
        this.toastr.error('Something went wrong while fetching branches');

        return of([]); // Return an empty array or appropriate fallback value
      })
    ).subscribe(branches => {
      console.log(branches)
      this.dataSource = branches;
      this.branches = branches;
    });
    }
  displayedColumns: string[] = ['name', 'checkstatus', 'pullrequest', 'symbol'];


  animal: string="";


  openDialog(): void {
    const dialogRef = this.dialog.open(NewDialogComponent, {
      data: { title: 'Create a branch', message: 'This is my dialog message.', branches:this.dataSource, }
    });

    dialogRef.afterClosed().subscribe(()=>{
      this.branchService.getBranches().pipe(
        catchError(error => {
          this.toastr.error('Something went wrong while fetching branches');
          return of([]); 
        })
      ).subscribe(branches => {
        console.log(branches)
        this.dataSource = branches;
        this.branches = this.dataSource;
      });
    })
  }

  onDelete(branch: BranchDto): void{
    console.log("Ovo je grana koja se brise",branch)
    if(branch.id != null){
      this.branchService.deleteBranch(branch.id).pipe(
        catchError(error => {
          if (error.status === 200) {
            console.log('Branch deleted successfully');
            this.dataSource = this.dataSource.filter(l => l.id !== branch.id);
            this.branches = this.dataSource;
            this.toastr.success('Branch deleted successfully');
          } else {
            console.error('Error while deleting branch:', error);
            return throwError(error); // Prosleđujemo grešku dalje
          }
          return of(); 
        })
      ).subscribe(() => {
        this.branchService.getBranches().pipe(
          catchError(error => {
            this.toastr.error('Something went wrong while fetching branches');
            return of([]); 
          })
        ).subscribe(branches => {
          console.log(branches)
          this.dataSource = branches;
        });
        console.log("lista posle brisanja", this.dataSource)
        this.toastr.success('Branch deleted successfully');
      });
      
    }
  }

  onPullRequest(): void{}
  onRenameBranch(branch: BranchDto): void{
    console.log(branch)
    const dialogRef = this.dialog.open(RenameDialogComponent, {
      data: { title: 'Rename branch', message: 'This is my dialog message.', branche:branch, }
    });

    dialogRef.afterClosed().subscribe(()=>{
      this.branchService.getBranches().pipe(
        catchError(error => {
          this.toastr.error('Something went wrong while fetching branches');
          return of([]); 
        })
      ).subscribe(branches => {
        console.log(branches)
        this.dataSource = branches;
        this.branches = this.dataSource;
      });
    })
  }

  onInputChange(event: Event) {
    const value = (event.target as HTMLInputElement).value;
    console.log('Nova vrednost unesena:', value);
    let filtriranaLista = this.branches.filter(objekat => objekat.name.includes(value));
    this.dataSource = filtriranaLista;
    console.log(filtriranaLista)
  }

}
