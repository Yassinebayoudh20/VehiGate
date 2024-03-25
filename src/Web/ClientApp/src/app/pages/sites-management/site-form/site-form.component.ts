import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TranslocoService } from '@ngneat/transloco';
import { CrudService } from 'src/app/shared/components/crud/crud.service';
import { noWhiteSpaceValidator } from 'src/app/core/validators/white-space.validator';
import { FormState } from 'src/app/core/data/models/form-state.enum';
import { CreateSiteCommand, UpdateSiteCommand } from 'src/app/web-api-client';
import { SITES_LIST_PATH } from 'src/app/core/paths';
import { SitesService } from '../services/sites.service';
import { ToasterResponse } from 'src/app/shared/components/models/toaster-response';
import { ToasterService } from 'src/app/shared/services/toaster.service';
import { take } from 'rxjs';

@Component({
  selector: 'app-site-form',
  templateUrl: './site-form.component.html',
  styleUrls: ['./site-form.component.css'],
})
export class SiteFormComponent implements OnInit {
  form: FormGroup;
  isEditing: boolean = false;
  isViewing: boolean = false;
  pageTitle: string;
  requestProcessing = false;


  constructor(
    private formBuilder: FormBuilder,
    private siteService: SitesService,
    private crudService: CrudService,
    private transloco: TranslocoService,
    private router: Router,
    private toasterService: ToasterService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    const siteId = this.route.snapshot.params.id;

    this.route.queryParams.subscribe((params) => {
      this.isEditing = params['action'] === FormState.EDITING ? true : false;
      this.isViewing = params['action'] === FormState.VIEWING ? true : false;
      this.resolvePageTitle();
      this.form = this.formBuilder.group({});
      if (this.isEditing || this.isViewing) {
        this.fetchSiteDetails(siteId);
      }
    });
  }
  disableForm() {
    if (this.isViewing) {
      this.form.disable();
      this.form.get('contactInfo').disable();
    }
  }
  resolvePageTitle() {
    if (this.isViewing) {
      this.pageTitle = 'VIEW_SITE_DETAILS';
    } else {
    this.pageTitle = this.isEditing ? 'EDIT_SITE' : 'ADD_NEW_SITE';
    }
  }

  onSubmit(): void {
    if (this.form.invalid) {
      return;
    }

    this.requestProcessing = true;

    const command = this.isEditing ? this.createUpdateSiteCommand() : this.createSiteCommand();
    const siteServiceMethod = this.isEditing ? this.siteService.updateSite : this.siteService.addNewSite;
    const successMessage = this.isEditing ? this.transloco.translate('SITE_UPDATED_SUCCESSFULLY') : this.transloco.translate('SITE_ADDED_SUCCESSFULLY');

    const methodParams = this.isEditing ? [this.route.snapshot.params.id, command] : [command];

    siteServiceMethod.apply(this.siteService, methodParams).subscribe({
      next: () => this.handleSuccess(successMessage),
      error: (error) => this.handleError(error),
      complete: () => (this.requestProcessing = false),
    });
  }

  private createSiteCommand(): CreateSiteCommand {
    const siteCmd = new CreateSiteCommand();
    siteCmd.address = this.form.get('contactInfo').get('address').value;
    siteCmd.contact = this.form.get('contactInfo').get('contact').value;
    siteCmd.phone = this.form.get('contactInfo').get('phoneNumber').value;
    siteCmd.email = this.form.get('contactInfo').get('email').value;
    return siteCmd;
  }

  fetchSiteDetails(siteId: string) {
    this.siteService.getSite(siteId).subscribe({
      next: (siteData) => {
        this.form.get('contactInfo').patchValue({
          address: siteData.address,
          contact: siteData.contact,
          phoneNumber: siteData.phone,
          email: siteData.email,
        });
        this.disableForm();
      },
    });
  }

  private createUpdateSiteCommand(): UpdateSiteCommand {
    const updateCmd = new UpdateSiteCommand();
    updateCmd.id = this.route.snapshot.params.id;

    if (this.form.get('contactInfo').get('address').dirty) {
      updateCmd.address = this.form.get('contactInfo').get('address').value;
    }
    if (this.form.get('contactInfo').get('contact').dirty) {
      updateCmd.contact = this.form.get('contactInfo').get('contact').value;
    }
    if (this.form.get('contactInfo').get('phoneNumber').dirty) {
      updateCmd.phone = this.form.get('contactInfo').get('phoneNumber').value;
    }
    if (this.form.get('contactInfo').get('email').dirty) {
      updateCmd.email = this.form.get('contactInfo').get('email').value;
    }

    return updateCmd;
  }

  private handleSuccess(message) {
    this.crudService.setExecuteToaster({ isSuccess: true, message: message });
    this.router.navigate([SITES_LIST_PATH]);
  }

  private handleError(error) {
    console.error(error);
    this.requestProcessing = false;
  }

  goBack() {
    this.router.navigate([SITES_LIST_PATH]);
  }
}
