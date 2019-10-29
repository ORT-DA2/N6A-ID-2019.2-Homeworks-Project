import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { HomeworksListComponent } from './homeworks-list/homeworks-list.component';
import { HomeworksFilterPipe } from './homeworks-list/homeworks-filter.pipe';
import { HomeworksService } from './services/homeworks.service';
import { StarComponent } from './star/star.component';
import { WelcomeComponent } from './welcome/welcome.component';
import { HttpClientModule } from '@angular/common/http';
import { HomeworkDetailComponent } from './homework-detail/homework-detail.component';
import { HomeworkDetailGuard } from './guards/homework-detail.guard';

@NgModule({
  declarations: [
    AppComponent,
    HomeworksListComponent,
    HomeworksFilterPipe,
    StarComponent,
    WelcomeComponent,
    HomeworkDetailComponent
  ],
  imports: [
    HttpClientModule,
    FormsModule,
    BrowserModule,
    RouterModule.forRoot([
        { path: 'homeworks', component: HomeworksListComponent },
        { path: 'homeworks/:id', 
          component: HomeworkDetailComponent,
          canActivate: [HomeworkDetailGuard]
        },
        { path: 'welcome', component:  WelcomeComponent }, 
        { path: '', redirectTo: 'welcome', pathMatch: 'full' },
        { path: '**', redirectTo: 'welcome', pathMatch: 'full'}
    ])
  ],
  providers: [
    HomeworksService,
    HomeworkDetailGuard
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }