import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Device } from '../models/device.model';

@Injectable({
  providedIn: 'root',
})
export class DeviceService {
  // Pointing directly to your Devices controller
  private apiUrl = 'http://localhost:5246/api/devices';

  // CRITICAL: This tells Angular to send the secure Admin/User cookies with EVERY request
  private httpOptions = {
    withCredentials: true,
  };

  constructor(private http: HttpClient) {}

  // Get all devices
  getDevices(): Observable<Device[]> {
    return this.http.get<Device[]>(this.apiUrl, this.httpOptions);
  }

  getDeviceById(id: number): Observable<Device> {
    return this.http.get<Device>(`${this.apiUrl}/${id}`, this.httpOptions);
  }

  deleteDevice(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`, this.httpOptions);
  }

  createDevice(device: any): Observable<Device> {
    return this.http.post<Device>(this.apiUrl, device, this.httpOptions);
  }

  updateDevice(id: number, device: any) {
    return this.http.put(`${this.apiUrl}/${id}`, device, this.httpOptions);
  }

  checkSerialNumberExists(serialNumber: string): Observable<boolean> {
    return this.http.get<boolean>(
      `${this.apiUrl}/check-serial?sn=${serialNumber}`,
      this.httpOptions,
    );
  }

  // ==========================================
  // NEW ASSIGNMENT METHODS
  // ==========================================

  assignDevice(id: number): Observable<any> {
    // Assuming your .NET backend expects a PUT or POST to /api/devices/{id}/assign
    return this.http.post(`${this.apiUrl}/${id}/assign`, {}, this.httpOptions);
  }

  unassignDevice(id: number): Observable<any> {
    // Assuming your .NET backend expects a PUT or POST to /api/devices/{id}/unassign
    return this.http.post(
      `${this.apiUrl}/${id}/unassign`,
      {},
      this.httpOptions,
    );
  }
}
