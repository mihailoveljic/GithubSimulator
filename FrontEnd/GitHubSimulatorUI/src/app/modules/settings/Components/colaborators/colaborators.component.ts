import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AddPeopleDialogComponent } from '../add-people-dialog/add-people-dialog.component';
import { RepositoryService } from 'src/app/services/repository_service.service';
import { ActivatedRoute, Router } from '@angular/router';
import { UserRepositoryService } from 'src/app/services/user-repository.service';

@Component({
  selector: 'app-colaborators',
  templateUrl: './colaborators.component.html',
  styleUrls: ['./colaborators.component.scss'],
})
export class ColaboratorsComponent implements OnInit {
  constructor(
    private repositoryService: RepositoryService,
    private userRepositoryService: UserRepositoryService,
    private dialog: MatDialog,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  repoName: string = '';
  repoOwnerName: string = '';

  ngOnInit(): void {
    this.route.parent!.params.subscribe((params: any) => {
      this.repoOwnerName = params['userName'];
      this.repoName = params['repositoryName'];
    });

    this.repositoryService
      .getRepositoryByName(this.repoOwnerName, this.repoName)
      .subscribe((res: any) => {
        this.repoInfo = res;
        console.log(this.repoInfo);

        this.userRepositoryService
          .getUserRepositoriesByRepositoryNameAlt({
            repositoryName: this.repoInfo.name,
          })
          .subscribe((res1: any) => {
            this.directAccessNum = res1.length;
            this.users = res1;
            this.filteredUsers = this.users;
            console.log(res1);
          });
      });
  }

  repoInfo: any = {};
  directAccessNum = 0;
  users: any = [];
  filteredUsers: any = [];

  filterUserString: string = '';

  openDialog(): void {
    const dialogRef = this.dialog.open(AddPeopleDialogComponent, {
      data: { repoName: this.repoInfo.name },
      position: { top: '100px' },
      width: '700px',
      disableClose: false,
      panelClass: 'custom-dialog-container',
    });

    dialogRef.afterClosed().subscribe((res) => {
      if (res) {
        console.log('Result after closing the dialog: ');
        console.log(res.userToAdd);

        this.userRepositoryService
          .addUserToRepository({
            user: { email: res.userToAdd.email.email },
            repositoryName: this.repoInfo.name,
          })
          .subscribe((res1) => {
            console.log(res1);
            this.users.push(res1);
            this.filteredUsers = this.users
            this.directAccessNum++
          });
      }
    });
  }

  getUserRepositoryRole(role: any) {
    if (role === null || role === undefined) return;

    switch (role) {
      case 0:
        return 'Read';
      case 1:
        return 'Triage';
      case 2:
        return 'Write';
      case 3:
        return 'Admin';
      default:
        return 'Owner';
    }
  }

  removeUserFromRepository(user: any) {
    this.userRepositoryService
      .removeUserFromRepository({
        user: { email: user.userEmail },
        repositoryName: this.repoInfo.name,
      })
      .subscribe((res) => {
        this.users = this.users.filter(
          (u: any) => u.userEmail !== user.userEmail
        );

        this.filteredUsers = this.users
        this.directAccessNum--
      });
  }

  changeUserRole(user: any, newRole: any) {
    this.userRepositoryService
      .changeUserRole({
        user: { email: user.userEmail },
        repositoryName: this.repoInfo.name,
        newRole: newRole,
      })
      .subscribe((res: any) => {
        console.log(res);
        let updatedUser = this.users.find(
          (u: any) => u.userEmail === user.userEmail
        );
        updatedUser.userRepositoryRole = res.userRepositoryRole;
      });
  }

  filterUsersByRole(role: number) {
    this.filteredUsers = this.users.filter(
      (u: any) => u.userRepositoryRole === role
    );
  }

  filterByUserEmail() {
    this.filteredUsers = this.users.filter((u: any) =>
      u.userEmail.includes(this.filterUserString)
    );
  }
}
