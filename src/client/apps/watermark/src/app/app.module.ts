import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { WatermarkModule } from "@groupdocs.examples.angular/watermark";

@NgModule({
  declarations: [AppComponent],
  imports: [BrowserModule,
    WatermarkModule.forRoot("http://localhost:8080")],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {}
