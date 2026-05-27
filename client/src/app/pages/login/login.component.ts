import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  form: FormGroup;
  submitted = false;
  error: string | null = null;
  loading = false;

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.form = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  get f() {
    return this.form.controls;
  }

  onSubmit() {
    this.submitted = true;

    if (this.form.invalid) return;

    this.loading = true;
    this.authService.login(this.form.value).subscribe({
      next: (response) => {
        this.authService.setToken(response.token, response.userId, response.fullName);
        this.router.navigate(['/feed']);
      },
      error: (err) => {
        // Handle different error response formats
        if (err.error && typeof err.error === 'string') {
          this.error = err.error;
        } else if (err.error?.message) {
          this.error = err.error.message;
        } else if (err.error?.description) {
          this.error = err.error.description;
        } else if (err.error?.errors && Array.isArray(err.error.errors)) {
          this.error = err.error.errors.map((e: any) => e.description).join(', ');
        } else {
          this.error = err.statusText || 'Login failed';
        }
        this.loading = false;
      }
    });
  }

  navigateToRegister() {
    this.router.navigate(['/register']);
  }
}
