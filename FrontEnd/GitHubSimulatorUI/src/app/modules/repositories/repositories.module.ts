import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageComponent } from './page/page.component';
import { RouterModule, Routes } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatRadioModule } from '@angular/material/radio';
import { MatDialogModule } from '@angular/material/dialog';
import { RepositoryDetailsDialogComponent } from './dialog/repository_details_dialog/repository_details_dialog.component';
import { AuthGuard } from 'src/app/guards/auth.guard';

const routes: Routes = [
  { path: 'repositories-page', component: PageComponent, canActivate: [AuthGuard] },
];


@NgModule({
  declarations: [
    PageComponent,
    RepositoryDetailsDialogComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    MatCardModule,
    MatIconModule,
    MatFormFieldModule,
    MatSlideToggleModule,
    FormsModule,
    MatInputModule,
    MatButtonModule,
    MatRadioModule,
    MatDialogModule
  ]
})
export class RepositoriesModule { }
