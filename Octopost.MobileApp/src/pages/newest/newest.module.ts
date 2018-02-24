import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { NewestPage } from './newest';
import { ComponentsModule } from '../../components';
import { PostFilterProvider } from '../../providers/post-filter/post-filter';

@NgModule({
  declarations: [
    NewestPage
  ],
  imports: [
    IonicPageModule.forChild(NewestPage),
    ComponentsModule.forRoot()
  ],
  providers: [
    PostFilterProvider
  ]
})
export class NewestPageModule {}
