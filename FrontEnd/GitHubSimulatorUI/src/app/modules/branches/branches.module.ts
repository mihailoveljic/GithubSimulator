import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageComponent } from './page/page.component';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/app/guards/auth.guard';
import { AppRoutingModule } from 'src/app/app-routing.module';
import { NewDialogComponent } from './new-dialog/new-dialog.component';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTableModule } from '@angular/material/table';
import {MatSelectModule} from '@angular/material/select';
import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import { FormsModule } from '@angular/forms';

const routes: Routes = [
  { path: 'branches-page', component: PageComponent },
];


@NgModule({
  declarations: [
    PageComponent,
    NewDialogComponent,
  ],
  imports: [
    CommonModule,
    AppRoutingModule,
    RouterModule.forChild(routes),
    MatTabsModule,
    FormsModule,
    MatTableModule,
    MatSelectModule,
    MatInputModule,
    MatFormFieldModule
  ]
})
export class BranchesModule { }
