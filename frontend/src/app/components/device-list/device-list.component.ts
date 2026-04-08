import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common'; // Needed for loops in HTML
import { DeviceService } from '../../services/device.service';
import { Device } from '../../models/device.model';
import { RouterLink, RouterModule, RouterOutlet } from '@angular/router';
import { Router } from '@angular/router';


@Component({
  selector: 'app-device-list',
  standalone: true,
  imports: [CommonModule,RouterLink], // Tells Angular "I want to use *ngFor"
  templateUrl: './device-list.component.html',
  styleUrl: './device-list.component.scss'
})
export class DeviceListComponent implements OnInit {
  devices: Device[] = []; // This is our "Storage Bin" for the data

  constructor(private deviceService: DeviceService,
    private router: Router ) {}

  ngOnInit(): void {
    // 1. Call the service
    this.deviceService.getDevices().subscribe({
      next: (data) => {
        this.devices = data; // 2. Put the data in the bin
      },
      error: (err) => console.error('API Error:', err)
    });
  }

  // 3. The function that handles the click
  goToDetails(id: number): void {
    this.router.navigate(['/devices', id]);
  }

deleteDevice(event: Event, id: number): void {
  event.stopPropagation(); // Prevents navigating to details
  
  if (confirm('Are you sure you want to delete this device?')) {
    this.deviceService.deleteDevice(id).subscribe({
      next: () => {
        // Success! Remove the device from the local list so the UI updates
        this.devices = this.devices.filter(d => d.id !== id);
        console.log(`Device ${id} deleted successfully`);
      },
      error: (err) => {
        console.error('Delete failed:', err);
        alert('Could not delete the device. Is the backend running?');
      }
    });
  }
}
}