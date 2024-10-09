import { Component, CUSTOM_ELEMENTS_SCHEMA, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import moment from 'moment';
import { CommonModule } from '@angular/common';
import { Log } from '@services/log/log.interface';
import { LogService } from '@services/log/log.service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-log-detail',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './log-detail.component.html',
  styleUrl: './log-detail.component.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class LogDetailComponent implements OnInit {
  log: Log = {
    id: 0,
    method: '',
    statusCode: 0,
    url: '',
    clientIpAddress: '',
    userName: '',
    duration: 0,
    createdAt: new Date()
  };
  constructor(private messageService: MessageService, private logService: LogService, public dialogRef: MatDialogRef<LogDetailComponent>, @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit(): void {
    this.getLogById();
  }

  getLogById(): void {
    this.logService.getLogById(this.data.id).subscribe({
      next: (response) => {
        this.messageService.add({ severity: 'success', summary: 'Success', detail: response.message });
        this.log = response.data;
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: err });
      },
      complete: () => console.log("Lấy dữ liệu log theo id thành công")
    });
  }

  formatDate(date: Date): string {
    var time = moment(date).format('D/M/YYYY h:mm A');
    return time;
  }
  close(): void {
    this.dialogRef.close();
  }

}
