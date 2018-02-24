import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class LocationService {
  private lat: number | undefined = undefined;
  private lon: number | undefined = undefined;

  constructor(private httpClient: HttpClient) {
    navigator.geolocation.getCurrentPosition(position => {
      this.lat = position.coords.latitude;
      this.lon = position.coords.longitude;
    });
    this.httpClient.get('http://ip-api.com/json').toPromise().then(position => {
      if (this.lat === undefined || this.lon === undefined) {
        this.lat = <number>position['lat'];
        this.lon = <number>position['lon'];
      }
    });
  }

  public async getLocation(): Promise<{ longitude: number, latitude: number }> {
    return Promise.resolve({ longitude: this.lon, latitude: this.lat });
  }
}
