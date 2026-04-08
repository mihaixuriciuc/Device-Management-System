import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  ReactiveFormsModule,
  FormGroup,
  FormControl,
  Validators,
  AbstractControl,
  AsyncValidatorFn,
  ValidationErrors,
} from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { DeviceService } from '../../services/device.service';
import { Observable, of, timer } from 'rxjs';
import { map, catchError, switchMap } from 'rxjs/operators';

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

    // Serial Number mapped with explicit validators
    serialNumber: new FormControl('', {
      validators: [Validators.required],
      asyncValidators: [this.uniqueSerialValidator()],
      updateOn: 'blur',
    }),

    manufacturer: new FormControl('', [Validators.required]),
    type: new FormControl('Phone', [Validators.required]),
    operatingSystem: new FormControl('', [Validators.required]),
    osVersion: new FormControl('', [Validators.required]),
    processor: new FormControl('', [Validators.required]),
    ramAmount: new FormControl('', [
      Validators.required,
      Validators.pattern(/^([1-9]|[1-9][0-9]|[1-4][0-9][0-9]|512)(GB|gb)?$/),
    ]),
    description: new FormControl('', [Validators.required]),
    status: new FormControl('Available', [Validators.required]),
  });

  constructor(
    private deviceService: DeviceService,
    private router: Router,
  ) {}

  onSubmit() {
    // Extra safety check: Don't submit if it's pending a server response
    if (this.deviceForm.valid && !this.deviceForm.pending) {
      const rawData = this.deviceForm.value;

      let ramValue = rawData.ramAmount?.toString().trim() || '';
      const numericPart = ramValue.match(/\d+/);
      if (numericPart) {
        ramValue = `${numericPart[0]}GB`;
      }

      const deviceToSave = {
        ...rawData,
        ramAmount: ramValue,
      };

      this.deviceService.createDevice(deviceToSave).subscribe({
        next: () => {
          alert('Device Created Successfully!');
          this.router.navigate(['/']);
        },
        error: (err) => {
          alert('Failed to save. Check console.');
          console.error('Error detail:', err);
        },
      });
    }
  }

  // ==========================================
  // Custom Async Validator
  // ==========================================
  uniqueSerialValidator(): AsyncValidatorFn {
    return (control: AbstractControl): Observable<ValidationErrors | null> => {
      if (!control.value) {
        return of(null);
      }

      return timer(300).pipe(
        switchMap(() =>
          this.deviceService.checkSerialNumberExists(control.value),
        ),
        map((exists) => {
          console.log(`Backend check for ${control.value}: Exists =`, exists);
          // Return the error state if it exists
          return exists ? { serialTaken: true } : null;
        }),
        catchError((err) => {
          console.error(
            'API validation failed (probably CORS or Server Offline):',
            err,
          );
          // CRITICAL FIX: If the API fails, return an error so the form becomes INVALID
          // and locks the submit button, instead of assuming it's fine.
          return of({ apiError: true });
        }),
      );
    };
  }
}
