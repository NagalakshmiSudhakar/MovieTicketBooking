import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { finalize } from 'rxjs';

import { AuthService } from '../../../services/auth.service';
import { RegisterRequest } from '../../../models/auth';

@Component({
  selector: 'app-register',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register {

  fullName = '';
  email = '';
  password = '';
  confirmPassword = '';

  errorMessage = signal('');
  successMessage = signal('');
  isLoading = signal(false);

  constructor(private authService: AuthService) { }

  onRegister() {

    this.errorMessage.set('');
    this.successMessage.set('');

    if (this.password !== this.confirmPassword) {
      this.errorMessage.set('Passwords do not match.');
      return;
    }

    if (this.password.length < 8) {
      this.errorMessage.set('Password must contain at least 8 characters.');
      return;
    }

    this.isLoading.set(true);

    const request: RegisterRequest = {
      fullName: this.fullName,
      email: this.email,
      password: this.password,
      confirmPassword: this.confirmPassword
    };

    this.authService.register(request)
      .pipe(
        finalize(() => this.isLoading.set(false))
      )
      .subscribe({

        next: (response) => {
          this.successMessage.set(response.message);
          this.fullName = '';
          this.email = '';
          this.password = '';
          this.confirmPassword = '';
        },

        error: (error) => {
          console.error('Registration error:', error);

          this.errorMessage.set(
            error.error?.detail ||
            error.error?.message ||
            'Registration failed. Please try again.'
          );
        }
      });
  }
}
