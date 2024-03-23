import { AbstractControl } from '@angular/forms';

export function phoneNumberValidator(control: AbstractControl): { [key: string]: boolean } | null {
  const phoneRegex = /^\d+$/;
  return !phoneRegex.test(control.value) ? { phoneNumber: true } : null;
}
