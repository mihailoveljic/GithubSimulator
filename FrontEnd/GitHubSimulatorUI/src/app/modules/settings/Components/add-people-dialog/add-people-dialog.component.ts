import { Component, Inject, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-add-people-dialog',
  templateUrl: './add-people-dialog.component.html',
  styleUrls: ['./add-people-dialog.component.scss'],
})
export class AddPeopleDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<AddPeopleDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private userService: UserService
  ) {}

  searchString: string = '';
  users: any = [];
  clickedUsers: boolean[] = []
  selectedUser: any = {};

  timeout: any;

  onCloseClick(): void {
    this.dialogRef.close({ userToAdd: this.selectedUser });
  }

  getUsers() {
    this.userService
      .getUsersNotInRepo({ repositoryName: this.data.repoName, searchString: this.searchString})
      .subscribe((res) => {
        console.log(res);
        this.users = res;
        this.users.forEach(() => this.clickedUsers.push(false))
      });
  }

  onSearchStringChange() {
    clearTimeout(this.timeout);

    this.timeout = setTimeout(() => {
      this.getUsers();
    }, 1000);
  }

  selectUser(user: any, index: number) {
    if (this.selectedUser === user) {
      this.clickedUsers[index] = false;
      this.selectedUser = {}
      return
    }

    this.clickedUsers = []
    this.users.forEach(() => this.clickedUsers.push(false));

    this.selectedUser = user
    this.clickedUsers[index] = true
  }
}
