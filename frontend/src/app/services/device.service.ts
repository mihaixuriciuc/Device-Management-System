import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Device } from '../models/device.model';
import { API_CONFIG } from '../config/api.config';

@Injectable({
  providedIn: 'root',
})
export class DeviceService {
  // Pointing directly to your Devices controller
  private apiUrl = `${API_CONFIG.baseUrl}/devices`;

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

  // Get devices assigned to the current logged-in user
  getMyDevices(): Observable<Device[]> {
    return this.http.get<Device[]>(
      `${this.apiUrl}/my-devices`,
      this.httpOptions,
    );
  }

  searchDevices(query: string): Observable<Device[]> {
    return this.http.get<Device[]>(
      `${this.apiUrl}/search?q=${encodeURIComponent(query)}`,
      this.httpOptions,
    );
  }
}
