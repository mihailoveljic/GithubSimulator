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
import { MatSelectModule } from '@angular/material/select';
import { RepositoryDetailsDialogComponent } from './dialog/repository_details_dialog/repository_details_dialog.component';
import { AuthGuard } from 'src/app/guards/auth.guard';
import { NewRepositoryComponent } from './new-repository/new-repository.component';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { RepoListComponent } from './repo-list/repo-list.component';
import { YourProfileModule } from '../your-profile/your-profile.module';


const routes: Routes = [
  { path: 'repositories/:userName', component: PageComponent, canActivate: [AuthGuard] },
  { path: 'new-repository', component: NewRepositoryComponent, canActivate: [AuthGuard] }
];


@NgModule({
  declarations: [
    PageComponent,
    RepositoryDetailsDialogComponent,
    NewRepositoryComponent,
    RepoListComponent
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
    MatDialogModule,
    MatSelectModule,
    NgxMatSelectSearchModule,
    YourProfileModule
  ]
})
export class RepositoriesModule { }
