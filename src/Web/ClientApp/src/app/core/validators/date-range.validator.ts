import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
import { getDateOnly } from '../utils';

export function dateRangeValidator(fromDateControlName: string, toDateControlName: string): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const fromDate = control.get(fromDateControlName)?.value;
    const toDate = control.get(toDateControlName)?.value;

    if (fromDate && toDate) {
      const fromDateWithoutTime = getDateOnly(fromDate);
      const toDateWithoutTime = getDateOnly(toDate);

      if (fromDateWithoutTime > toDateWithoutTime) {
        return { dateRange: true };
      }
    }

    return null;
  };
}
