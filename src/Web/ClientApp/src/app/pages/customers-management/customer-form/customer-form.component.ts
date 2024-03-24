import { CUSTOMERS_LIST_PATH } from './../../../core/paths';
import { CustomerService } from './../services/customer.service';
import { CrudService } from 'src/app/shared/components/crud/crud.service';
import { CreateCustomerCommand, UpdateCustomerCommand } from './../../../web-api-client';
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
  isViewing: boolean = false;
  pageTitle: string;
  requestProcessing = false;

  constructor(
    private formBuilder: FormBuilder,
    private customerService: CustomerService,
    private crudService: CrudService,
    private transloco: TranslocoService,
    private router: Router,
    private aRoute: ActivatedRoute
  ) {}
  ngOnInit(): void {
    const customerId = this.aRoute.snapshot.params.id;

    this.aRoute.queryParams.subscribe((params) => {
      this.isEditing = params['action'] === FormState.EDITING ? true : false;
      this.isViewing = params['action'] === FormState.VIEWING ? true : false;
      this.resolvePageTitle();
      this.form = this.formBuilder.group({
        name: [null, [Validators.required, Validators.minLength(2), noWhiteSpaceValidator()]],
        distance: [null, [Validators.required]],
      });
      if (this.isEditing || this.isViewing) {
        this.fetchCustomerDetails(customerId);

      }
    });

  }

  disableForm() {
    if (this.isViewing) {
      this.form.disable();
      this.form.get('contactInfo').get('email').disable();
      this.form.get('contactInfo').get('phoneNumber').disable();
      this.form.get('contactInfo').get('contact').disable();

    }
  }

  resolvePageTitle() {
    this.pageTitle = this.isEditing ? 'EDIT_CUSTOMER' : 'ADD_NEW_CUSTOMER';
  }

  onSubmit(): void {
    if (this.form.invalid) {
      return;
    }

    this.requestProcessing = true;

    const command = this.isEditing ? this.createUpdateCustomerCommand() : this.CreateCustomerCommand();
    const customerServiceMethod = this.isEditing ? this.customerService.updateCustomer : this.customerService.createNewCustomer;
    const successMessage = this.isEditing ? this.transloco.translate('CUSTOMER_UPDATED_SUCCESSFULLY') : this.transloco.translate('CUSTOMER_ADDED_SUCCESSFULLY');

    const methodParams = this.isEditing ? [this.aRoute.snapshot.params.id, command] : [command];

    customerServiceMethod.apply(this.customerService, methodParams).subscribe({
      next: () => this.handleSuccess(successMessage),
      error: () => this.handleError(),
      complete: () => (this.requestProcessing = false),
    });
  }
  private CreateCustomerCommand(): CreateCustomerCommand {
    const customerCmd = new CreateCustomerCommand();
    customerCmd.name = this.form.get('name').value;
    customerCmd.email = this.form.get('contactInfo').get('email').value;
    customerCmd.phone = this.form.get('contactInfo').get('phoneNumber').value;
    customerCmd.contact = this.form.get('contactInfo').get('contact').value;
    customerCmd.distance = this.form.get('distance').value;
    return customerCmd;
  }
  fetchCustomerDetails(customerId: string) {
    this.customerService.getCustomerDetails(customerId).subscribe({
      next: (customerData) => {
        this.form.patchValue({
          name: customerData.name,
          distance: customerData.distance,
          contactInfo: {
            email: customerData.email,
            phoneNumber: customerData.phone,
            contact: customerData.contact,
          },
        });
        this.disableForm();
      },
    });
  }
  private createUpdateCustomerCommand(): UpdateCustomerCommand {
    const updateCmd = new UpdateCustomerCommand();
    updateCmd.id = this.aRoute.snapshot.params.id;

    if (this.form.get('name').dirty) {
      updateCmd.name = this.form.get('name').value;
    }
    if (this.form.get('contactInfo').get('email').dirty) {
      updateCmd.email = this.form.get('contactInfo').get('email').value;
    }
    if (this.form.get('contactInfo').get('phoneNumber').dirty) {
      updateCmd.phone = this.form.get('contactInfo').get('phoneNumber').value;
    }
    if (this.form.get('contactInfo').get('contact').dirty) {
      updateCmd.contact = this.form.get('contactInfo').get('contact').value;
    }
    if (this.form.get('distance').dirty) {
      updateCmd.distance = this.form.get('distance').value;
    }

    return updateCmd;
  }
  private handleSuccess(message: string) {
    this.crudService.setExecuteToaster({ isSuccess: true, message: message });
    this.router.navigate([CUSTOMERS_LIST_PATH]);
  }

  private handleError() {
    this.requestProcessing = false;
  }
  goBack() {
    this.router.navigate([CUSTOMERS_LIST_PATH]);
  }
}
