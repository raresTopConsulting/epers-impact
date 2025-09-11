import { Component, EventEmitter, Input, Output } from '@angular/core';
import { DropdownSelection } from 'src/app/models/common/Dorpdown';

@Component({
    selector: 'table-header',
    templateUrl: './table-header.component.html',
    standalone: false
})
export class TableHeaderComponent {
  @Output() add: EventEmitter<any> = new EventEmitter();
  @Output() nextPage: EventEmitter<number> = new EventEmitter<number>();
  @Output() previousPage: EventEmitter<number> = new EventEmitter<number>();
  @Output() lastPage: EventEmitter<number> = new EventEmitter<number>();
  @Output() selectItemsPerPage: EventEmitter<number> = new EventEmitter();
  @Output() serachFilter: EventEmitter<string> = new EventEmitter();
  @Output() serachIdDDFilter: EventEmitter<number> = new EventEmitter<number>();

  @Input() currentPage: number;
  @Input() pages: number;
  @Input() itemsPerPage: number;
  @Input() filter: string;
  @Input() displayAddButton: boolean = false;
  @Input() searchCriteria: string = "";
  @Input() ddFilterVisible: boolean = false;
  @Input() ddFilterCriteria: string;
  @Input() ddFilter: DropdownSelection;
  @Input() ddFilterList: DropdownSelection[];

  totalItemsInPage: number[] = [10, 15, 20, 25, 50];

  constructor() {
  }

  onAddNewItem() {
    this.add.emit();
  }

  onPreviousPage() {
    if (this.currentPage > 1) {
      this.previousPage.emit(this.currentPage - 1);
      this.currentPage--;
    }
  }

  onNextPage() {
    if (this.currentPage < this.pages) {
      this.nextPage.emit(this.currentPage + 1);
      this.currentPage++;
    }
  }

  onLastPage() {
    this.lastPage.emit(this.pages);
    this.currentPage = this.pages;
  }

  onSelectItems() {
    this.selectItemsPerPage.emit(this.itemsPerPage);
  }

  onFilter() {
    this.serachFilter.emit(this.filter);
  }

  onDDFilter(idDDFilter) {
    this.serachIdDDFilter.emit(+idDDFilter);
  }

  onClearFilter() {
    this.filter = '';
    this.serachFilter.emit(this.filter);
  }
}

