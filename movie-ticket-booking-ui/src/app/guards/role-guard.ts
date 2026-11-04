import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';

export const roleGuard: CanActivateFn = (route, state) => {

  const router = inject(Router);

  const token = localStorage.getItem('token');
  const roles = JSON.parse(localStorage.getItem('roles') || '[]');

  // Check whether the user is logged in
  if (!token) {
    router.navigate(['/login']);
    return false;
  }

  // Get the required role from route configuration
  const requiredRole = route.data['role'];

  // Check whether the logged-in user has the required role
  if (requiredRole && roles.includes(requiredRole)) {
    return true;
  }

  // Redirect unauthorized users to Home
  router.navigate(['/']);

  return false;
};
