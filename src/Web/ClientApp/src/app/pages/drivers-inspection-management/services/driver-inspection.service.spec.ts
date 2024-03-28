import { TestBed } from '@angular/core/testing';

import { DriverInspectionService } from './driver-inspection.service';

describe('DriverInspectionService', () => {
  let service: DriverInspectionService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DriverInspectionService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
