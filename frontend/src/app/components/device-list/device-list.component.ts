import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DeviceService } from '../../services/device.service';
import { Device } from '../../models/device.model';
import { RouterLink, RouterModule, Router } from '@angular/router';
import { AuthService } from '../../services/auth.service'; // 1. Import the Auth Service
import { FormsModule } from '@angular/forms';
@Component({
  selector: 'app-device-list',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterModule, FormsModule],
  templateUrl: './device-list.component.html',
  styleUrl: './device-list.component.scss',
})
export class DeviceListComponent implements OnInit {
  devices: Device[] = [];

  constructor(
    private deviceService: DeviceService,
    private router: Router,
    public authService: AuthService, // 2. Inject it as 'public' so HTML can read it
  ) {}

  ngOnInit(): void {
    // Call the helper function to load data
    this.loadDevices();
  }

  // Helper function to fetch devices so we can easily refresh the list later
  loadDevices(): void {
    this.deviceService.getDevices().subscribe({
      next: (data) => {
        this.devices = data;
      },
      error: (err) => console.error('API Error:', err),
    });
  }

  goToDetails(id: number): void {
    this.router.navigate(['/devices', id]);
  }

  // ==========================================
  // ADMIN ACTIONS
  // ==========================================

  deleteDevice(event: Event, id: number): void {
    event.stopPropagation();

    if (confirm('Are you sure you want to delete this device?')) {
      this.deviceService.deleteDevice(id).subscribe({
        next: () => {
          this.devices = this.devices.filter((d) => d.id !== id);
          console.log(`Device ${id} deleted successfully`);
        },
        error: (err) => {
          console.error('Delete failed:', err);
          alert('Could not delete the device. Is the backend running?');
        },
      });
    }
  }

  // ==========================================
  // REGULAR USER ACTIONS
  // ==========================================

  assignToMe(event: Event, id: number): void {
    event.stopPropagation();

    this.deviceService.assignDevice(id).subscribe({
      next: () => {
        console.log(`Device ${id} assigned successfully`);
        this.loadDevices(); // Refresh the list so the UI shows it as assigned!
      },
      error: (err) => {
        console.error('Assign failed:', err);
        alert('Could not assign the device.');
      },
    });
  }

  unassign(event: Event, id: number): void {
    event.stopPropagation();

    this.deviceService.unassignDevice(id).subscribe({
      next: () => {
        console.log(`Device ${id} unassigned successfully`);
        this.loadDevices(); // Refresh the list so the UI shows it as available!
      },
      error: (err) => {
        console.error('Unassign failed:', err);
        alert('Could not unassign the device.');
      },
    });
  }

  searchQuery: string = '';

  searchDevices(): void {
    if (!this.searchQuery.trim()) {
      this.loadDevices(); // reset to full list
      return;
    }

    this.deviceService.searchDevices(this.searchQuery).subscribe({
      next: (data) => (this.devices = data),
      error: (err) => console.error('Search failed:', err),
    });
  }
}
