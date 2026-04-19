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
    if (this.deviceForm.invalid || this.deviceForm.pending) {
      return;
    }

    const raw = this.deviceForm.value;

    // Fix RAM format for backend
    let ramValue = (raw.ramAmount || '').toString().trim();
    const match = ramValue.match(/\d+/);
    if (match) {
      ramValue = match[0] + 'GB';
    }

    const payload = {
      ...raw,
      ramAmount: ramValue,
    };

    this.deviceService.createDevice(payload).subscribe({
      next: () => {
        alert('Device created successfully!');
        this.router.navigate(['/devices']);
      },
      error: (err) => {
        console.error('Create failed:', err);
        if (err.status === 400 || err.status === 409) {
          alert(
            'Bad Request: ' +
              (err.error?.message ||
                'Check all fields (especially Serial Number)'),
          );
        } else {
          alert('Failed to create device. Is the server running?');
        }
      },
    });
  }

  uniqueSerialValidator(): AsyncValidatorFn {
    return (control: AbstractControl): Observable<ValidationErrors | null> => {
      if (!control.value) return of(null);

      return timer(300).pipe(
        switchMap(() =>
          this.deviceService.checkSerialNumberExists(control.value),
        ),
        map((exists) => (exists ? { serialTaken: true } : null)),
        catchError(() => of({ apiError: true })),
      );
    };
  }
}
