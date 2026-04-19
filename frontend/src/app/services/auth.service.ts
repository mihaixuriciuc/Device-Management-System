import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { Router } from '@angular/router';
import { API_CONFIG } from '../config/api.config';
import { AuthResponse, CurrentUser } from '../models/user.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = `${API_CONFIG.baseUrl}/account`;

  private httpOptions = { withCredentials: true };

  private userRoleSubject = new BehaviorSubject<string | null>(null);
  public userRole$ = this.userRoleSubject.asObservable();

  private currentUserSubject = new BehaviorSubject<CurrentUser | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(
    private http: HttpClient,
    private router: Router,
  ) {
    // Load from localStorage on startup
    const savedRole = localStorage.getItem('userRole');
    const savedUser = localStorage.getItem('currentUser');

    if (savedRole) this.userRoleSubject.next(savedRole);
    if (savedUser) this.currentUserSubject.next(JSON.parse(savedUser));
  }

  login(credentials: any): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${this.apiUrl}/login`, credentials, {
        withCredentials: true,
      })
      .pipe(
        tap((response) => {
          if (response.isSuccess && response.role) {
            localStorage.setItem('userRole', response.role);
            this.userRoleSubject.next(response.role);

            const user: CurrentUser = {
              firstName: response.firstName || 'User',
              lastName: response.lastName || '',
              role: response.role,
            };

            localStorage.setItem('currentUser', JSON.stringify(user));
            this.currentUserSubject.next(user);
          }
        }),
      );
  }

  register(userData: any): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${this.apiUrl}/register`, userData, {
        withCredentials: true,
      })
      .pipe(
        tap((response) => {
          if (response.isSuccess && response.role) {
            localStorage.setItem('userRole', response.role);
            this.userRoleSubject.next(response.role);

            const user: CurrentUser = {
              firstName: response.firstName || 'User',
              lastName: response.lastName || '',
              role: response.role,
            };

            localStorage.setItem('currentUser', JSON.stringify(user));
            this.currentUserSubject.next(user);
          }
        }),
      );
  }

  // Helper: Read AccessToken cookie
  private getAccessTokenFromCookie(): string | null {
    const cookies = document.cookie.split(';');
    for (let cookie of cookies) {
      const [name, value] = cookie.trim().split('=');
      if (name === 'AccessToken') return decodeURIComponent(value);
    }
    return null;
  }

  // Helper: Decode FirstName + LastName from JWT
  private decodeUserFromToken(token: string): CurrentUser {
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return {
        firstName: payload.FirstName || 'User',
        lastName: payload.LastName || '',
        role:
          payload[
            'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
          ] || 'User',
      };
    } catch {
      return { firstName: 'User', lastName: '', role: 'User' };
    }
  }

  logout(): void {
    localStorage.removeItem('userRole');
    localStorage.removeItem('currentUser');
    this.userRoleSubject.next(null);
    this.currentUserSubject.next(null);
    this.router.navigate(['/login']);

    this.http.post(`${this.apiUrl}/logout`, {}, this.httpOptions).subscribe();
  }

  isLoggedIn(): boolean {
    return !!this.userRoleSubject.value;
  }

  isAdmin(): boolean {
    return this.userRoleSubject.value === 'Admin';
  }
}
