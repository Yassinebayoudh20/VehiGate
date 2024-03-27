import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CheckListAssociation, CheckListsClient } from 'src/app/web-api-client';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ChecklistService {
  private readonly apiEndpoint = environment.API_ENDPOINT;

  constructor(private http: HttpClient, private checkListItemsClient: CheckListsClient) {}

  getCheckListItemsByAssociation(associatedTo: CheckListAssociation) {
    return this.checkListItemsClient.getCheckingItemsByAssociation(associatedTo);
  }
}
