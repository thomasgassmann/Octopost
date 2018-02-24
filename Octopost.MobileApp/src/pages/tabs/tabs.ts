import { Component } from '@angular/core';
import { NavController } from 'ionic-angular';
import { NewestPage } from '../newest';

@Component({
  selector: 'tabs-home',
  templateUrl: 'tabs.html'
})
export class TabsPage {
  private _newestPage: any = NewestPage;

  constructor(public navCtrl: NavController) {
  }

  public get newestPage(): any {
    return this._newestPage;
  }  
}
