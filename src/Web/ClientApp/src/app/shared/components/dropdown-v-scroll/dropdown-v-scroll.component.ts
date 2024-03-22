import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges, ViewChild, ElementRef } from '@angular/core';
import { Observable } from 'rxjs';
import { ItemsPaginationInfo } from './models/items-paginations-info';
import { DEFAULT_PAGE_SIZE } from 'src/app/core/constants';

@Component({
  selector: 'app-dropdown-v-scroll',
  templateUrl: './dropdown-v-scroll.component.html',
  styleUrls: ['./dropdown-v-scroll.component.css'],
})
export class DropdownVScrollComponent implements OnInit, OnChanges {
  @Input() items$: Observable<any> | null = null;
  @Input() optionName: string = 'name';
  @Input() optionValue: string = 'id';
  @Output() selectedItemChange = new EventEmitter<any>();
  @Output() loadMoreDataEmitter = new EventEmitter<number>();

  items: any[] = [];
  itemsPaginationOptions: ItemsPaginationInfo = null;
  selectedItem: any = null;

  loading: boolean = false;
  rows: number = 10; // Number of items to display per page

  @ViewChild('scrollContainer') scrollContainer: ElementRef;

  private loadedPages: number[] = [];
  isOpen: boolean = false;

  constructor() {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.items$) {
      this.loadItems(); // Load items when the input changes
    }
  }

  ngOnInit() {
    // Initial load
    // this.loadItems();
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

      // Check if the nextPage is within the total number of pages
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
  }

  selectItem(item: any) {
    this.selectedItem = item;
    this.selectedItemChange.emit(item);
    this.isOpen = false;
  }
}
