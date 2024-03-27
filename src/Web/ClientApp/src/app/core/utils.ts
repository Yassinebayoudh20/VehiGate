import { FormGroup } from '@angular/forms';

export function setFormFieldAndMarkAsDirty(form: FormGroup, control: string, value: any) {
  form.get(control).setValue(value);
  form.get(control).markAsDirty();
  form.get(control).markAsTouched();
}

export function getDateOnly(date: Date) {
  const dateOnly = date.setHours(0, 0, 0, 0); // Set time to 00:00:00:00
  return dateOnly;
}

export function isDateBetween(dateToCheck: Date, fromDate: Date, toDate: Date): boolean {
  const dateOnly = getDateOnly(dateToCheck);
  const fromOnly = getDateOnly(fromDate);
  const toOnly = getDateOnly(toDate);

  return dateOnly >= fromOnly && dateOnly <= toOnly;
}
