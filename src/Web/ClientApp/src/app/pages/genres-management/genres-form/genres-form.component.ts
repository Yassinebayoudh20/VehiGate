import { CrudService } from 'src/app/shared/components/crud/crud.service';
import { noWhiteSpaceValidator } from 'src/app/core/validators/white-space.validator';
import { FormState } from 'src/app/core/data/models/form-state.enum';
import { TranslocoService } from '@ngneat/transloco';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { VehicleTypeService } from '../services/genres.service';
import { CreateVehicleTypeCommand, UpdateVehicleTypeCommand } from 'src/app/web-api-client';
import { VEHICLE_TYPES_LIST_PATH } from 'src/app/core/paths';

@Component({
  selector: 'app-genre-form',
  templateUrl: './genres-form.component.html',
  styleUrls: ['./genres-form.component.css'],
})
export class VehicleTypeFormComponent implements OnInit {
  form: FormGroup;
  isEditing: boolean = false;
  pageTitle: string;
  requestProcessing = false;

  constructor(
    private formBuilder: FormBuilder,
    private genreService: VehicleTypeService,
    private crudService: CrudService,
    private transloco: TranslocoService,
    private router: Router,
    private aRoute: ActivatedRoute
  ) {}

  ngOnInit(): void {
    const genreId = this.aRoute.snapshot.params.id;

    this.aRoute.queryParams.subscribe((params) => {
      this.isEditing = params['action'] === FormState.EDITING ? true : false;
      this.resolvePageTitle();
      this.form = this.formBuilder.group({
        name: [null, [Validators.required, Validators.minLength(2), noWhiteSpaceValidator()]],
      });
      if (this.isEditing) {
        this.fetchVehicleTypeDetails(genreId);
      }
    });
  }

  resolvePageTitle() {
    this.pageTitle = this.isEditing ? 'EDIT_GENRE' : 'ADD_NEW_GENRE';
  }

  onSubmit(): void {
    if (this.form.invalid) {
      return;
    }

    this.requestProcessing = true;

    const command = this.isEditing ? this.createUpdateVehicleTypeCommand() : this.createVehicleTypeCommand();
    const genreServiceMethod = this.isEditing ? this.genreService.updateVehicleType : this.genreService.createNewVehicleType;
    const successMessage = this.isEditing ? this.transloco.translate('GENRE_UPDATED_SUCCESSFULLY') : this.transloco.translate('GENRE_ADDED_SUCCESSFULLY');

    const methodParams = this.isEditing ? [this.aRoute.snapshot.params.id, command] : [command];

    genreServiceMethod.apply(this.genreService, methodParams).subscribe({
      next: () => this.handleSuccess(successMessage),
      error: () => this.handleError(),
      complete: () => (this.requestProcessing = false),
    });
  }

  private createVehicleTypeCommand(): CreateVehicleTypeCommand {
    const genreCmd = new CreateVehicleTypeCommand();
    genreCmd.name = this.form.get('name').value;
    return genreCmd;
  }

  fetchVehicleTypeDetails(genreId: string) {
    this.genreService.getVehicleTypeDetails(genreId).subscribe({
      next: (genreData) => {
        this.form.patchValue({
          name: genreData.name,
        });
      },
    });
  }

  private createUpdateVehicleTypeCommand(): UpdateVehicleTypeCommand {
    const updateCmd = new UpdateVehicleTypeCommand();
    updateCmd.id = this.aRoute.snapshot.params.id;

    if (this.form.get('name').dirty) {
      updateCmd.name = this.form.get('name').value;
    }

    return updateCmd;
  }

  private handleSuccess(message: string) {
    this.crudService.setExecuteToaster({ isSuccess: true, message: message });
    this.router.navigate([VEHICLE_TYPES_LIST_PATH]);
  }

  private handleError() {
    this.requestProcessing = false;
  }

  goBack() {
    this.router.navigate([VEHICLE_TYPES_LIST_PATH]);
  }
}
