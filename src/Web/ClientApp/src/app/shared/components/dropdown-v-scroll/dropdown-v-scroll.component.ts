import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges, ViewChild, ElementRef } from '@angular/core';
import { Observable } from 'rxjs';
import { ItemsPaginationInfo } from './models/items-paginations-info';
import { TranslocoService } from '@ngneat/transloco';

@Component({
  selector: 'app-dropdown-v-scroll',
  templateUrl: './dropdown-v-scroll.component.html',
  styleUrls: ['./dropdown-v-scroll.component.css'],
})
export class DropdownVScrollComponent implements OnChanges {
  @Input() items$: Observable<any> | null = null;
  @Input() optionName: string = 'name';
  @Input() additionalInfo: string[] = [];
  @Input() isRequired: boolean = false;
  @Input() selected: string = null;
  @Output() selectedItemChange = new EventEmitter<any>();
  @Output() loadMoreDataEmitter = new EventEmitter<number>();
  @ViewChild('scrollContainer') scrollContainer: ElementRef;

  items: any[] = [];
  itemsPaginationOptions: ItemsPaginationInfo = null;
  selectedItem: any = null;

  loading: boolean = false;
  loadedPages: number[] = [];
  isOpen: boolean = false;
  isInvalid: boolean = false;
  errorMessage: string = '';

  constructor(private translate: TranslocoService) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.items$) {
      this.loadItems();
    }
  }

  private loadItems() {
    this.loading = true;
    if (this.items$) {
      this.items$.subscribe((items) => {
        this.items = [...this.items, ...items.data];
        this.itemsPaginationOptions = {
          hasNextPage: items.hasNextPage,
          hasPreviousPage: items.hasPreviousPage,
          pageNumber: items.pageNumber,
          totalCount: items.totalCount,
          totalPages: items.totalPages,
        };
        if (this.selected) {
          this.selectedItem = this.items.find((i) => i.id === this.selected);
          if (!this.selectedItem) this.loadMoreDataEmitter.emit(this.itemsPaginationOptions.pageNumber + 1);
        }
        this.loading = false;
      });
    } else {
      console.error('Items Observable is not provided.');
    }
  }

  onItemChange(item: any) {
    this.selectedItemChange.emit(item);
  }

  onScroll() {
    const element = this.scrollContainer.nativeElement;

    if (element.scrollTop + element.clientHeight >= element.scrollHeight) {
      this.onLazyLoad(null);
    }
  }

  onLazyLoad(event: any) {
    this.loading = true;

    if (this.itemsPaginationOptions.hasNextPage) {
      const nextPage = this.itemsPaginationOptions.pageNumber + 1;

      if (nextPage <= this.itemsPaginationOptions.totalPages && !this.loadedPages.includes(nextPage)) {
        this.loadedPages.push(nextPage);
        this.loadMoreDataEmitter.emit(nextPage);
      } else {
        this.loading = false;
      }
    } else {
      this.loading = false;
    }
  }

  toggleDropdown(event: Event) {
    event.stopPropagation();
    this.isOpen = !this.isOpen;
    this.validate();
  }

  selectItem($event: Event, item: any) {
    $event.stopPropagation();
    this.selectedItem = item;
    this.selectedItemChange.emit(item);
    this.isOpen = false;
    this.validate();
  }

  validate(): void {
    if (this.isRequired && !this.selectedItem) {
      this.isInvalid = true;
      this.errorMessage = this.translate.translate('THIS_FIELD_IS_REQUIRED');
    } else {
      this.isInvalid = false;
      this.errorMessage = '';
    }
  }
}
