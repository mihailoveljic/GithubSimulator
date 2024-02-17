import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageComponent } from './page/page.component';
import { Routes, RouterModule, UrlSegment, UrlSegmentGroup, Route, UrlMatchResult } from '@angular/router';
import { AppRoutingModule } from 'src/app/app-routing.module';
import { AuthGuard } from 'src/app/guards/auth.guard';
import { DirectoryTableComponent } from './directory-table/directory-table.component';

const routes: Routes = [
  { path: 'code/:repositoryName/branch/:branchName', component: PageComponent, canActivate: [AuthGuard] },
  { matcher: filepathMatcher, component: PageComponent, canActivate: [AuthGuard] }
];

function filepathMatcher(segments: UrlSegment[], group: UrlSegmentGroup, route: Route) : UrlMatchResult | null {
  // match urls like "code/:repositoryName/branch/:branchName/:path" where path can contain '/'
  if (segments.length > 4) {
    // if first segment is 'code', then concat all the next segments into a single one
    // and return it as a parameter named 'filepath'
    if (segments[0].path == "code") {
      console.log(segments.slice(4).join("/"));
      console.log(segments);
      return {
        consumed: segments,
        posParams: {
          repositoryName: new UrlSegment(segments[1].path, {}),
          branchName: new UrlSegment(segments[3].path, {}),
          path: new UrlSegment(segments.slice(4).join("/"), {})
        }
      };
    }
  }
  console.log("error matching url");
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
    RouterModule.forChild(routes),
  ],
  exports: [
    RouterModule
  ]
})
export class CodeModule { }
