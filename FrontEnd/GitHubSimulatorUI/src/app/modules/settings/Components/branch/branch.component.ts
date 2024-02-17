import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-branch',
  templateUrl: './branch.component.html',
  styleUrls: ['./branch.component.scss'],
})
export class BranchComponent {
  constructor(private router: Router) {}

  //rules: any = ['test'];
  rules: any = [];

  goToCreateBranchRulePage() {
    this.router.navigate(['/settings-page/app-add-branch-rule']);
  }
}
