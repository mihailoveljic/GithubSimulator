import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { tap, catchError, of } from 'rxjs';
import { BranchDto } from 'src/app/dto/branchDto';
import { BranchService } from 'src/app/services/branch.service';
import { NewDialogComponent } from '../new-dialog/new-dialog.component';


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
    ) {console.log(data)}

    
    onClose(): void {
      
      this.dialogRef.close();
    }    
    
    onRename():void{
          

      this.branchService.updateLabel(this.data.branche
      ).pipe(
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
