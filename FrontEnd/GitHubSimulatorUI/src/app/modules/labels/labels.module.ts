import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LabelsComponent } from './page/labels.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { RouterModule, Routes } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { ColorPickerModule } from 'ngx-color-picker';
import { MatDividerModule } from '@angular/material/divider';
import { MatMenuModule } from '@angular/material/menu';

//TODO: add can activate route guard
const routes: Routes = [
  { path: 'labels-page', component: LabelsComponent },
];

@NgModule({
  imports: [
    CommonModule,
    ColorPickerModule,
    MatFormFieldModule,
    MatCardModule,
    MatIconModule,
    MatButtonModule,
    ReactiveFormsModule,
    MatInputModule,
    FormsModule,
    MatDividerModule,
    MatMenuModule,
    RouterModule.forChild(routes),
  ],
  declarations: [LabelsComponent],
  exports: [ColorPickerModule],
})
export class LabelsModule {}
