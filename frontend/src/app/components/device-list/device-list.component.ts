import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common'; // Needed for loops in HTML
import { DeviceService } from '../../services/device.service';
import { Device } from '../../models/device.model';

@Component({
  selector: 'app-device-list',
  standalone: true,
  imports: [CommonModule], // Tells Angular "I want to use *ngFor"
  templateUrl: './device-list.component.html',
  styleUrl: './device-list.component.scss'
})
export class DeviceListComponent implements OnInit {
  devices: Device[] = []; // This is our "Storage Bin" for the data

  constructor(private deviceService: DeviceService) {}

  ngOnInit(): void {
    // 1. Call the service
    this.deviceService.getDevices().subscribe({
      next: (data) => {
        this.devices = data; // 2. Put the data in the bin
      },
      error: (err) => console.error('API Error:', err)
    });
  }
}