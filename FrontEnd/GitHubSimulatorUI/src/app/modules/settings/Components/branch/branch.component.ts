import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-branch',
  templateUrl: './branch.component.html',
  styleUrls: ['./branch.component.scss'],
})
export class BranchComponent implements OnInit {
  constructor(private router: Router, private route: ActivatedRoute) {}

  repoName: string = '';
  repoOwnerName: string = '';

  ngOnInit(): void {
    this.route.parent!.params.subscribe((params: any) => {
      this.repoOwnerName = params['userName'];
      this.repoName = params['repositoryName'];
    });
  }

  rules: any = [];

  goToCreateBranchRulePage() {
    this.router.navigate([this.repoOwnerName + '/' + this.repoName + '/settings/branch_protection_rules/new']);
  }
}
