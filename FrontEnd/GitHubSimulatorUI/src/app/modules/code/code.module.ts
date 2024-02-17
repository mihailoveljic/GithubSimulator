import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageComponent } from './page/page.component';
import { Routes, RouterModule } from '@angular/router';
import { AppRoutingModule } from 'src/app/app-routing.module';
import { AuthGuard } from 'src/app/guards/auth.guard';
import { MatSelectModule } from '@angular/material/select';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { ReactiveFormsModule } from '@angular/forms';


const routes: Routes = [
  { path: 'code-page', component: PageComponent, canActivate: [AuthGuard] },

];


@NgModule({
  declarations: [
    PageComponent
  ],
  imports: [
    CommonModule,
    AppRoutingModule,
    RouterModule.forChild(routes),
    MatSelectModule,
    MatAutocompleteModule,
    ReactiveFormsModule
  ]
})
export class CodeModule { }
