import { BrowserModule } from '@angular/platform-browser';
import { ErrorHandler, NgModule } from '@angular/core';
import { IonicApp, IonicErrorHandler, IonicModule } from 'ionic-angular';
import { SplashScreen } from '@ionic-native/splash-screen';
import { StatusBar } from '@ionic-native/status-bar';

import { MyApp } from './app.component';
import { TabsPage, NewestPageModule } from '../pages';
import { ComponentsModule } from '../components';
import { PostFilterProvider } from '../providers/post-filter/post-filter';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@NgModule({
  declarations: [
    MyApp,
    TabsPage
  ],
  imports: [
    CommonModule,
    BrowserModule,
    IonicModule.forRoot(MyApp),
    NewestPageModule,
    ComponentsModule.forRoot()
  ],
  bootstrap: [ IonicApp ],
  entryComponents: [
    MyApp,
    TabsPage
  ],
  providers: [
    StatusBar,
    SplashScreen,
    { provide: ErrorHandler, useClass: IonicErrorHandler },
    PostFilterProvider
  ]
})
export class AppModule {}
