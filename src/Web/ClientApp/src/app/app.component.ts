import { Component, OnInit } from '@angular/core';
import { ErrorHandlingService } from './core/services/error-handling.service';
import { ToasterService } from './shared/services/toaster.service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  providers : [ToasterService,MessageService]
})
export class AppComponent implements OnInit {
  constructor(
    private errorHandlingService: ErrorHandlingService,
    private toasterService: ToasterService
  ) {}

  ngOnInit() {
    this.errorHandlingService.error$.subscribe((errorMessage) => {
      this.toasterService.showError('Error', errorMessage);
    });
  }
}
