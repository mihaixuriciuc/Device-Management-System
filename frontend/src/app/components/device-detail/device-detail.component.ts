import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink, Router } from '@angular/router';
import { DeviceService } from '../../services/device.service';
import { Device } from '../../models/device.model';
import { AuthService } from '../../services/auth.service'; // 1. Import this

@Component({
  selector: 'app-device-detail',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './device-detail.component.html',
  styleUrl: './device-detail.component.scss',
})
export class DeviceDetailComponent implements OnInit {
  device: Device | undefined;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private deviceService: DeviceService,
    public authService: AuthService, // 2. Inject it as public
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    if (id) {
      this.deviceService.getDeviceById(id).subscribe({
        next: (data) => (this.device = data),
        error: (err) => console.error('Error fetching device:', err),
      });
    }
  }
}
