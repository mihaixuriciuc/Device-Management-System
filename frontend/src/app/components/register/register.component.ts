import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  ReactiveFormsModule,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent {
  registerForm: FormGroup;
  errorMessage: string = '';
  isLoading: boolean = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
  ) {
    // This version looks for ANY character that is NOT a letter or a digit
    const passwordPattern =
      /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{6,}$/;
    // 1. Build the form to perfectly match your C# RegisterDto!
    this.registerForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      firstName: ['', [Validators.required, Validators.minLength(2)]],
      lastName: ['', [Validators.required, Validators.minLength(2)]],
      password: [
        '',
        [Validators.required, Validators.pattern(passwordPattern)],
      ], // <-- Add pattern here
    });
  }

  onSubmit() {
    // Stop if the form has errors
    if (this.registerForm.invalid) return;

    this.isLoading = true;
    this.errorMessage = '';

    // 2. Send the data to your .NET Backend via the Auth Service
    this.authService.register(this.registerForm.value).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          // 3. Success! The cookies are baked. Send them to the secure zone.
          this.router.navigate(['/devices']);
        } else {
          this.errorMessage = response.message || 'Registration failed.';
          this.isLoading = false;
        }
      },
      error: (err) => {
        // This catches 400 Bad Requests if the C# API rejects the data
        this.errorMessage =
          err.error?.message || 'A network error occurred. Is the API running?';
        this.isLoading = false;
      },
    });
  }
}
