import { FormGroup } from '@angular/forms';

export function setFormFieldAndMarkAsDirty(form: FormGroup, control: string, value: any) {
  form.get(control).setValue(value);
  form.get(control).markAsDirty();
  form.get(control).markAsTouched();
}
