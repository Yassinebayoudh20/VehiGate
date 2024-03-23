import { noWhiteSpaceValidator } from 'src/app/core/validators/white-space.validator';
import { FormGroup, ControlContainer, FormBuilder, Validators, FormControl, AbstractControl } from '@angular/forms';
import { Component, OnInit, Input, inject } from '@angular/core';
import { phoneNumberValidator } from 'src/app/core/validators/phone-number.validator';
import { emailValidator } from 'src/app/core/validators/email.validator';

@Component({
  selector: 'app-contact-info',
  templateUrl: './contact-info.component.html',
  styleUrls: ['./contact-info.component.css'],
  viewProviders: [
    {
      provide: ControlContainer,
      useFactory: () => inject(ControlContainer, { skipSelf: true }),
    },
  ],
})
export class ContactInfoComponent implements OnInit {
  @Input({ required: true }) controlKey = '';
  @Input() displayInputs: string[] = [];
  parentContainer = inject(ControlContainer);
  form: AbstractControl;

  get parentFormGroup() {
    return this.parentContainer.control as FormGroup;
  }
  constructor(private formBuilder: FormBuilder) {}

  ngOnInit(): void {
    const formGroupConfig = {};

    if (this.displayInputs.length === 0) {
      formGroupConfig['phoneNumber'] = [null, [Validators.required, phoneNumberValidator, noWhiteSpaceValidator()]];
      formGroupConfig['email'] = [null, [Validators.required, emailValidator, noWhiteSpaceValidator()]];
      formGroupConfig['contact'] = [null, [Validators.required, noWhiteSpaceValidator()]];
      formGroupConfig['address'] = [null, [Validators.required, noWhiteSpaceValidator()]];
    } else {
      this.displayInputs.forEach((inputName) => {
        switch (inputName) {
          case 'phoneNumber':
            formGroupConfig[inputName] = [null, [Validators.required, phoneNumberValidator, noWhiteSpaceValidator()]];
            break;
          case 'email':
            formGroupConfig[inputName] = [null, [Validators.required, emailValidator, noWhiteSpaceValidator()]];
            break;
          case 'contact':
            formGroupConfig[inputName] = [null, [Validators.required, noWhiteSpaceValidator()]];
            break;
          case 'address':
            formGroupConfig[inputName] = [null, [Validators.required, noWhiteSpaceValidator()]];
            break;
        }
      });
    }

    this.parentFormGroup.addControl(this.controlKey, this.formBuilder.group(formGroupConfig));

    this.form = this.parentContainer.control.get(this.controlKey);
  }

  shouldDisplay(inputName: string): boolean {
    return this.displayInputs.length === 0 || this.displayInputs.includes(inputName);
  }

  ngOnDestroy() {
    this.parentFormGroup.removeControl(this.controlKey);
  }
}
