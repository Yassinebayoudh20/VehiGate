// user-form.component.ts
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { TranslocoService } from '@ngneat/transloco';
import { Observable } from 'rxjs';
import { noWhiteSpaceValidator } from 'src/app/core/validators/white-space.validator';
import { UsersService } from '../services/users.service';

@Component({
  selector: 'app-user-form',
  templateUrl: './user-form.component.html',
})
export class UserFormComponent implements OnInit {
  form: FormGroup;
  isEditing: boolean = false;
  pageTitle: string;

  constructor(private formBuilder: FormBuilder, private transloco: TranslocoService, private router: Router, private userService: UsersService) {}

  ngOnInit() {
    this.initializeUserRolesList();
    this.resolvePageTitle();
    this.form = this.formBuilder.group({
      firstName: [null, [Validators.required, Validators.minLength(2), noWhiteSpaceValidator()]],
      lastName: [null, [Validators.required, Validators.minLength(2), noWhiteSpaceValidator()]],
      email: [null, [Validators.required, Validators.email, noWhiteSpaceValidator()]],
      password: [null, [Validators.required, Validators.minLength(6), noWhiteSpaceValidator()]],
      role: [null, [Validators.required]],
    });
  }

  resolvePageTitle() {
    this.pageTitle = !this.isEditing ? 'ADD_NEW_USER' : 'EDIT_USER';
  }

  initializeUserRolesList() {
    this.userService.getUserRoles().subscribe({
      next: (res) => {
        console.log(res);
      },
    });
  }

  onSubmit() {
    if (this.form.invalid) {
      // Handle invalid form submission (e.g., show error messages)
      return;
    }

    //! Add roles list to the dropdown
    //! Call the method to register new user

    console.log(this.form.value);

    // Handle valid form submission (e.g., save data to backend)
    // Example: Call a service to add or update user data
    // ...

    // After successful submission, navigate to a different page
    // Example: this.router.navigate(['/users']);
  }

  goBack() {
    // Implement navigation logic to go back to the previous page
    this.router.navigate(['pages/users']); // Replace '/previous-page' with the route of your previous page
  }
}
