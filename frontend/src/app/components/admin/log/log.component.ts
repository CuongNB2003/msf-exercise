import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Log } from '../../../services/log/log.interface';
import { LogService } from '../../../services/log/log.service';
import moment from 'moment';
import 'moment/locale/vi';
import { PaginationComponent } from '../../../ui/pagination/pagination.component';

@Component({
  selector: 'app-log',
  standalone: true,
  imports: [CommonModule, PaginationComponent],
  templateUrl: './log.component.html',
  styleUrl: './log.component.scss'
})
export class LogComponent implements OnInit {
  logs: Log[] = [];
  totalItems: number = 0;
  page: number = 1;
  limit: number = 10;
  currentPage: number = this.page;
  itemsPerPage: number = this.limit;
  isDropdownOpen: { [key: number]: boolean } = {};

  constructor(private logService: LogService) { }

  ngOnInit(): void {
    this.loadLogs();
  }

  loadLogs(): void {
    this.logService.getAll(this.page, this.limit).subscribe({
      next: (response) => {
        this.logs = response.data.data;
        this.totalItems = response.data.totalRecords;
      },
      error: (err) => {
        alert(`Không lấy được dữ liệu: ${err}`);
      },
      complete: () => console.log("Lấy dữ liệu thành công")
    });
  }

  formatDate(date: Date): string {
    var relative = moment(date).locale('vi').fromNow();
    return relative;
  }

  onPageChange(newPage: number): void {
    this.currentPage = newPage;
    this.page = newPage;
    this.loadLogs();
  }
}
