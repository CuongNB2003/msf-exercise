import { MatDialog } from '@angular/material/dialog';
import { MenuService } from './../../../../services/menu/menu.service';
import { Component, HostListener } from '@angular/core';
import { MenuResponse } from '@services/menu/menu.interface';
import moment from 'moment';
import { MessageService } from 'primeng/api';
import { MenuDetailComponent } from '../menu-detail/menu-detail.component';
import { MenuCreateUpdateComponent } from '../menu-create-update/menu-create-update.component';
import { CommonModule } from '@angular/common';
import { PaginationComponent } from '@ui/pagination/pagination.component';
import { MenuDeleteComponent } from '../menu-delete/menu-delete.component';

@Component({
  selector: 'app-menu-list',
  standalone: true,
  imports: [CommonModule, PaginationComponent],
  templateUrl: './menu-list.component.html',
  styleUrl: './menu-list.component.scss'
})
export class MenuListComponent {
  menu: MenuResponse[] = [];
  totalItems: number = 0;
  page: number = 1;
  limit: number = 10;
  currentPage: number = this.page;
  itemsPerPage: number = this.limit;
  isDropdownOpen: { [key: number]: boolean } = {};


  constructor(private messageService: MessageService, private menuService: MenuService, private dialog: MatDialog) { }

  ngOnInit(): void {
    this.loadRoles();
  }

  loadRoles(): void {
    this.menuService.getMenuAll(this.page, this.limit).subscribe({
      next: (response) => {
        // this.messageService.add({ severity: 'success', summary: 'Success', detail: response.message });
        this.menu = response.data.data;
        this.totalItems = response.data.totalRecords;
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: err });
      },
      complete: () => console.log("Lấy dữ liệu role thành công")
    });
  }

  formatDate(date: Date): string {
    var relative = moment(date).locale('vi').fromNow();
    var multiple = moment(date).locale('vi').format('Do MMM YYYY');
    return relative + ", " + multiple;
  }

  toggleDropdown(event: Event, roleId: number) {
    event.stopPropagation();
    this.isDropdownOpen[roleId] = !this.isDropdownOpen[roleId];
  }

  @HostListener('document:click', ['$event'])
  onClickOutside(event: Event) {
    const targetElement = event.target as HTMLElement;
    for (const roleId in this.isDropdownOpen) {
      if (this.isDropdownOpen.hasOwnProperty(roleId)) {
        if (!targetElement.closest('.dropdown') && !targetElement.closest('.dropdown-toggle')) {
          this.isDropdownOpen[roleId] = false;
        }
      }
    }
  }

  onPageChange(newPage: number): void {
    this.currentPage = newPage;
    this.page = newPage;
    this.loadRoles();
  }

  openDialogDetail(id: number): void {
    const dialogRef = this.dialog.open(MenuDetailComponent, {
      width: '600px',
      data: { id: id }
    });
  }

  openDialogCreate(): void {
    const dialogRef = this.dialog.open(MenuCreateUpdateComponent, {
      width: '600px',
      data: {
        id: null,
        load: () => this.loadRoles(),
      }
    });
  }

  openDialogUpdate(id: number): void {
    const dialogRef = this.dialog.open(MenuCreateUpdateComponent, {
      width: '600px',
      data: {
        id: id,
        load: () => this.loadRoles(),
      }
    });
  }

  openDialogDelete(id: number): void {
    const dialogRef = this.dialog.open(MenuDeleteComponent, {
      width: '600px',
      data: {
        id: id,
        load: () => this.loadRoles(),
      }
    });
  }
}
