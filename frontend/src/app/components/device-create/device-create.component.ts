import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  ReactiveFormsModule,
  FormGroup,
  FormControl,
  Validators,
} from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { DeviceService } from '../../services/device.service';

@Component({
  selector: 'app-device-create',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './device-create.component.html',
  styleUrl: './device-create.component.scss',
})
export class DeviceCreateComponent {
  deviceForm = new FormGroup({
    name: new FormControl('', [Validators.required]),
    serialNumber: new FormControl('', [Validators.required]),
    manufacturer: new FormControl('', [Validators.required]),
    type: new FormControl('Phone', [Validators.required]), // Added required
    operatingSystem: new FormControl('', [Validators.required]), // Added required
    osVersion: new FormControl('', [Validators.required]), // Added required
    processor: new FormControl('', [Validators.required]), // Added required
    ramAmount: new FormControl('', [
      Validators.required,
      Validators.pattern(/^([1-9]|[1-9][0-9]|[1-4][0-9][0-9]|512)(GB|gb)?$/),
    ]),
    description: new FormControl('', [Validators.required]), // Added required
    status: new FormControl('Available', [Validators.required]),
  });

  constructor(
    private deviceService: DeviceService,
    private router: Router,
  ) {}

  onSubmit() {
    if (this.deviceForm.valid) {
      const rawData = this.deviceForm.value;

      // 1. Clean the RAM value
      let ramValue = rawData.ramAmount?.toString().trim() || '';

      // Extract just the numbers to avoid double "GBGB"
      const numericPart = ramValue.match(/\d+/);
      if (numericPart) {
        ramValue = `${numericPart[0]}GB`;
      }

      // 2. Prepare the clean data object (NO FormControls here!)
      const deviceToSave = {
        ...rawData,
        ramAmount: ramValue, // Just the string!
      };

      console.log('Sending to backend:', deviceToSave);

      this.deviceService.createDevice(deviceToSave).subscribe({
        next: () => {
          alert('Device Created Successfully!');
          this.router.navigate(['/']);
        },
        error: (err) => {
          if (err.status === 400 || err.status === 409) {
            const message =
              err.error?.message || 'Check validation rules or Serial Number.';
            alert('Backend Error: ' + message);
          } else {
            alert('Server error. Check console.');
          }
          console.error('Error detail:', err);
        },
      });
    }
  }
}
