import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  // If we have a role in memory, they are allowed in
  if (authService.isLoggedIn()) {
    return true;
  }

  // Otherwise, kick them to the login page
  router.navigate(['/login']);
  return false;
};
