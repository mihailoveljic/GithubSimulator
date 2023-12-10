import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { PageComponent } from '../your-profile/page/page.component';
import { AuthGuard } from 'src/app/guards/auth.guard';


const routes: Routes = [
  { path: 'your-profile-page', component: PageComponent, canActivate: [AuthGuard] },
];

@NgModule({
  declarations: [
    PageComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
  ]
})
export class YourProfileModule { }
