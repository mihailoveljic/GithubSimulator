import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { tap, catchError, of } from 'rxjs';
import { BranchDto } from 'src/app/dto/branchDto';
import { BranchService } from 'src/app/services/branch.service';

export class InsertBranchDto {
  old_branch_name: string ="";
  old_ref_name: string ="";
  new_branch_name: string = "";
  repositoryId: string = "";
  issueId: string |null = "";
}
@Component({
  selector: 'app-new-dialog',
  templateUrl: './new-dialog.component.html',
  styleUrls: ['./new-dialog.component.scss'],
})
export class NewDialogComponent {
  constructor(
    private branchService: BranchService, private toastr: ToastrService,
    public dialogRef: MatDialogRef<NewDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
    ) {}

    branchDto : BranchDto = new BranchDto("","","", "", "", "");
    name: string = "";
    source: string = "";
  
    onClose(): void {
      
      this.dialogRef.close();
    }    
    
        onCreate():void{
          let a = new InsertBranchDto();
          a.new_branch_name= this.branchDto.name;
          a.old_branch_name = "";
          a.old_ref_name= this.source;
          a.repositoryId="88e585c5-e4da-4663-8ddb-937bb83081e9"
          a.issueId = null

          if(a.old_ref_name !==""){
      this.branchService.createBranch(a, "first"
      ).pipe(
        tap(branch => {
          if (branch) {
            this.data.branches.push(branch);
            this.dialogRef.close();
            this.toastr.success('Branch created successfully');
          }
        }),
        catchError(error => {
          this.dialogRef.close();
          this.toastr.error('Something went wrong while creating the Branch');
          return of(null); 
        })
      ).subscribe();
      this.dialogRef.close();
    }
  }
}
