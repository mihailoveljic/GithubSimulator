import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PageComponent } from './pages/login/page/page.component';


const routes: Routes = [
  { path: 'access-control', loadChildren: () => import('./pages/access-control/access-control.module').then(m => m.AccessControlModule) },
  { path: '', redirectTo: '/login-page', pathMatch: 'full' }, 
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes),
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
