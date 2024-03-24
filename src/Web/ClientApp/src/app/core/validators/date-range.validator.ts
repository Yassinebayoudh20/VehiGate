import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function dateRangeValidator(fromDateControlName: string, toDateControlName: string): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const fromDate = control.get(fromDateControlName)?.value;
    const toDate = control.get(toDateControlName)?.value;

    if (fromDate && toDate && new Date(fromDate) > new Date(toDate)) {
      return { dateRange: true };
    }

    return null;
  };
}
