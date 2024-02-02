import { MatDividerModule } from '@angular/material/divider';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
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
import { MatTableModule } from '@angular/material/table'

const routes: Routes = [
  { path: 'issues-page', component: PageComponent, canActivate: [AuthGuard] },
];

@NgModule({
  declarations: [PageComponent, FilterBarComponent, IssueListComponent],
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
    MatTableModule
  ],
  exports: [PageComponent],
})
export class IssuesModule {}
