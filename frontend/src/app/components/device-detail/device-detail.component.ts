import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink, Router } from '@angular/router';
import { DeviceService } from '../../services/device.service';
import { Device } from '../../models/device.model';
import { AuthService } from '../../services/auth.service';
import { AiService } from '../../services/ai.service';
import { FormsModule } from '@angular/forms';

// ... imports ...
@Component({
  selector: 'app-device-detail',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './device-detail.component.html',
  styleUrl: './device-detail.component.scss',
})
export class DeviceDetailComponent implements OnInit {
  device: Device | undefined;
  aiGeneratedDescription: string = '';
  showAiPreview: boolean = false;
  isAiLoading: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private deviceService: DeviceService,
    public authService: AuthService,
    private aiService: AiService,
  ) {}

  generateAIContent() {
    if (!this.device) return;
    this.isAiLoading = true;

    this.aiService.generateDescription(this.device).subscribe({
      next: (res) => {
        this.aiGeneratedDescription = res.description;
        this.showAiPreview = true;
        this.isAiLoading = false;
      },
      error: () => (this.isAiLoading = false),
    });
  }

  // Just closes the purple box
  closeAiPreview() {
    this.showAiPreview = false;
    this.aiGeneratedDescription = '';
  }

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (id) {
      this.deviceService
        .getDeviceById(id)
        .subscribe((data) => (this.device = data));
    }
  }
}
