import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageComponent } from './page/page.component';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/app/guards/auth.guard';

const routes: Routes = [
  { path: 'settings-page', component: PageComponent, canActivate: [AuthGuard] },
];

@NgModule({
  declarations: [
    PageComponent
  ],
  imports: [
    CommonModule,    
    RouterModule.forChild(routes),

  ],
  exports: [RouterModule],
})
export class SettingsModule { }
