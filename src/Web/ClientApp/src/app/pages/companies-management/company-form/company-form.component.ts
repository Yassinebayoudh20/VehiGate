import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslocoService } from '@ngneat/transloco';
import { COMPANIES_LIST_PATH } from 'src/app/core/paths';
import { noWhiteSpaceValidator } from 'src/app/core/validators/white-space.validator';
import { CrudService } from 'src/app/shared/components/crud/crud.service';
import { CreateCompanyCommand, UpdateCompanyCommand } from 'src/app/web-api-client';
import { CompaniesService } from '../services/companies.service';
import { FormState } from 'src/app/core/data/models/form-state.enum';

@Component({
  selector: 'app-company-form',
  templateUrl: './company-form.component.html',
  styleUrls: ['./company-form.component.css'],
})
export class CompanyFormComponent implements OnInit {
  form: FormGroup;
  isEditing: boolean = false;
  pageTitle: string;
  requestProcessing = false;

  constructor(
    private formBuilder: FormBuilder,
    private transloco: TranslocoService,
    private crudService: CrudService,
    private router: Router,
    private companyService: CompaniesService,
    private aRoute: ActivatedRoute
  ) {}

  ngOnInit(): void {
    const companyId = this.aRoute.snapshot.params.id;

    this.aRoute.queryParams.subscribe((params) => {
      this.isEditing = params['action'] === FormState.EDITING ? true : false;
      this.resolvePageTitle();
      this.form = this.formBuilder.group({
        name: [null, [Validators.required, noWhiteSpaceValidator()]],
      });
      if (this.isEditing) {
        this.fetchUserDetails(companyId);
      }
    });
  }

  fetchUserDetails(companyId: string) {
    this.companyService.getCompany(companyId).subscribe({
      next: (companyData) => {
        this.form.patchValue({
          name: companyData.name,
          contactInfo: {
            email: companyData.email,
            phoneNumber: companyData.phone,
            contact: companyData.contact,
            address: companyData.address,
          },
        });
      },
    });
  }

  resolvePageTitle() {
    this.pageTitle = this.isEditing ? 'EDIT_COMPANY' : 'ADD_NEW_COMPANY';
  }

  onSubmit(): void {
    if (this.form.invalid) {
      return;
    }

    this.requestProcessing = true;

    const command = this.isEditing ? this.createUpdateCompanyCommand() : this.createCompanyCommand();
    const userServiceMethod = this.isEditing ? this.companyService.updateCompany : this.companyService.addNewCompany;
    const successMessage = this.isEditing ? this.transloco.translate('COMPANY_UPDATED_SUCCESSFULLY') : this.transloco.translate('COMPANY_ADDED_SUCCESSFULLY');

    const methodParams = this.isEditing ? [this.aRoute.snapshot.params.id, command] : [command];

    userServiceMethod.apply(this.companyService, methodParams).subscribe({
      next: () => this.handleSuccess(successMessage),
      error: () => this.handleError(),
      complete: () => (this.requestProcessing = false),
    });
  }

  private createCompanyCommand(): CreateCompanyCommand {
    const companyCmd = new CreateCompanyCommand();
    companyCmd.name = this.form.get('name').value;
    companyCmd.email = this.form.get('contactInfo').get('email').value;
    companyCmd.address = this.form.get('contactInfo').get('address').value;
    companyCmd.contact = this.form.get('contactInfo').get('contact').value;
    companyCmd.phone = this.form.get('contactInfo').get('phoneNumber').value;
    return companyCmd;
  }
  private createUpdateCompanyCommand(): UpdateCompanyCommand {
    const updateCmd = new UpdateCompanyCommand();

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
    if (this.form.get('contactInfo').get('address').dirty) {
      updateCmd.address = this.form.get('contactInfo').get('address').value;
    }
    if (this.form.get('contactInfo').get('contact').dirty) {
      updateCmd.contact = this.form.get('contactInfo').get('contact').value;
    }
    return updateCmd;
  }

  private handleSuccess(message: string) {
    this.crudService.setExecuteToaster({ isSuccess: true, message: message });
    this.router.navigate([COMPANIES_LIST_PATH]);
  }

  private handleError() {
    this.requestProcessing = false;
  }

  goBack() {
    this.router.navigate([COMPANIES_LIST_PATH]);
  }
}
