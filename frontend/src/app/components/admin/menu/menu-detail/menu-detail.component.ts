import { MenuService } from './../../../../services/menu/menu.service';
import { CommonModule } from '@angular/common';
import { Component, CUSTOM_ELEMENTS_SCHEMA, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MenuResponse } from '@services/menu/menu.interface';
import { MaterialModule } from '@ui/material/material.module';
import moment from 'moment';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-menu-detail',
  standalone: true,
  imports: [CommonModule, MaterialModule],
  templateUrl: './menu-detail.component.html',
  styleUrl: './menu-detail.component.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class MenuDetailComponent {
  constructor(
    private messageService: MessageService,
    private menuService: MenuService,
    public dialogRef: MatDialogRef<MenuDetailComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  menu: MenuResponse = {
    id: 0,
    displayName: '',
    countRole: 0,
    createdAt: new Date(),
    total: 0,
    iconName: '',
    status: false,
    url: ''
  };


  ngOnInit(): void {
    this.getMenuById();
  }

  getMenuById(): void {
    this.menuService.getMenuById(this.data.id).subscribe({
      next: (response) => {
        this.messageService.add({ severity: 'success', summary: 'Success', detail: response.message });
        this.menu = response.data;
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: err });
      },
      complete: () => console.log("Lấy dữ liệu role theo id thành công")
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
