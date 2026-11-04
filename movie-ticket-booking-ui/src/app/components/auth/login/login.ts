import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { finalize } from 'rxjs';

import { AuthService } from '../../../services/auth.service';
import { LoginRequest } from '../../../models/auth';

@Component({
  selector: 'app-login',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {

  email = '';
  password = '';

  errorMessage = signal('');
  isLoading = signal(false);

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  onLogin() {

    this.errorMessage.set('');
    this.isLoading.set(true);

    const request: LoginRequest = {
      email: this.email,
      password: this.password
    };

    this.authService.login(request)
      .pipe(
        finalize(() => {
          this.isLoading.set(false);
        })
      )
      .subscribe({

        next: (response) => {

          localStorage.setItem('token', response.token);
          localStorage.setItem('userId', response.userId);
          localStorage.setItem('email', response.email);
          localStorage.setItem('roles', JSON.stringify(response.roles));

          if (response.roles.includes('Admin')) {
            this.router.navigate(['/admin']);
          } else {
            this.router.navigate(['/']);
          }
        },

        error: (error) => {

          console.error('Login error:', error);

          this.errorMessage.set(
            error.error?.detail || 'Invalid email or password.'
          );
        }
      });
  }
}
