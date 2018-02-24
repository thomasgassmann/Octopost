import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import { FetchRequest } from '../../model/index';

@IonicPage()
@Component({
  selector: 'page-newest',
  templateUrl: 'newest.html',
})
export class NewestPage {

  constructor(public navCtrl: NavController, public navParams: NavParams) {
  }

  public get fetchRequest(): FetchRequest {
    return new FetchRequest((service, data) => service.getNewest(data.page, data.pageSize, data.commentSize), {
      commentSize: 3,
      pageSize: 10
    });
  }
}
