import { Component, Input, OnChanges, OnDestroy, OnInit, SimpleChanges, inject } from '@angular/core';
import { ControlContainer, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FORM_ENTRY_DATA_COLS } from 'src/app/core/constants';
import { dateRangeValidator } from 'src/app/core/validators/date-range.validator';

@Component({
  selector: 'app-data-entry-form',
  templateUrl: './data-entry-form.component.html',
  styleUrls: ['./data-entry-form.component.css'],
  viewProviders: [
    {
      provide: ControlContainer,
      useFactory: () => inject(ControlContainer, { skipSelf: true }),
    },
  ],
})
export class DataEntryFormComponent implements OnInit, OnChanges, OnDestroy {
  @Input() isAuthorized = false;
  @Input() isEditable = false;

  formData: FormGroup;
  cols = FORM_ENTRY_DATA_COLS;
  parentContainer = inject(ControlContainer);

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.formData = this.fb.group(
      {
        date: [{ value: new Date(), disabled: true }],
        isValidFrom: [{ value: new Date(), disabled: !this.isEditable }, Validators.required],
        validTo: [{ value: new Date(), disabled: !this.isEditable }, Validators.required],
        isAuthorized: [this.isAuthorized],
      },
      {
        validator: dateRangeValidator('isValidFrom', 'validTo'),
      }
    );

    this.parentFormGroup.addControl('dataEntry', this.formData);
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (this.formData && changes.isAuthorized) {
      this.formData.get('isAuthorized').setValue(this.isAuthorized);
    }
    if (this.formData && changes.isEditable) {
      if (this.isEditable) {
        this.formData.get('isValidFrom').enable();
        this.formData.get('validTo').enable();
      } else {
        this.formData.get('isValidFrom').disable();
        this.formData.get('validTo').disable();
      }
    }
  }

  get parentFormGroup() {
    return this.parentContainer.control as FormGroup;
  }

  ngOnDestroy(): void {
    this.parentFormGroup.removeControl('dataEntry');
  }
}
