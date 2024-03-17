import { COMPANIES_LIST_PATH, COMPANY_UPSERT_FORM, CUSTOMER_UPSERT_FORM } from './../../core/paths';
import { OnInit } from '@angular/core';
import { Component } from '@angular/core';
import { LayoutService } from '../service/app.layout.service';
import { USERS_LIST_PATH, USER_UPSERT_FORM } from 'src/app/core/paths';

@Component({
  selector: 'app-menu',
  templateUrl: './app.menu.component.html',
})
export class AppMenuComponent implements OnInit {
  model: any[] = [];

  constructor(public layoutService: LayoutService) {}

  ngOnInit() {
    this.model = [
      {
        label: 'Home',
        items: [{ label: 'Dashboard', icon: 'pi pi-fw pi-home', routerLink: ['/'] }],
      },
      {
        label: 'Management',
        items: [
          {
            label: 'User Management',
            icon: 'pi pi-fw pi-id-card',
            items: [
              { label: 'Users', icon: 'pi pi-list', routerLink: [USERS_LIST_PATH] },
              { label: 'Add User', icon: 'pi pi-plus', routerLink: [USER_UPSERT_FORM] },
            ],
          },
          {
            label: 'Customers Management',
            icon: 'pi pi-fw pi-id-card',
            items: [
              // { label: 'Customers', icon: 'pi pi-list', routerLink: [CUSTOMERS_LIST_PATH] },
              { label: 'Add Customer', icon: 'pi pi-plus', routerLink: [CUSTOMER_UPSERT_FORM] },
            ],
          },
          {
            label: 'Companies Management',
            icon: 'pi pi-fw pi-ticket',
            items: [
              { label: 'Companies', icon: 'pi pi-list', routerLink: [COMPANIES_LIST_PATH] },
              { label: 'Add Company', icon: 'pi pi-plus', routerLink: [COMPANY_UPSERT_FORM] },
            ],
          },
        ],
      },
    ];
  }
}
