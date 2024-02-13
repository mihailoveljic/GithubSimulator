import { Component } from '@angular/core';
import { Repository } from '../model/Repository';
import { RepositoryService } from 'src/app/services/repository_service.service';
import { Observable, of } from 'rxjs';
import { Visibility } from '../model/Visibility';
import { MatDialog } from '@angular/material/dialog';
import { RepositoryDetailsDialogComponent } from '../dialog/repository_details_dialog/repository_details_dialog.component';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

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
    public dialog: MatDialog,
    public toastr: ToastrService,
    private router: Router) {}

  ngOnInit() {
    this.repositories = this.repositoryService.getAllRepositories();
  }

  openDialog(repository: Repository | undefined) {

    this.router.navigate(['/new-repository']);

    // if(repository){
    //   const dialogRef = this.dialog.open(RepositoryDetailsDialogComponent, {
    //     data: repository
    //   });
    //   dialogRef.afterClosed().subscribe((result) => {
    //     console.log(result);
    //     if(result.name){
    //         this.repositoryService.updateRepository(result).subscribe({
    //           next: (repo) => {
    //             this.repositories = this.repositoryService.getAllRepositories();
    //             this.toastr.success('${name} Successfully updated', repo.name);
    //           },
    //           error: (e) => {
    //             this.toastr.error(e.error);
    //           }
    //         });
    //       }
    //       else if(result) {
    //         this.repositoryService.deleteRepository(result).subscribe({
    //           next: (repo) => {
    //             this.repositories = this.repositoryService.getAllRepositories();
    //             this.toastr.success(repository.name + ' Successfully deleted');
    //           },
    //           error: (e) => {
    //             console.log(e);
    //             this.toastr.error(e.error);
    //           }
    //         });
    //       }
    //     });
    // }
    // else {
    //   const dialogRef = this.dialog.open(RepositoryDetailsDialogComponent);
    //   dialogRef.afterClosed().subscribe(result => {
    //     if(result){
    //         this.repositoryService.createRepository(result).subscribe({
    //           next: (repo) => {
    //             this.repositories = this.repositoryService.getAllRepositories();
    //             this.toastr.success(repo.name + 'Successfully created');
    //           },
    //           error: (e) => {
    //             this.toastr.error(e.error);
    //           }
    //         });
    //       }
    //     });
    // }

  }
}
