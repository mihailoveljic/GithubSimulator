import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserRepositoryService } from 'src/app/services/user-repository.service';

@Component({
  selector: 'app-settings-page',
  templateUrl: './page.component.html',
  styleUrls: ['./page.component.scss'],
})
export class PageComponent implements OnInit {
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private userRepositoryService: UserRepositoryService
  ) {}

  repoOwnerName: string = '';
  repoName: string = '';
  repoUserRole: number = -1;

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
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

          if (this.repoUserRole < 3 || this.repoUserRole > 4) {
            this.router.navigate(['/home-page'])
          }
        });
    });
  }
}
