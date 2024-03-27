import { Component, Input, OnInit } from '@angular/core';
import { DRIVER_INFO_COLS, VEHICLE_TANK_INFO_COLS } from 'src/app/core/constants';
import { CheckListAssociation } from 'src/app/web-api-client';

@Component({
  selector: 'app-entity-info',
  templateUrl: './entity-info.component.html',
  styleUrls: ['./entity-info.component.css'],
})
export class EntityInfoComponent implements OnInit {
  @Input() info: any;
  @Input() associatedTo: CheckListAssociation;
  cols = [];

  ngOnInit(): void {
    this.loadColumns();
  }

  loadColumns() {
    if (this.associatedTo === CheckListAssociation.Driver) {
      this.cols = DRIVER_INFO_COLS;
    } else {
      this.cols = VEHICLE_TANK_INFO_COLS;
    }
  }
}
