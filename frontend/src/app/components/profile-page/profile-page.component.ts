import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { DeviceService } from '../../services/device.service';
import { Device } from '../../models/device.model';
import { CurrentUser } from '../../models/user.model';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './profile-page.component.html',
  styleUrl: './profile-page.component.scss',
})
export class ProfileComponent implements OnInit {
  currentUser: CurrentUser | null = null;
  assignedDevices: Device[] = [];
  isLoading = true;

  constructor(
    public authService: AuthService,
    private deviceService: DeviceService,
  ) {}

  ngOnInit(): void {
    this.authService.currentUser$.subscribe((user) => {
      this.currentUser = user;
    });

    this.loadMyDevices();
  }

  private loadMyDevices(): void {
    this.deviceService.getMyDevices().subscribe({
      next: (devices) => {
        this.assignedDevices = devices;
        this.isLoading = false;

        // Fallback: try to get name from first device if not available in currentUser
        if (
          !this.currentUser?.firstName &&
          devices.length > 0 &&
          devices[0].assignedUser
        ) {
          const u = devices[0].assignedUser;
          this.currentUser = {
            firstName: u.firstName,
            lastName: u.lastName,
            role: this.currentUser?.role,
          };
        }
      },
      error: () => (this.isLoading = false),
    });
  }

  unassignDevice(deviceId: number): void {
    if (confirm('Unassign this device?')) {
      this.deviceService.unassignDevice(deviceId).subscribe({
        next: () => this.loadMyDevices(),
        error: () => alert('Failed to unassign'),
      });
    }
  }

  logout(): void {
    this.authService.logout();
  }
}
