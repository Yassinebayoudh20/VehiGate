import { AbstractControl } from '@angular/forms';

export function emailValidator(control: AbstractControl): { [key: string]: boolean } | null {
  const emailRegex = /^[\w-]+(\.[\w-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*(\.[a-zA-Z]{2,})$/;

  return !emailRegex.test(control.value) ? { invalidEmailFormat: true } : null;
}
