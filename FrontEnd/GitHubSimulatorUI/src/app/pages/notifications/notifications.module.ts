import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageComponent } from './page/page.component';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from 'src/app/guards/auth.guard';

const routes: Routes = [
  { path: 'notification-page', component: PageComponent, canActivate: [AuthGuard] },
];

@NgModule({
  declarations: [PageComponent],
  imports: [CommonModule, RouterModule.forChild(routes)],
})
export class NotificationsModule {}
