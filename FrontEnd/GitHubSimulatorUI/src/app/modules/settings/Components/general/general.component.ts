import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { RepositoryService } from 'src/app/services/repository_service.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-general',
  templateUrl: './general.component.html',
  styleUrls: ['./general.component.scss'],
})
export class GeneralComponent implements OnInit {
  constructor(
    private repositoryService: RepositoryService,
    private userService: UserService,
    private router: Router
  ) {}

  // TODO promeni ovo
  ngOnInit(): void {
    this.repositoryService
      .getRepositoryByName('GithubSimulator1')
      .subscribe((res: any) => {
        this.repoInfo = res;
        this.repoName = res.name;
        this.defaultBranch = res.default_Branch;
        console.log(this.repoInfo);
      });

    this.userService.getAllUsers().subscribe((res1) => {
      this.users = res1;
      console.log(res1)
    });
  }

  repoInfo: any = {};
  repoName: string = '';
  defaultBranch: string = '';

  renameRepo() {
    this.repositoryService
      .updateRepositoryName({
        repositoryName: this.repoInfo.name,
        newName: this.repoName,
      })
      .subscribe((res: any) => {
        this.repoName = res.name;
        this.repoInfo.name = res.name;
      });
  }

  changeToPublic() {
    this.repositoryService
      .updateRepositoryVisibility({
        repositoryName: this.repoInfo.name,
        isPrivate: false,
      })
      .subscribe((res: any) => {
        this.repoInfo.private = res.private;
      });
  }

  changeToPrivate() {
    this.repositoryService
      .updateRepositoryVisibility({
        repositoryName: this.repoInfo.name,
        isPrivate: true,
      })
      .subscribe((res: any) => {
        this.repoInfo.private = res.private;
      });
  }

  archiveRepository() {
    this.repositoryService
      .updateRepositoryArchivedState({
        repositoryName: this.repoInfo.name,
        isArchived: true,
      })
      .subscribe((res: any) => {
        this.repoInfo.archived = res.archived;
      });
  }

  unarchiveRepository() {
    this.repositoryService
      .updateRepositoryArchivedState({
        repositoryName: this.repoInfo.name,
        isArchived: false,
      })
      .subscribe((res: any) => {
        this.repoInfo.archived = res.archived;
      });
  }

  deleteRepository() {
    this.repositoryService
      .deleteRepository(this.repoInfo.name)
      .subscribe((res) => {
        console.log(res);
        this.router.navigate(['/home-page']);
      });
  }

  /////////////////USERS
  // TODO promeni da se dovlace samo kolaboratori repozitorijuma
  users: any = [];
  filteredUsers: any = [];
  userFilter: string = '';

  //user.email.email
  filterUsers(): void {
    if (!this.userFilter.trim() || this.userFilter === '') {
      this.filteredUsers = this.users;
      return;
    }
    const userFilterLower = this.userFilter.toLowerCase();

    this.filteredUsers = this.users.filter((user: any) => {
      return user.email.email.toLowerCase().includes(userFilterLower);
    });
  }

  resetUserFilter() {
    this.userFilter = '';
    this.filterUsers();
  }

  transferOwnership(newOwnerEmail: string) {
    this.repositoryService
      .updateRepositoryOwner({
        repositoryName: this.repoInfo.name,
        newOwner: { email: newOwnerEmail },
      })
      .subscribe((res) => {
        console.log(res);
        this.router.navigate(['/home-page']);
      });
  }
}
