import { Component, OnInit } from '@angular/core';
import { IssueService } from 'src/app/services/issue_service.service';

@Component({
  selector: 'app-access-control',
  templateUrl: './access-control.component.html',
  styleUrls: ['./access-control.component.css']
})
export class AccessControlComponent implements OnInit {

  constructor(
    private issueService: IssueService
  ) { }

  ngOnInit() {
    this.issueService.getAllIssues().subscribe({
      next: (result) => {
        console.log(result);
      },
      error: (error : any) => {
        console.log(error);
      }
    })
  }

}
