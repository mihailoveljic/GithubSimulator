import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { RemoreRepoService } from 'src/app/services/remote-repo.service';
import { License } from '../model/License';
import { RepositoryService } from 'src/app/services/repository_service.service';
import { InsertRepositoryRequest } from '../model/dtos/InsertRepositoryRequest';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-new-repository',
  templateUrl: './new-repository.component.html',
  styleUrls: ['./new-repository.component.scss']
})
export class NewRepositoryComponent implements OnInit {

  name: string = '';
  description: string = '';
  selectedVisibility: any;
  gitingoreTemplates: string[] = [];
  licenseTemplates: License[] = [];
  selectedGitingore: string = '';
  selectedLicense: string = '';

  constructor(private router: Router, private remoteRepoService: RemoreRepoService, private repositoryService: RepositoryService, private authService: AuthService) { }

  ngOnInit() {
    this.remoteRepoService.getGitignoreTemplates().subscribe(data => {
      this.gitingoreTemplates = data;
    });

    this.remoteRepoService.getLicencesTemplates().subscribe(data => {
      this.licenseTemplates = data;
    });
  }

  createRepository() {
    let dto : InsertRepositoryRequest = {
      name: this.name,
      description: this.description,
      visibility: this.selectedVisibility,
      gitignores: this.selectedGitingore,
      license: this.selectedLicense
    }

    this.repositoryService.createRepository(dto).subscribe(data => {
      this.router.navigate(['code', this.name, this.authService.getUserName(), 'branch', 'main']);
    });
  }
}
