import { Injectable } from '@angular/core';
import { PaginationParams } from 'src/app/shared/services/pagination-params.service';
import { SitesClient, CreateSiteCommand, UpdateSiteCommand } from 'src/app/web-api-client'; // Assuming the appropriate imports for sites

@Injectable({
  providedIn: 'root',
})
export class SitesService {
  constructor(private sitesClient: SitesClient) {}

  getAllSites(params: PaginationParams) {
    return this.sitesClient.getSites(params.pageNumber, params.pageSize, params.searchBy, params.orderBy, params.sortOrder);
  }

  getSite(siteId: string) {
    return this.sitesClient.getSiteById(siteId);
  }

  addNewSite(siteCmd: CreateSiteCommand) {
    return this.sitesClient.createSite(siteCmd);
  }

  updateSite(id: string, siteCmd: UpdateSiteCommand) {
    return this.sitesClient.updateSite(id, siteCmd);
  }

  deleteSite(id: string) {
    return this.sitesClient.deleteSite(id);
  }
}
