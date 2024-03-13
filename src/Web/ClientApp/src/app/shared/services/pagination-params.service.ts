import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export interface PaginationParams {
  pageNumber?: number;
  pageSize?: number;
  searchBy?: string;
  orderBy?: string;
  sortOrder? : number;
  inRoles?: string;
}

@Injectable({
  providedIn: 'root',
})
export class PaginationParamsService {
  private paramsSource = new BehaviorSubject<PaginationParams>({ pageNumber: 1, pageSize: 10 });

  params$ = this.paramsSource.asObservable();

  updateParams(params: PaginationParams) {
    this.paramsSource.next(params);
  }
}
