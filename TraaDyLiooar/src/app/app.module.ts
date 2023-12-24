import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HeaderComponent } from './header/header.component';
import { FormsModule } from '@angular/forms';
import { SecondsToDateTimePipe } from './seconds-to-date-time.pipe';
import { WorkItemsListComponent } from './work-items-list/work-items-list.component';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    SecondsToDateTimePipe,
    WorkItemsListComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { 
  
}
