import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageComponent } from './page/page.component';
import { RouterModule, Routes } from '@angular/router';
import { AppRoutingModule } from 'src/app/app-routing.module';
import { AuthGuard } from 'src/app/guards/auth.guard';


const routes: Routes = [
  { path: 'projects-page', component: PageComponent, canActivate: [AuthGuard] },

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
