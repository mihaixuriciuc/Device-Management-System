import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Device } from '../models/device.model';

@Injectable({
  providedIn: 'root',
})
export class DeviceService {
  //here is the conection to the api
  private apiUrl = 'http://localhost:5246/api/devices';

  constructor(private http: HttpClient) {}

  // Get all devices
  getDevices(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }
  getDeviceById(id: number): Observable<Device> {
    return this.http.get<Device>(`${this.apiUrl}/${id}`);
  }
  // Add this inside your DeviceService class
  deleteDevice(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  // Add this to your DeviceService class
  createDevice(device: any): Observable<Device> {
    return this.http.post<Device>(this.apiUrl, device);
  }

  updateDevice(id: number, device: any) {
    return this.http.put(`${this.apiUrl}/${id}`, device);
  }

  // 6. Check if Serial Number exists (For Async Validation)
  checkSerialNumberExists(serialNumber: string): Observable<boolean> {
    return this.http.get<boolean>(
      `${this.apiUrl}/check-serial?sn=${serialNumber}`,
    );
  }
}
