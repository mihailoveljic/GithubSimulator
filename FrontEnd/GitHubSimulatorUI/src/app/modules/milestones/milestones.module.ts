import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageComponent } from './page/page.component';
import { RouterModule, Routes } from '@angular/router';
import { AppRoutingModule } from 'src/app/app-routing.module';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { AuthGuard } from 'src/app/guards/auth.guard';
import { HeaderComponent } from './Components/header/header.component';
import { MilestoneListComponent } from './Components/milestone-list/milestone-list.component';
import { NewMilestoneComponent } from './Components/new-milestone/new-milestone.component';
import { MilestoneDetailsComponent } from './Components/milestone-details/milestone-details.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatTableModule } from '@angular/material/table';


const routes: Routes = [
  {
    path: 'milestones-page',
    component: PageComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'new-milestone',
    component: NewMilestoneComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'milestone-details',
    component: MilestoneDetailsComponent,
    canActivate: [AuthGuard],
  },
];

@NgModule({
  declarations: [
    PageComponent,
    HeaderComponent,
    MilestoneListComponent,
    NewMilestoneComponent,
    MilestoneDetailsComponent,
  ],
  imports: [
    CommonModule,
    AppRoutingModule,
    RouterModule.forChild(routes),
    FormsModule,
    MatInputModule,
    MatButtonModule,
    MatProgressBarModule,
    MatDividerModule,
    MatIconModule,
    MatFormFieldModule,
    MatNativeDateModule,
    MatDatepickerModule,
    MatTableModule
  ],
  exports: [PageComponent],
})
export class MilestonesModule {}
