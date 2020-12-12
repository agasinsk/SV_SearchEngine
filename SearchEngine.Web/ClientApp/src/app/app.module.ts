import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { SearchEngineComponent } from './search-engine/search-engine.component';
import { SearchResultListItemComponent } from './search-result-list-item/search-result-list-item.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    SearchEngineComponent,
    SearchResultListItemComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: SearchEngineComponent, pathMatch: 'full' },
      { path: 'search-engine', component: SearchEngineComponent },
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
