import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { Router } from '@angular/router';

// This interface matches your .NET AuthResponseDto exactly!
export interface AuthResponse {
  isSuccess: boolean;
  message: string;
  role?: string;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = 'http://localhost:5246/api/account';
  private httpOptions = {
    withCredentials: true,
  };

  // BehaviorSubject is like a live radio station broadcasting the user's role
  private userRoleSubject = new BehaviorSubject<string | null>(
    localStorage.getItem('userRole'),
  );
  public userRole$ = this.userRoleSubject.asObservable();

  constructor(
    private http: HttpClient,
    private router: Router,
  ) {}

  login(credentials: any): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${this.apiUrl}/login`, credentials, {
        withCredentials: true, // 🍪 CRITICAL: Tells Angular to accept and send the HttpOnly cookies!
      })
      .pipe(
        tap((response) => {
          // If login works, save the role so the UI knows who is in charge
          if (response.isSuccess && response.role) {
            localStorage.setItem('userRole', response.role);
            this.userRoleSubject.next(response.role);
          }
        }),
      );
  }

  register(userData: any): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${this.apiUrl}/register`, userData, {
        withCredentials: true, // 🍪 CRITICAL: Accepts the cookies generated during registration
      })
      .pipe(
        tap((response) => {
          // Because your .NET API automatically logs them in after a successful register,
          // we save the role to instantly update the UI, just like the login method!
          if (response.isSuccess && response.role) {
            localStorage.setItem('userRole', response.role);
            this.userRoleSubject.next(response.role);
          }
        }),
      );
  }
  logout(): void {
    // 1. WIPE THE MEMORY IMMEDIATELY!
    // Do this first so the Guard instantly locks down the app.
    localStorage.removeItem('userRole');
    this.userRoleSubject.next(null);

    // 2. KICK THEM OUT IMMEDIATELY!
    this.router.navigate(['/login']);

    // 3. TELL THE SERVER TO KILL THE COOKIE IN THE BACKGROUND
    // We don't care if this takes a second, or even if it fails. The frontend is already locked.
    this.http.post(`${this.apiUrl}/logout`, {}, this.httpOptions).subscribe({
      next: () => console.log('Secure cookies destroyed on the server.'),
      error: (err) =>
        console.error(
          'Server logout failed, but frontend is still secure.',
          err,
        ),
    });
  }
  // --- Helper Methods for your UI Components ---

  isLoggedIn(): boolean {
    return !!this.userRoleSubject.value; // True if a role exists
  }

  isAdmin(): boolean {
    return this.userRoleSubject.value === 'Admin';
  }
}
