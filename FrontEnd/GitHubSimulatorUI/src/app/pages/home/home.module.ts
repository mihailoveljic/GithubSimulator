import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageComponent } from './page/page.component';
import { Routes, RouterModule } from '@angular/router';
import { NavbarModule } from 'src/app/shared/navbar/navbar.module';
import { AuthGuard } from 'src/app/guards/auth.guard';
import { RepositoriesModule } from 'src/app/modules/repositories/repositories.module';

const routes: Routes = [
  { path: 'home-page', component: PageComponent, canActivate: [AuthGuard]},
];

@NgModule({
  declarations: [
    PageComponent
  ],
  imports: [
    CommonModule,
    RepositoriesModule,
    RouterModule.forChild(routes),
  ]
})
export class HomeModule {}
