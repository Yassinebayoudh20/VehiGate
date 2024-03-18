import { noWhiteSpaceValidator } from 'src/app/core/validators/white-space.validator';
import { FormGroup, ControlContainer, FormBuilder, Validators, FormControl, AbstractControl } from '@angular/forms';
import { Component, OnInit, Input, inject } from '@angular/core';

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

    // Check if displayInputs array is empty
    if (this.displayInputs.length === 0) {
      // If displayInputs is empty, add all form controls
      formGroupConfig['phoneNumber'] = [null, [Validators.required, noWhiteSpaceValidator()]];
      formGroupConfig['email'] = [null, [Validators.required, Validators.email, noWhiteSpaceValidator()]];
      formGroupConfig['contact'] = [null, [Validators.required, noWhiteSpaceValidator()]];
      formGroupConfig['address'] = [null, [Validators.required, noWhiteSpaceValidator()]];
    } else {
      // Loop through displayInputs and add controls accordingly
      this.displayInputs.forEach(inputName => {
        switch (inputName) {
          case 'phoneNumber':
            formGroupConfig[inputName] = [null, [Validators.required, noWhiteSpaceValidator()]];
            break;
          case 'email':
            formGroupConfig[inputName] = [null, [Validators.required, Validators.email, noWhiteSpaceValidator()]];
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

    // Add controls to the parentFormGroup based on displayInputs or all controls
    this.parentFormGroup.addControl(this.controlKey, this.formBuilder.group(formGroupConfig));

    this.form = this.parentContainer.control.get(this.controlKey);
  }

  shouldDisplay(inputName: string): boolean {
    return this.displayInputs.length === 0 || this.displayInputs.includes(inputName);
  }


  ngOnDestroy(){
    this.parentFormGroup.removeControl(this.controlKey)
  }
}
