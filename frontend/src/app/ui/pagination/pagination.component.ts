import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-pagination',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.scss'] // Lưu ý sửa từ 'styleUrl' thành 'styleUrls'
})
export class PaginationComponent implements OnInit {
  @Input() totalItems: number = 0;
  @Input() itemsPerPage: number = 0;
  @Input() currentPage: number = 0;
  @Output() pageChanged = new EventEmitter<number>();

  totalPages: number = 0;
  pagesArray: number[] = [];

  ngOnInit(): void {
    this.calculatePages();
  }

  ngOnChanges(): void {
    this.calculatePages();
  }

  calculatePages(): void {
    this.totalPages = Math.ceil(this.totalItems / this.itemsPerPage);

    const maxPagesToShow = 3;
    const halfRange = Math.floor(maxPagesToShow / 2);
    let startPage = Math.max(this.currentPage - halfRange, 1);
    let endPage = Math.min(startPage + maxPagesToShow - 1, this.totalPages);

    if (endPage - startPage < maxPagesToShow - 1) {
      startPage = Math.max(endPage - maxPagesToShow + 1, 1);
    }

    this.pagesArray = Array.from({ length: endPage - startPage + 1 }, (_, i) => startPage + i);
  }

  goToFirstPage(): void {
    this.goToPage(1);
  }

  goToLastPage(): void {
    this.goToPage(this.totalPages);
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.pageChanged.emit(this.currentPage);
      this.calculatePages();
    }
  }


}
