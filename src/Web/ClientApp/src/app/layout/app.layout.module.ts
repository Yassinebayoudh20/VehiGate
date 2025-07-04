import { NgModule } from '@angular/core';
import { MenuModule } from 'primeng/menu';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { InputTextModule } from 'primeng/inputtext';
import { SidebarModule } from 'primeng/sidebar';
import { BadgeModule } from 'primeng/badge';
import { RadioButtonModule } from 'primeng/radiobutton';
import { InputSwitchModule } from 'primeng/inputswitch';
import { RippleModule } from 'primeng/ripple';
import { AppMenuComponent } from './menu/app.menu.component';
import { AppMenuitemComponent } from './menu/app.menuitem.component';
import { RouterModule } from '@angular/router';
import { AppTopBarComponent } from './topbar/app.topbar.component';
import { AppFooterComponent } from './footer/app.footer.component';
import { AppConfigModule } from './config/config.module';
import { AppSidebarComponent } from './sidebar/app.sidebar.component';
import { AppLayoutComponent } from './app.layout.component';
import { CommonModule } from '@angular/common';
import { TranslocoModule } from '@ngneat/transloco';
import { AvatarModule } from 'primeng/avatar';



@NgModule({
  declarations: [AppMenuitemComponent, AppTopBarComponent, AppFooterComponent, AppMenuComponent, AppSidebarComponent, AppLayoutComponent],
  imports: [
    FormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    InputTextModule,
    MenuModule,
    TranslocoModule,
    AvatarModule,
    SidebarModule,
    BadgeModule,
    RadioButtonModule,
    InputSwitchModule,
    RippleModule,
    RouterModule,
    AppConfigModule,
    CommonModule,
  ],
  exports: [AppLayoutComponent],
})
export class AppLayoutModule {}
