import { PaginationParams } from 'src/app/shared/services/pagination-params.service';
import { CustomersClient, CreateCustomerCommand, UpdateCustomerCommand } from './../../../web-api-client';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {

  constructor(private customerClient: CustomersClient) { }
  getAllCustomers(params: PaginationParams) {
    return this.customerClient.getCustomers(params.pageNumber, params.pageSize, params.searchBy, params.orderBy, params.sortOrder);
  }
  createNewCustomer(creatCmd: CreateCustomerCommand) {
    return this.customerClient.createCustomer(creatCmd)
  }
  updateCustomer(CustomerId: string, updateCmd: UpdateCustomerCommand) {
    return this.customerClient.updateCustomer(CustomerId, updateCmd);
  }

  getCustomerDetails(CustomerId: string) {
    return this.customerClient.getCustomer(CustomerId);
  }
}
