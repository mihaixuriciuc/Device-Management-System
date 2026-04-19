import { Component, OnInit } from '@angular/core';
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
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss', // We can reuse the same styles
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  errorMessage: string = '';
  isLoading: boolean = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
    });
  }
  ngOnInit(): void {
    // THE BACK BUTTON TRAP:
    // If this page loads for ANY reason (like hitting the browser Back button),
    // instantly wipe the user's role from memory.
    localStorage.removeItem('userRole');

    // Note: If userRoleSubject is private, you might need to add a public
    // clearSession() method in your AuthService to reset it, or just let
    // the page refresh handle it.
  }

  onSubmit() {
    if (this.loginForm.invalid) return;

    this.isLoading = true;
    this.errorMessage = '';

    this.authService.login(this.loginForm.value).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          // Send them to the devices list upon success
          this.router.navigate(['/devices']);
        } else {
          this.errorMessage = response.message || 'Invalid email or password.';
          this.isLoading = false;
        }
      },
      error: (err) => {
        // If the backend returns 401, it lands here
        this.errorMessage = 'Invalid email or password.';
        this.isLoading = false;
      },
    });
  }
}
