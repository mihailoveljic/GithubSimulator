import { MatDividerModule } from '@angular/material/divider';
import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { PageComponent } from './page/page.component';
import { Routes, RouterModule } from '@angular/router';
import { AppRoutingModule } from 'src/app/app-routing.module';
import { AuthGuard } from 'src/app/guards/auth.guard';

// Angular Material
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { FilterBarComponent } from './Components/filter-bar/filter-bar.component';
import { IssueListComponent } from './Components/issue-list/issue-list.component';
import { MatTableModule } from '@angular/material/table';
import { IssueDetailsComponent } from './Components/issue-details/issue-details.component';
import { IssueAssignComponent } from './Components/issue-assign/issue-assign.component';
import { IssueHistoryComponent } from './Components/issue-history/issue-history.component'
import { MatListModule } from '@angular/material/list'; 
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { FormsModule } from '@angular/forms';
import { NewIssueComponent } from './Components/new-issue/new-issue.component';
import { IssueAssignNewIssueComponent } from './Components/issue-assign-new-issue/issue-assign-new-issue.component';
import { ReactiveFormsModule } from '@angular/forms';


const routes: Routes = [
  {
    path: ':userName/:repositoryName/issues',
    component: PageComponent,
    canActivate: [AuthGuard],
  },
  {
    //path: 'new-issue',
    path: ':userName/:repositoryName/issues/new',
    component: NewIssueComponent,
    canActivate: [AuthGuard],
  },
  {
    //path: 'issue-details',
    path: ':userName/:repositoryName/issues/details',
    component: IssueDetailsComponent,
    canActivate: [AuthGuard],
  },
];

@NgModule({
  declarations: [
    PageComponent,
    FilterBarComponent,
    IssueListComponent,
    IssueDetailsComponent,
    IssueAssignComponent,
    IssueHistoryComponent,
    NewIssueComponent,
    IssueAssignNewIssueComponent,
  ],
  imports: [
    CommonModule,
    AppRoutingModule,
    RouterModule.forChild(routes),
    MatInputModule,
    MatButtonModule,
    MatFormFieldModule,
    MatGridListModule,
    MatMenuModule,
    MatDividerModule,
    MatIconModule,
    MatTableModule,
    MatListModule,
    MatProgressBarModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  providers: [DatePipe],
  exports: [PageComponent],
})
export class IssuesModule {}
