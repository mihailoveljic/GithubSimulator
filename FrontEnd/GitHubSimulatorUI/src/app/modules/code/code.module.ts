import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageComponent } from './page/page.component';
import { Routes, RouterModule, UrlSegment, UrlSegmentGroup, Route, UrlMatchResult } from '@angular/router';
import { AppRoutingModule } from 'src/app/app-routing.module';
import { AuthGuard } from 'src/app/guards/auth.guard';
import { DirectoryTableComponent } from './directory-table/directory-table.component';
import { MatButtonToggleModule } from '@angular/material/button-toggle';

const routes: Routes = [
  { path: 'code/:userName/:repositoryName/branch/:branchName', component: PageComponent, canActivate: [AuthGuard] },
  { matcher: filepathMatcher, component: PageComponent, canActivate: [AuthGuard] }
];

function filepathMatcher(segments: UrlSegment[], group: UrlSegmentGroup, route: Route) : UrlMatchResult | null {
  // match urls like "code/:userName/:repositoryName/branch/:branchName/:path" where path can contain '/'
  if (segments.length > 5) {
    // if first segment is 'code', then concat all the next segments into a single one
    // and return it as a parameter named 'filepath'
    if (segments[0].path == "code") {
      return {
        consumed: segments,
        posParams: {
          userName: new UrlSegment(segments[1].path, {}),
          repositoryName: new UrlSegment(segments[2].path, {}),
          branchName: new UrlSegment(segments[4].path, {}),
          path: new UrlSegment(segments.slice(5).join("/"), {})
        }
      };
    }
  }
  return null;
}

@NgModule({
  declarations: [
    PageComponent,
    DirectoryTableComponent
  ],
  imports: [
    CommonModule,
    AppRoutingModule,
    MatButtonToggleModule,
    RouterModule.forChild(routes),
  ],
  exports: [
    RouterModule
  ]
})
export class CodeModule { }
