import { Component } from '@angular/core';
import { Repository } from '../model/Repository';
import { RepositoryService } from 'src/app/services/repository_service.service';
import { Observable, of } from 'rxjs';
import { Visibility } from '../model/Visibility';
import { MatDialog } from '@angular/material/dialog';
import { RepositoryDetailsDialogComponent } from '../dialog/repository_details_dialog/repository_details_dialog.component';

@Component({
  selector: 'app-page',
  templateUrl: './page.component.html',
  styleUrls: ['./page.component.scss']
})
export class PageComponent {
  public Visibility = Visibility;
  repositories : Observable<Repository[]> = of([]);
  toggleValue: any;
  searchTerm: any;
  constructor(
    private repositoryService: RepositoryService,
    public dialog: MatDialog) {}

  ngOnInit() {
    this.repositories = this.repositoryService.getAllRepositories();
  }

  openDialog(repository: Repository | undefined) {
    if(repository){
      this.dialog.open(RepositoryDetailsDialogComponent, {
        data: repository
      });
    }
    else {
      const dialogRef = this.dialog.open(RepositoryDetailsDialogComponent);
    }
    // const dialogRef = this.dialog.open(AddPriceDialogComponent);
    // dialogRef.afterClosed().subscribe(result => {
    //   if(result){
    //     if( !this.validDate(result.startingDate) || !this.validDate(result.endingDate) ||
    //      this.endDateBeforeStartDate(result.startingDate, result.endingDate)) {
    //       this.openDialog(accommodation)
    //     }
    //     else {

    //       let startingDateString = this.datepipe.transform(result.startingDate, 'MM/dd/yyyy')??''
    //       let endingDateString = this.datepipe.transform(result.endingDate, 'MM/dd/yyyy')??''
    //       this.price = {accommodationId : accommodation.id, startDate: startingDateString, endDate: endingDateString, value: result.price }
    //       console.log(this.price)
    //       this.accService.AddPrice(this.price).subscribe({
    //         next: (res) => {
    //           this.showSuccess('Successfully added price');
    //         },
    //         error: (e) => {
    //           this.showError(e.error);
    //           this.openDialog(accommodation)
    //         }
    //       });
    //     }
    //   }
    // });
  }
}
