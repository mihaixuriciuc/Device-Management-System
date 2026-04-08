import { Component, OnInit } from '@angular/core';
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
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { DeviceService } from '../../services/device.service';
import { Device } from '../../models/device.model';
import { Observable, of, timer } from 'rxjs';
import { map, catchError, switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-device-edit',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './device-edit.component.html',
  styleUrl: './device-edit.component.scss',
})
export class DeviceEditComponent implements OnInit {
  device: Device | undefined;
  deviceId!: number;

  // NEW: We need to remember the original serial number so we don't validate against it
  originalSerialNumber: string = '';

  deviceForm = new FormGroup({
    id: new FormControl<number | null>(null),
    name: new FormControl('', [Validators.required]),

    // Upgraded Serial Number Control (Identical to Create)
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
    private route: ActivatedRoute,
    private deviceService: DeviceService,
    private router: Router,
  ) {}

  ngOnInit(): void {
    this.deviceId = Number(this.route.snapshot.paramMap.get('id'));

    if (this.deviceId) {
      this.deviceService.getDeviceById(this.deviceId).subscribe({
        next: (data) => {
          this.device = data;
          // NEW: Save the original serial number before patching the form
          this.originalSerialNumber = data.serialNumber;
          this.deviceForm.patchValue(data);
        },
        error: (err) => console.error('Error fetching device:', err),
      });
    }
  }

  onSubmit() {
    // Upgraded Save guard: Must be valid AND not waiting on C#
    if (this.deviceForm.valid && !this.deviceForm.pending && this.deviceId) {
      const rawData = this.deviceForm.value;

      let ramValue = rawData.ramAmount?.toString().trim() || '';
      const numericPart = ramValue.match(/\d+/);
      if (numericPart) {
        ramValue = `${numericPart[0]}GB`;
      }

      const deviceToUpdate = {
        ...rawData,
        ramAmount: ramValue,
      };

      this.deviceService.updateDevice(this.deviceId, deviceToUpdate).subscribe({
        next: () => {
          alert('Device Updated Successfully!');
          this.router.navigate(['/devices', this.deviceId]);
        },
        error: (err) => {
          if (err.status === 400 || err.status === 409) {
            const message = err.error?.message || 'Check validation rules.';
            alert('Backend Error: ' + message);
          } else {
            alert('Server error. Check console.');
          }
          console.error('Error detail:', err);
        },
      });
    }
  }

  // ==========================================
  // Smart Async Validator for Editing
  // ==========================================
  uniqueSerialValidator(): AsyncValidatorFn {
    return (control: AbstractControl): Observable<ValidationErrors | null> => {
      if (!control.value) return of(null);

      // CRITICAL EDIT LOGIC: If the user hasn't changed the serial number from what
      // it originally was, we immediately say "It's Valid!" without asking the backend.
      if (
        this.originalSerialNumber &&
        control.value.toUpperCase() === this.originalSerialNumber.toUpperCase()
      ) {
        return of(null);
      }

      return timer(300).pipe(
        switchMap(() =>
          this.deviceService.checkSerialNumberExists(control.value),
        ),
        map((exists) => {
          return exists ? { serialTaken: true } : null;
        }),
        catchError(() => {
          return of({ apiError: true }); // Lock form if C# is unreachable
        }),
      );
    };
  }
}
