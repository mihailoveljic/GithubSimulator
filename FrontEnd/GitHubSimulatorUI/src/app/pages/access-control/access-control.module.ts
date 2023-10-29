import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccessControlComponent } from './access-control.component';
import { AccessControlRoutingModule } from './access-control-routing.module';

@NgModule({
  imports: [
    CommonModule,
    AccessControlRoutingModule
  ],
  declarations: [AccessControlComponent],
  exports: [AccessControlComponent]
})
export class AccessControlModule { }
