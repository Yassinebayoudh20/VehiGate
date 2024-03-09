import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginRoutingModule } from './login-routing.module';
import { LoginComponent } from './login.component';
import { ButtonModule } from 'primeng/button';
import { CheckboxModule } from 'primeng/checkbox';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PasswordModule } from 'primeng/password';
import { InputTextModule } from 'primeng/inputtext';
import { TranslocoModule } from '@ngneat/transloco';
import { CoreModule } from 'src/app/core/core.module';

@NgModule({
    imports: [
        CommonModule,
        LoginRoutingModule,
        ButtonModule,
        CheckboxModule,
        InputTextModule,
        ReactiveFormsModule,
        PasswordModule,
        TranslocoModule,
        CoreModule
    ],
    declarations: [LoginComponent]
})
export class LoginModule { }
