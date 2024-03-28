import {
  COMPANIES_LIST_PATH,
  COMPANY_UPSERT_FORM,
  CUSTOMER_UPSERT_FORM,
  CUSTOMERS_LIST_PATH,
  DRIVER_INSPECTION_UPSERT_FORM,
  DRIVER_INSPECTIONS_LIST_PATH,
  DRIVER_UPSERT_FORM,
  DRIVERS_LIST_PATH,
  SITE_UPSERT_FORM,
  SITES_LIST_PATH,
  VEHICLE_INSPECTION_UPSERT_FORM,
  VEHICLE_INSPECTIONS_LIST_PATH,
  VEHICLE_TYPE_UPSERT_FORM,
  VEHICLE_TYPES_LIST_PATH,
  VEHICLE_UPSERT_FORM,
  VEHICLES_LIST_PATH,
} from './../../core/paths';
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
        label: 'HOME',
        items: [{ label: 'DASHBOARD', icon: 'pi pi-fw pi-home', routerLink: ['/'] }],
      },
      {
        label: 'MANAGEMENT',
        items: [
          {
            label: 'USER_MANAGEMENT',
            icon: 'pi pi-fw pi-id-card',
            iconClass: 'text-blue-500',
            items: [
              { label: 'USERS', icon: 'pi pi-list', routerLink: [USERS_LIST_PATH] },
              { label: 'ADD_USER', icon: 'pi pi-plus', routerLink: [USER_UPSERT_FORM] },
            ],
          },
          {
            label: 'CUSTOMERS_MANAGEMENT',
            icon: 'pi pi-fw pi-users',
            iconClass: 'text-green-500',
            items: [
              { label: 'CUSTOMERS', icon: 'pi pi-list', routerLink: [CUSTOMERS_LIST_PATH] },
              { label: 'ADD_CUSTOMER', icon: 'pi pi-plus', routerLink: [CUSTOMER_UPSERT_FORM] },
            ],
          },
          {
            label: 'COMPANIES_MANAGEMENT',
            icon: 'pi pi-fw pi-ticket',
            iconClass: 'text-purple-500',
            items: [
              { label: 'COMPANIES', icon: 'pi pi-list', routerLink: [COMPANIES_LIST_PATH] },
              { label: 'ADD_COMPANY', icon: 'pi pi-plus', routerLink: [COMPANY_UPSERT_FORM] },
            ],
          },
          {
            label: 'SITES_MANAGEMENT',
            icon: 'pi pi-fw pi-map-marker',
            iconClass: 'text-red-500',
            items: [
              { label: 'SITES', icon: 'pi pi-list', routerLink: [SITES_LIST_PATH] },
              { label: 'ADD_SITE', icon: 'pi pi-plus', routerLink: [SITE_UPSERT_FORM] },
            ],
          },
          {
            label: 'DRIVERS_MANAGEMENT',
            icon: 'pi pi-fw pi-user',
            iconClass: 'text-yellow-900',
            items: [
              { label: 'DRIVERS', icon: 'pi pi-list', routerLink: [DRIVERS_LIST_PATH] },
              { label: 'ADD_DRIVER', icon: 'pi pi-plus', routerLink: [DRIVER_UPSERT_FORM] },
              { label: 'DRIVER_INSPECTIONS', icon: 'pi pi-list', routerLink: [DRIVER_INSPECTIONS_LIST_PATH] },
              { label: 'ADD_DRIVER_INSPECTION', icon: 'pi pi-plus', routerLink: [DRIVER_INSPECTION_UPSERT_FORM] },
            ],
          },
          {
            label: 'VEHICLES_MANAGEMENT',
            icon: 'pi pi-fw pi-car',
            iconClass: 'text-pink-500 ',
            items: [
              { label: 'VEHICLES', icon: 'pi pi-list', routerLink: [VEHICLES_LIST_PATH] },
              { label: 'ADD_VEHICLE', icon: 'pi pi-plus', routerLink: [VEHICLE_UPSERT_FORM] },
              { label: 'VEHICLE_TYPES', icon: 'pi pi-list', routerLink: [VEHICLE_TYPES_LIST_PATH] },
              { label: 'ADD_TYPE', icon: 'pi pi-plus', routerLink: [VEHICLE_TYPE_UPSERT_FORM] },
              { label: 'VEHICLE_INSPECTIONS', icon: 'pi pi-list', routerLink: [VEHICLE_INSPECTIONS_LIST_PATH] },
              { label: 'ADD_VEHICLE_INSPECTION', icon: 'pi pi-plus', routerLink: [VEHICLE_INSPECTION_UPSERT_FORM] },
            ],
          },
        ],
      },
    ];
  }
}
