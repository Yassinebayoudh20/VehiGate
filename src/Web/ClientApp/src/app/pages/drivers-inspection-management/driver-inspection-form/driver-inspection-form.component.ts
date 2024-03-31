import { AfterViewInit, Component, OnInit } from '@angular/core';
import { Observable, of, switchMap, tap } from 'rxjs';
import { ChecklistService } from 'src/app/shared/services/checklist.service';
import { CheckListAssociation, CheckListItemDto, CreateDriverInspectionCommand, DriverDto, DriverInspectionDto, PagedResultOfDriverDto, UpdateDriverInspectionCommand } from 'src/app/web-api-client';
import { DriverService } from '../../drivers-management/services/driver.service';
import { DEFAULT_PAGE_SIZE } from 'src/app/core/constants';
import { FormBuilder, FormGroup } from '@angular/forms';
import { dateToUtc, getDateOnly, isDateBetween } from 'src/app/core/utils';
import { DriverInspectionService } from '../services/driver-inspection.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CrudService } from 'src/app/shared/components/crud/crud.service';
import { DRIVER_INSPECTIONS_LIST_PATH } from 'src/app/core/paths';
import { FormState } from 'src/app/core/data/models/form-state.enum';
import { TranslocoService } from '@ngneat/transloco';

@Component({
  selector: 'app-driver-inspection-form',
  templateUrl: './driver-inspection-form.component.html',
  styleUrls: ['./driver-inspection-form.component.css'],
})
export class DriverInspectionFormComponent implements OnInit, AfterViewInit {
  checkListItems$: Observable<CheckListItemDto[]> = new Observable();
  driversList$: Observable<PagedResultOfDriverDto> = new Observable();
  selectedDriver: DriverDto = null;
  driverInspectionModel: DriverInspectionDto = null;
  form: FormGroup;
  items: CheckListItemDto[];
  isInspectionAuthorized: boolean = false;
  requestProcessing = false;
  isEditing: boolean = false;
  isViewing: boolean = false;
  pageTitle: string;

  constructor(
    private checkListService: ChecklistService,
    private aRoute: ActivatedRoute,
    private crudService: CrudService,
    private router: Router,
    private driverService: DriverService,
    private dis: DriverInspectionService,
    private fb: FormBuilder,
    private transloco: TranslocoService
  ) {}

  ngAfterViewInit(): void {
    this.form.get('dataEntry').valueChanges.subscribe({ next: (form) => this.isAuthorized() });
  }

  ngOnInit(): void {
    const driverInspectionId = this.aRoute.snapshot.params.id;
    this.aRoute.queryParams.subscribe((params) => {
      this.isEditing = params['action'] === FormState.EDITING ? true : false;
      this.isViewing = params['action'] === FormState.VIEWING ? true : false;
      this.resolvePageTitle();
      this.loadCheckListItems();
      this.loadDrivers();
      this.form = this.fb.group({
        driverId: [],
        notes: [],
      });
      if (this.isEditing || this.isViewing) {
        this.fetchDriverInspectionDetails(driverInspectionId);
      }
    });
  }
  disableForm() {
    if (this.isViewing) {
      this.form.disable();
    }
  }
  fetchDriverInspectionDetails(driverInspectionId: string) {
    this.dis
      .getDriverInspectionDetails(driverInspectionId)
      .pipe(
        switchMap((driverInspectionInfo) => {
          this.driverInspectionModel = driverInspectionInfo;
          if (this.driverInspectionModel.items) {
            this.checkListItems$ = of(this.driverInspectionModel.items).pipe(tap((items) => (this.items = items)));
          }
          return this.driverService.getDriverDetails(this.driverInspectionModel.driverId);
        })
      )
      .subscribe({
        next: (driverInfo: DriverDto) => {
          this.selectedDriver = driverInfo;
          const isValidFrom = new Date(this.driverInspectionModel.authorizedFrom);
          const validTo = new Date(this.driverInspectionModel.authorizedTo);
          this.form.get('notes').patchValue(this.driverInspectionModel.notes);
          this.form.get('driverId').patchValue(driverInfo.id);
          this.form.get('dataEntry').patchValue({
            isValidFrom: isValidFrom,
            validTo: validTo,
            isAuthorized: this.driverInspectionModel.isAuthorized,
          });
          this.isAuthorized();
          this.disableForm();
        },
      });
  }

  resolvePageTitle() {
    if (this.isViewing) {
      this.pageTitle = 'VIEW_DRIVER_INSPECTION_DETAILS';
    } else {
    this.pageTitle = this.isEditing ? 'EDIT_DRIVER_INSPECTION' : 'ADD_NEW_DRIVER_INSPECTION';}
  }

  loadCheckListItems() {
    this.checkListItems$ = this.checkListService.getCheckListItemsByAssociation(CheckListAssociation.Driver).pipe(tap((result) => (this.items = result)));
  }

  loadDrivers(pageNumber: number = 1) {
    this.driversList$ = this.driverService.getAllDrivers({ pageNumber: pageNumber, pageSize: DEFAULT_PAGE_SIZE });
  }

  getSelectedDriver($event: DriverDto) {
    this.selectedDriver = $event;
    this.form.get('driverId').setValue(this.selectedDriver.id);
  }

  onLoadMoreData($event) {
    this.loadDrivers($event);
  }

  get CheckListAssociation() {
    return CheckListAssociation;
  }

  onNoteChange($event: any) {
    const itemToUpdate = this.items.find((i) => i.id === $event.id);
    itemToUpdate.note = $event.newNote;
    this.isAuthorized();
  }

  onStateChange($event: any) {
    const itemToUpdate = this.items.find((i) => i.id === $event.id);
    itemToUpdate.state = $event.newState;
    this.isAuthorized();
  }

  goBack() {
    this.router.navigate([DRIVER_INSPECTIONS_LIST_PATH]);
  }

  onSubmit() {
    if (this.form.invalid) {
      return;
    }

    this.requestProcessing = true;

    this.requestProcessing = true;

    const command = this.isEditing ? this.UpdateDriverInspectionCommand() : this.createDriverInspectionCommand();
    const userServiceMethod = this.isEditing ? this.dis.updateDriverInspection : this.dis.createNewDriverInspection;
    const successMessage = this.isEditing ? this.transloco.translate('DRIVER_INSPECTION_UPDATED_SUCCESSFULLY') : this.transloco.translate('DRIVER_INSPECTION_ADDED_SUCCESSFULLY');

    const methodParams = this.isEditing ? [this.aRoute.snapshot.params.id, command] : [command];

    userServiceMethod.apply(this.dis, methodParams).subscribe({
      next: () => this.handleSuccess(successMessage),
      error: () => this.handleError(),
      complete: () => (this.requestProcessing = false),
    });
  }

  createDriverInspectionCommand() {
    const cmd = new CreateDriverInspectionCommand();
    cmd.driverId = this.selectedDriver.id;
    cmd.notes = this.form.get('notes').value;
    cmd.authorizedFrom = dateToUtc(this.form.get('dataEntry').get('isValidFrom').value);
    cmd.authorizedTo = dateToUtc(this.form.get('dataEntry').get('validTo').value);
    cmd.checkItems = this.items;
    return cmd;
  }

  UpdateDriverInspectionCommand() {
    const cmd = new UpdateDriverInspectionCommand();
    cmd.id = this.aRoute.snapshot.params.id;

    if (this.form.get('driverId').dirty) {
      cmd.driverId = this.form.get('driverId').value;
    }

    if (this.form.get('notes').dirty) {
      cmd.notes = this.form.get('notes').value;
    }

    if (this.form.get('dataEntry').get('isValidFrom').dirty) {
      cmd.authorizedFrom = dateToUtc(this.form.get('dataEntry').get('isValidFrom').value);
    }

    if (this.form.get('dataEntry').get('validTo').dirty) {
      cmd.authorizedTo = dateToUtc(this.form.get('dataEntry').get('validTo').value);
    }

    cmd.checkItems = this.items;

    return cmd;
  }

  isAuthorized() {
    const itemsCheckedToFalse = this.items.some((i) => !i.state);
    const validTo = this.form.get('dataEntry').get('validTo').value;
    const validFrom = this.form.get('dataEntry').get('isValidFrom').value;
    this.isInspectionAuthorized = !itemsCheckedToFalse && this.isDateValid() && isDateBetween(new Date(), validFrom, validTo);
  }

  isDateValid() {
    const validTo = this.form.get('dataEntry').get('validTo').value;
    const validFrom = this.form.get('dataEntry').get('isValidFrom').value;
    const isDateValid = getDateOnly(validFrom) <= getDateOnly(validTo);
    return isDateValid;
  }

  private handleSuccess(message: string) {
    this.crudService.setExecuteToaster({ isSuccess: true, message: message });
    this.router.navigate([DRIVER_INSPECTIONS_LIST_PATH]);
  }

  private handleError() {
    this.requestProcessing = false;
  }
}
