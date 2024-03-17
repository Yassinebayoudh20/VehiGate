import { noWhiteSpaceValidator } from 'src/app/core/validators/white-space.validator';
import { FormState } from 'src/app/core/data/models/form-state.enum';
import { TranslocoService } from '@ngneat/transloco';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-customer-form',
  templateUrl: './customer-form.component.html',
  styleUrls: ['./customer-form.component.css'],
})
export class CustomerFormComponent implements OnInit {
  form: FormGroup;
  isEditing: boolean = false;
  pageTitle: string;

  constructor(private formBuilder: FormBuilder, private transloco: TranslocoService, private router: Router, private aRoute: ActivatedRoute) {}
  ngOnInit(): void {
    const customerId = this.aRoute.snapshot.params.id;

    this.aRoute.queryParams.subscribe((params) => {
      this.isEditing = params['action'] === FormState.EDITING ? true : false;
      this.resolvePageTitle();
      this.form = this.formBuilder.group({
        name: [null, [Validators.required, Validators.minLength(2), noWhiteSpaceValidator()]],
        distance: [null, [Validators.required]],
      });
      if (this.isEditing) {
        this.fetchCUstomerDetails(customerId);
      }
    });
  }

  resolvePageTitle() {
    this.pageTitle = this.isEditing ? 'EDIT_CUSTOMER' : 'ADD_NEW_CUSTOMER';
  }

  onSubmit(): void {
    //logic
  }
  fetchCUstomerDetails(customerId: string) {
    //logic
  }
  goBack() {
    // this.router.navigate([CUSTOMERS_LIST_PATH]);
  }
}
