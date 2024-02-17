import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { tap, catchError, of } from 'rxjs';
import { BranchDto } from 'src/app/dto/branchDto';
import { BranchService } from 'src/app/services/branch.service';
import { InsertBranchDto, NewDialogComponent } from '../new-dialog/new-dialog.component';


@Component({
  selector: 'app-rename-dialog',
  templateUrl: './rename-dialog.component.html',
  styleUrls: ['./rename-dialog.component.scss']
})
export class RenameDialogComponent  {
  constructor(
    private branchService: BranchService, private toastr: ToastrService,
    public dialogRef: MatDialogRef<NewDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
    ) {console.log(this.data); this.a.old_branch_name = this.data.branche.name}
    a: InsertBranchDto = new InsertBranchDto();
    
    onClose(): void {
      
      this.dialogRef.close();
    }    
    
    onRename():void{
           
          this.a.new_branch_name= this.data.branche.name;
          this.a.repositoryId="88e585c5-e4da-4663-8ddb-937bb83081e9"
          this.a.issueId = null

          console.log("pre isporuke", this.a)
      this.branchService.createBranch(this.a, "first").pipe(
        tap(branch => {
          if (branch) {
            this.dialogRef.close();
            this.toastr.success('Branch renamed successfully');
          }
        }),
        
      ).subscribe();
      this.dialogRef.close();
    }
    
}
