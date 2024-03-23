import { Injectable } from '@angular/core';
import { Subject, take } from 'rxjs';
import { ToasterResponse } from '../models/toaster-response';

@Injectable({
  providedIn: 'root',
})
export class CrudService {
  public executeToaster$: Subject<ToasterResponse> = new Subject<ToasterResponse>();

  setExecuteToaster(toaster: ToasterResponse) {
    this.executeToaster$.next(toaster);
  }

  getExecuteToaster() {
    return this.executeToaster$.asObservable().pipe(take(1));
  }
}
