import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Device } from '../models/device.model';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AiService {
  private apiUrl = 'http://localhost:5246/api/ai'; // Match your API port!

  constructor(private http: HttpClient) {}

  generateDescription(device: any): Observable<{ description: string }> {
    return this.http.post<{ description: string }>(
      `${this.apiUrl}/generate-description`,
      device,
      { withCredentials: true },
    );
  }
}
