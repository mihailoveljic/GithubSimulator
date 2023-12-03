import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageComponent } from './page/page.component';
import { RouterModule, Routes } from '@angular/router';
import { AppRoutingModule } from 'src/app/app-routing.module';


const routes: Routes = [
  { path: 'projects-page', component: PageComponent },

];

@NgModule({
  declarations: [
    PageComponent
  ],
  imports: [
    CommonModule,
    AppRoutingModule,
    RouterModule.forChild(routes),
  ]
})
export class ProjectsModule { }
