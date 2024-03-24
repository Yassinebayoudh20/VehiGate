import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/core/services/auth.service';
import { LayoutService } from 'src/app/layout/service/app.layout.service';
import { LoginCommand } from 'src/app/web-api-client';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  providers: [AuthService],
  styles: [
    `
      :host ::ng-deep .pi-eye,
      :host ::ng-deep .pi-eye-slash {
        transform: scale(1.6);
        margin-right: 1rem;
        color: var(--primary-color) !important;
      }
    `,
  ],
})
export class LoginComponent implements OnInit, OnDestroy {
  valCheck: string[] = ['remember'];
  loginForm: FormGroup;
  loginSubscription: Subscription;
  requestProcessing = false;

  constructor(public layoutService: LayoutService, private authService: AuthService, private formBuilder: FormBuilder, private router: Router) {}

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      email: [null, Validators.required],
      password: [null, Validators.required],
    });
  }

  onSubmit(): void {
    if (this.loginForm.valid) {
      this.requestProcessing = true;
      const loginCmd = new LoginCommand();
      loginCmd.email = this.loginForm.get('email').value;
      loginCmd.password = this.loginForm.get('password').value;
      this.loginSubscription = this.authService.login(loginCmd).subscribe(
        (response) => {
          this.requestProcessing = false;
          this.navigateToDashboard();
        },
        () => (this.requestProcessing = false),
        () => (this.requestProcessing = false)
      );
    } else {
      this.requestProcessing = false;
    }
  }

  navigateToDashboard(): void {
    this.router.navigate(['/pages']);
  }

  ngOnDestroy(): void {
    if (this.loginSubscription) {
      this.loginSubscription.unsubscribe();
    }
  }
}
