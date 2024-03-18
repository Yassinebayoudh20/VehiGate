import { Injectable } from '@angular/core';
import { PaginationParams } from 'src/app/shared/services/pagination-params.service';
import { AuthenticationClient, CompaniesClient, CreateCompanyCommand, RegisterCommand, UpdateCompanyCommand, UpdateUserInfoCommand } from 'src/app/web-api-client';

@Injectable({
  providedIn: 'root',
})
export class CompaniesService {
  constructor(private companiesClient: CompaniesClient) {}

  getAllCompanies(params: PaginationParams) {
    return this.companiesClient.getCompanies(params.pageNumber, params.pageSize, params.searchBy, params.orderBy, params.sortOrder);
  }

  getCompany(companyId : string){
    return this.companiesClient.getCompanyById(companyId);
  }

  addNewCompany(companyCmd: CreateCompanyCommand) {
    return this.companiesClient.createCompany(companyCmd);
  }

  updateCompany(id: string, companyCmd: UpdateCompanyCommand) {
    return this.companiesClient.updateCompany(id, companyCmd);
  }

  deleteCompany(id: string) {
    return this.companiesClient.deleteCompany(id);
  }
}
