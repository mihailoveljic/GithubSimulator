import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarModule } from './shared/navbar/navbar.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { SettingsModule } from './modules/settings/settings.module';
import { PullRequestsModule } from './modules/pull-requests/pull-requests.module';
import { ProjectsModule } from './modules/projects/projects.module';
import { IssuesModule } from './modules/issues/issues.module';
import { CodeModule } from './modules/code/code.module';
import { RepositoriesModule } from './modules/repositories/repositories.module';
import { YourProfileModule } from './modules/your-profile/your-profile.module';
import { LoginModule } from './pages/login/login.module';
import { HomeModule } from './pages/home/home.module';
import { ToastrModule } from 'ngx-toastr';
import { TokenInterceptor } from './interceptors/token.interceptor';
import { RegisterModule } from './pages/register/register.module';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NavbarModule,
    BrowserAnimationsModule,
    HttpClientModule,
    SettingsModule,
    PullRequestsModule,
    ProjectsModule,
    IssuesModule,
    CodeModule,
    RepositoriesModule,
    YourProfileModule,
    LoginModule,
    HomeModule,
    RegisterModule,
    ToastrModule.forRoot({
      positionClass :'toast-bottom-right'
    })
  ],
  providers: [{
    provide: HTTP_INTERCEPTORS,
    useClass: TokenInterceptor,
    multi: true
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
