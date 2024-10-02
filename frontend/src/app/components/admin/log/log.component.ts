import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core'
import { Log } from '../../../services/log/log.interface';
import { LogService } from '../../../services/log/log.service';
import moment from 'moment';
import 'moment/locale/vi';
@Component({
  selector: 'app-log',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './log.component.html',
  styleUrl: './log.component.scss'
})
export class LogComponent implements OnInit {
  logs: Log[] = [];
  totalRecords: number = 0;
  page: number = 1;
  limit: number = 100;

  constructor(private logService: LogService) { }

  ngOnInit(): void {
    this.loadLogs();
  }

  loadLogs(): void {
    this.logService.getAll(this.page, this.limit).subscribe({
      next: (response) => {
        this.logs = response.data.data;
        this.totalRecords = response.data.totalRecords;
      },
      error: (err) => {
        alert(`Không lấy được dữ liệu: ${err}`);
      },
      complete: () => console.log("Lấy dữ liệu thành công")
    });
  }


  formatDate(date: Date): string {
    var relative = moment(date).locale('vi').fromNow();
    var multiple = moment(date).locale('vi').format('Do MMM YYYY');
    return relative
  }
}
