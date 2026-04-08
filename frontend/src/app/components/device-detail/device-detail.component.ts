import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink, RouterModule } from '@angular/router'; // 1. Need both for logic and 'back' buttons
import { DeviceService } from '../../services/device.service';
import { Device } from '../../models/device.model';

@Component({
  selector: 'app-device-detail',
  standalone: true,
  imports: [CommonModule, RouterLink], // 2. Must import these to use *ngIf and routerLink
  templateUrl: './device-detail.component.html',
  styleUrl: './device-detail.component.scss'
})
export class DeviceDetailComponent implements OnInit {
  device: Device | undefined;

  constructor(
    private route: ActivatedRoute, // This "sniffs" the URL for the ID
    private deviceService: DeviceService // This is our C# phone line
  ) {}

  ngOnInit(): void {
    // 3. Grab the 'id' from the URL path: /devices/:id
    const id = Number(this.route.snapshot.paramMap.get('id'));

    if (id) {
      this.deviceService.getDeviceById(id).subscribe({
        next: (data) => this.device = data,
        error: (err) => console.error('Error fetching device:', err)
      });
    }
  }
}