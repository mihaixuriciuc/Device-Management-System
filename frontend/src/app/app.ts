import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from './services/auth.service'; // ← added

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule], // ← added CommonModule for *ngIf
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App {
  constructor(public authService: AuthService) {} // ← public so template can access it

  logout(): void {
    this.authService.logout();
  }
}
