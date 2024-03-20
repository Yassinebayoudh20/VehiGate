import { COMPANIES_LIST_PATH, COMPANY_UPSERT_FORM, CUSTOMER_UPSERT_FORM, CUSTOMERS_LIST_PATH, SITE_UPSERT_FORM, SITES_LIST_PATH } from './../../core/paths';
import { COMPANIES_LIST_PATH, COMPANY_UPSERT_FORM, CUSTOMER_UPSERT_FORM, CUSTOMERS_LIST_PATH, DRIVER_UPSERT_FORM, DRIVERS_LIST_PATH } from './../../core/paths';
import { OnInit } from '@angular/core';
import { Component } from '@angular/core';
import { LayoutService } from '../service/app.layout.service';
import { USERS_LIST_PATH, USER_UPSERT_FORM } from 'src/app/core/paths';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-menu',
  templateUrl: './app.menu.component.html',
})
export class AppMenuComponent implements OnInit {
  model: MenuItem[] = [];

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
            iconClass : "text-blue-500",
            items: [
              { label: 'Users', icon: 'pi pi-list', routerLink: [USERS_LIST_PATH] },
              { label: 'Add User', icon: 'pi pi-plus', routerLink: [USER_UPSERT_FORM] },
            ],
          },
          {
            label: 'Customers Management',
            icon: 'pi pi-fw pi-users',
            iconClass: 'text-green-500',

            items: [
              { label: 'Customers', icon: 'pi pi-list', routerLink: [CUSTOMERS_LIST_PATH] },
              { label: 'Add Customer', icon: 'pi pi-plus', routerLink: [CUSTOMER_UPSERT_FORM] },
            ],
          },
          {
            label: 'Companies Management',
            icon: 'pi pi-fw pi-ticket',
            iconClass: 'text-purple-500',

            items: [
              { label: 'Companies', icon: 'pi pi-list', routerLink: [COMPANIES_LIST_PATH] },
              { label: 'Add Company', icon: 'pi pi-plus', routerLink: [COMPANY_UPSERT_FORM] },
            ],
          },
          {
            label: 'Sites Management',
            icon: 'pi pi-fw pi-map-marker',
            iconClass: 'text-red-500',
            items: [
              { label: 'Sites', icon: 'pi pi-list', routerLink: [SITES_LIST_PATH] },
              { label: 'Add Site', icon: 'pi pi-plus', routerLink: [SITE_UPSERT_FORM] },
            label: 'Drivers Management',
            icon: 'pi pi-fw pi-user',
            items: [
              { label: 'Drivers', icon: 'pi pi-list', routerLink: [DRIVERS_LIST_PATH] },
              { label: 'Add Driver', icon: 'pi pi-plus', routerLink: [DRIVER_UPSERT_FORM] },
            ],
          },
        ],
      },
    ];
  }
}
