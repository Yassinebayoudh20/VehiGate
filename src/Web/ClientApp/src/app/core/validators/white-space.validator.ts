import { AbstractControl, ValidatorFn } from '@angular/forms';

export function noWhiteSpaceValidator(): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } | null => {
    const hasWhiteSpace = control.value && (control.value).trim().length === 0;
    return hasWhiteSpace ? { whiteSpace: true } : null;
  };
}
