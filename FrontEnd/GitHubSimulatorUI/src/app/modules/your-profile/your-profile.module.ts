import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { PageComponent } from '../your-profile/page/page.component';
import { AuthGuard } from 'src/app/guards/auth.guard';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { AuthService } from 'src/app/services/auth.service';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { MatButtonModule } from '@angular/material/button';

const routes: Routes = [
  { path: 'your-profile-page', component: PageComponent, canActivate: [AuthGuard] },
];

@NgModule({
  declarations: [
    PageComponent
  ],
  imports: [
    CommonModule,
    MatFormFieldModule,
    MatInputModule,
    MatCardModule,
    MatButtonModule, 
    FormsModule,
    BrowserAnimationsModule,
    RouterModule.forChild(routes),
  ]
})
export class YourProfileModule { }
