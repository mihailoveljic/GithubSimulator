import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { PageComponent } from './page/page.component';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/app/guards/auth.guard';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatTableModule } from '@angular/material/table';
import { AppRoutingModule } from 'src/app/app-routing.module';
import { FilterBarComponent } from './Components/filter-bar/filter-bar.component';
import { PRAssignNewPRComponent } from './Components/pr-assign-new-pr/pr-assign-new-pr.component';
import { PRAssignComponent } from './Components/pr-assign/pr-assign.component';
import { PRDetailsComponent } from './Components/pr-details/pr-details.component';
import { PRHistoryComponent } from './Components/pr-history/pr-history.component';
import { PRListComponent } from './Components/pr-list/pr-list.component';
import { NewPRComponent } from './Components/new-pr/new-pr.component';

const routes: Routes = [
  { path: 'pull-requests-page', component: PageComponent, canActivate: [AuthGuard] },
  { path: 'pr-details', component: PRDetailsComponent, canActivate: [AuthGuard],
  },
  { path: 'new-pr', component: NewPRComponent, canActivate: [AuthGuard],
  },

];


@NgModule({
  declarations: [
    PageComponent,
    FilterBarComponent,
    PRListComponent,
    PRDetailsComponent,
    PRAssignComponent,
    PRHistoryComponent,
    NewPRComponent,
    PRAssignNewPRComponent,
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
export class PullRequestsModule { }
