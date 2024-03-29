import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { RepositoryService } from 'src/app/services/repository_service.service';
import { UserRepositoryService } from 'src/app/services/user-repository.service';

@Component({
  selector: 'app-general',
  templateUrl: './general.component.html',
  styleUrls: ['./general.component.scss'],
})
export class GeneralComponent implements OnInit {
  constructor(
    private repositoryService: RepositoryService,
    private userRepositoryService: UserRepositoryService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.route.parent!.params.subscribe((params: any) => {
      this.repoOwnerName = params['userName'];
      this.repoName = params['repositoryName'];

      if (this.repoOwnerName === undefined || this.repoName === undefined) {
        let url = this.route.snapshot.url;
        this.repoOwnerName = url[1].path;
        this.repoName = url[2].path;
      }

      this.userRepositoryService
        .getAuthenticatedUserRepositoryRole(this.repoName)
        .subscribe((resR: any) => {
          this.repoUserRole = resR;
        });
      
      this.repositoryService.getRepositoryBranches(this.repoOwnerName, this.repoName)
        .subscribe((resG: any) => {
          this.allBranches = resG
        })
    });

    this.repositoryService
      .getRepositoryByName(this.repoOwnerName, this.repoName)
      .subscribe((res: any) => {
        this.repoInfo = res;
        this.repoName = res.name;
        this.defaultBranch = res.default_Branch;

        this.userRepositoryService
          .getUserRepositoriesByRepositoryNameAlt({
            repositoryName: this.repoName,
          })
          .subscribe((res1) => {
            this.users = res1;
          });
      });
  }

  repoInfo: any = {};
  repoName: string = '';
  repoOwnerName: string = '';
  repoUserRole: number = -1;
  defaultBranch: string = '';

  allBranches: any = [];

  renameRepo() {
    this.repositoryService
      .updateRepositoryName({
        repositoryOwner: this.repoOwnerName,
        repositoryName: this.repoInfo.name,
        newName: this.repoName,
      })
      .subscribe((res: any) => {
        this.repoName = res.name;
        this.repoInfo.name = res.name;

        const newUrl = `settings/${this.repoOwnerName}/${this.repoName}`;
        this.router.navigateByUrl(newUrl);
      });
  }

  changeToPublic() {
    this.repositoryService
      .updateRepositoryVisibility({
        repositoryOwner: this.repoOwnerName,
        repositoryName: this.repoInfo.name,
        isPrivate: false,
      })
      .subscribe((res: any) => {
        this.repoInfo.private = false;
      });
  }

  changeToPrivate() {
    this.repositoryService
      .updateRepositoryVisibility({
        repositoryOwner: this.repoOwnerName,
        repositoryName: this.repoInfo.name,
        isPrivate: true,
      })
      .subscribe((res: any) => {
        this.repoInfo.private = true;
      });
  }

  archiveRepository() {
    this.repositoryService
      .updateRepositoryArchivedState({
        repositoryOwner: this.repoOwnerName,
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
        repositoryOwner: this.repoOwnerName,
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
        this.router.navigate(['/home-page']);
      });
  }

  /////////////////USERS
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
      return user.userEmail.toLowerCase().includes(userFilterLower);
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
        this.router.navigate(['/home-page']);
      });
  }

  updateDefaultBranch() {
    this.repositoryService
      .updateRepositoryDefaultBranch({
        repositoryOwner: this.repoOwnerName,
        repositoryName: this.repoName,
        newDefaultBranchName: this.defaultBranch,
      })
      .subscribe((res: any) => {
        this.defaultBranch = res.default_Branch;
      });
  }
}
