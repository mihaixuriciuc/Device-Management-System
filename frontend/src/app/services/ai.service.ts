import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Device } from '../models/device.model';
import { Observable } from 'rxjs';
import { API_CONFIG } from '../config/api.config';

@Injectable({ providedIn: 'root' })
export class AiService {
  private apiUrl = `${API_CONFIG.baseUrl}/ai`;

  constructor(private http: HttpClient) {}

  generateDescription(device: any): Observable<{ description: string }> {
    return this.http.post<{ description: string }>(
      `${this.apiUrl}/generate-description`,
      device,
      { withCredentials: true },
    );
  }
}
