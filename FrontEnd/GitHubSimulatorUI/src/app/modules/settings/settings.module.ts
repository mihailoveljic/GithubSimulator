import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageComponent } from './page/page.component';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/app/guards/auth.guard';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { GeneralComponent } from './Components/general/general.component';
import { BranchComponent } from './Components/branch/branch.component';
import { ColaboratorsComponent } from './Components/colaborators/colaborators.component';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatMenuModule } from '@angular/material/menu';
import { MatDialogModule } from '@angular/material/dialog';
import { AddPeopleDialogComponent } from './Components/add-people-dialog/add-people-dialog.component';
import { FormsModule } from '@angular/forms';
import { AddBranchRuleComponent } from './Components/add-branch-rule/add-branch-rule.component';
import { MatCheckboxModule } from '@angular/material/checkbox';

// const routes: Routes = [
//   { path: 'settings-page', component: PageComponent, canActivate: [AuthGuard] },
//   {
//     path: 'app-general',
//     component: GeneralComponent,
//     canActivate: [AuthGuard],
//   },
//   {
//     path: 'app-colaborators',
//     component: ColaboratorsComponent,
//     canActivate: [AuthGuard],
//   },
//   { path: 'app-branch', component: BranchComponent, canActivate: [AuthGuard] },
// ];

const routes: Routes = [
  {
    path: 'settings-page',
    component: PageComponent,
    children: [
      { path: '', redirectTo: 'app-general', pathMatch: 'full' }, // Default route
      { path: 'app-general', component: GeneralComponent },
      { path: 'app-colaborators', component: ColaboratorsComponent },
      { path: 'app-branch', component: BranchComponent },
      { path: 'app-add-branch-rule', component: AddBranchRuleComponent },
    ],
  },
];

@NgModule({
  declarations: [
    PageComponent,
    GeneralComponent,
    BranchComponent,
    ColaboratorsComponent,
    AddPeopleDialogComponent,
    AddBranchRuleComponent
  ],
  imports: [
    CommonModule,    
    RouterModule.forChild(routes),
    MatSidenavModule,
    MatListModule,
    MatDividerModule,
    MatIconModule,
    MatButtonModule,
    MatCardModule,
    MatMenuModule,
    MatDialogModule,
    MatCheckboxModule,
    FormsModule
  ],
  exports: [RouterModule],
})
export class SettingsModule { }
