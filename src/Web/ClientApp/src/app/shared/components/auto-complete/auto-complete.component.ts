import { Observable } from 'rxjs';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { AutoCompleteCompleteEvent } from 'primeng/autocomplete';

@Component({
  selector: 'app-auto-complete',
  templateUrl: './auto-complete.component.html',
  styleUrls: ['./auto-complete.component.css'],
})
export class AutoCompleteComponent {
  @Input() items$: Observable<any[]> | undefined;
  @Input() filterBy: string = 'name';
  @Input() selected: string = null;
  @Input() isDisabled: boolean = false;
  @Output() selectedItemChange: EventEmitter<any> = new EventEmitter<any>();
  @Output() inputValueChange: EventEmitter<string> = new EventEmitter<string>();
  selectedItem: any;
  items: any[] = [];
  filteredItems: any[] | undefined;
  isTouchedAndDirty: boolean = false;

  ngOnInit() {
    this.items$.subscribe((items) => {
      this.items = items;
      if (this.selected) {
        this.selectedItem = this.items.find((i) => i[this.filterBy] === this.selected);
        if (!this.selectedItem) {
          const newValue = { [this.filterBy]: this.selected };
          this.items.push(newValue);
          this.selectedItem = newValue;
          this.isTouchedAndDirty = true;
        }
      }
    });
  }

  filterItems(event: AutoCompleteCompleteEvent) {
    let filtered: any[] = [];
    let query = event.query;

    for (let i = 0; i < this.items.length; i++) {
      let item = this.items[i];
      if (item[this.filterBy].toLowerCase().indexOf(query.toLowerCase()) === 0) {
        filtered.push(item);
      }
    }

    this.filteredItems = filtered;
    this.inputValueChange.emit(query);
    this.isTouchedAndDirty = true;
  }

  onItemSelect(event: any) {
    this.selectedItem = event.value;
    this.selectedItemChange.emit(this.selectedItem[this.filterBy]);
    this.isTouchedAndDirty = false;
  }

  validateSelection() {
    if (!this.selectedItem) {
      return false;
    }
    return true;
  }
}
