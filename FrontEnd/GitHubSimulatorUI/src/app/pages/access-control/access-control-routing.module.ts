import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AccessControlComponent } from './access-control.component';

const routes: Routes = [
  { path: '', component: AccessControlComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AccessControlRoutingModule { }
