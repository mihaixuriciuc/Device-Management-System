import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DeviceService {
  //here is the conection to the api
  private apiUrl = 'http://localhost:5246/api/devices'; 

  constructor(private http: HttpClient) { }

  // Get all devices
  getDevices(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  // Create a device
  addDevice(device: any): Observable<any> {
    return this.http.post<any>(this.apiUrl, device);
  }
}