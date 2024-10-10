import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import moment from 'moment';
import 'moment/locale/vi';
import { MatDialog } from '@angular/material/dialog';
import { PaginationComponent } from '@ui/pagination/pagination.component';
import { LogService } from '@services/log/log.service';
import { LogDetailComponent } from '../log-detail/log-detail.component';
import { Log } from '@services/log/log.interface';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-log-list',
  standalone: true,
  imports: [CommonModule, PaginationComponent],
  templateUrl: './log-list.component.html',
  styleUrl: './log-list.component.scss'
})
export class LogListComponent {
  logs: Log[] = [];
  totalItems: number = 0;
  page: number = 1;
  limit: number = 10;
  currentPage: number = this.page;
  itemsPerPage: number = this.limit;
  isDropdownOpen: { [key: number]: boolean } = {};

  constructor(private messageService: MessageService, private logService: LogService, private dialog: MatDialog) { }

  ngOnInit(): void {
    this.loadLogs();
  }

  loadLogs(): void {
    this.logService.getAll(this.page, this.limit).subscribe({
      next: (response) => {
        // this.messageService.add({ severity: 'success', summary: 'Success', detail: response.message });
        this.logs = response.data.data;
        this.totalItems = response.data.totalRecords;
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: err });
      },
      complete: () => console.log("Lấy dữ liệu log thành công")
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

  openDialog(id: number): void {
    const dialogRef = this.dialog.open(LogDetailComponent, {
      width: '600px',
      data: { id: id }
    });
  }
}