import { AfterViewInit, Component, OnInit } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { ChecklistService } from 'src/app/shared/services/checklist.service';
import { CheckListAssociation, CheckListItemDto, DriverDto, PagedResultOfDriverDto } from 'src/app/web-api-client';
import { DriverService } from '../../drivers-management/services/driver.service';
import { DEFAULT_PAGE_SIZE } from 'src/app/core/constants';
import { FormBuilder, FormGroup } from '@angular/forms';
import { getDateOnly, isDateBetween } from 'src/app/core/utils';

@Component({
  selector: 'app-driver-inspection-form',
  templateUrl: './driver-inspection-form.component.html',
  styleUrls: ['./driver-inspection-form.component.css'],
})
export class DriverInspectionFormComponent implements OnInit, AfterViewInit {
  checkListItems$: Observable<CheckListItemDto[]> = new Observable();
  driversList$: Observable<PagedResultOfDriverDto> = new Observable();
  selectedDriver: DriverDto = null;
  form: FormGroup;
  items: CheckListItemDto[];
  isInspectionAuthorized: boolean = false;

  constructor(private checkListService: ChecklistService, private driverService: DriverService, private fb: FormBuilder) {}

  ngAfterViewInit(): void {
    this.form.get('dataEntry').valueChanges.subscribe({ next: (form) => this.isAuthorized() });
  }

  ngOnInit(): void {
    this.loadCheckListItems();
    this.loadDrivers();
    this.form = this.fb.group({});
  }

  loadCheckListItems() {
    this.checkListItems$ = this.checkListService.getCheckListItemsByAssociation(CheckListAssociation.Driver).pipe(tap((result) => (this.items = result)));
  }

  loadDrivers(pageNumber: number = 1) {
    this.driversList$ = this.driverService.getAllDrivers({ pageNumber: pageNumber, pageSize: DEFAULT_PAGE_SIZE });
  }

  getSelectedDriver($event: DriverDto) {
    this.selectedDriver = $event;
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
    throw new Error('Method not implemented.');
  }
  onSubmit() {
    console.log(this.form.value);
    console.log(this.items);
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
}
