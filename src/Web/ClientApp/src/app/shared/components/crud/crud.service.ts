import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { ToasterResponse } from '../models/toaster-response';

@Injectable({
  providedIn: 'root',
})
export class CrudService {
  public executeToaster: Subject<ToasterResponse> = new Subject<ToasterResponse>();
}
