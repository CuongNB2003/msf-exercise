import { CommonModule } from '@angular/common';
import { Component, CUSTOM_ELEMENTS_SCHEMA, Inject } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { RoleCreateUpdateComponent } from '@components/admin/role/role-create-update/role-create-update.component';
import { MenuCreateInput, MenuResponse, MenuUpdateInput } from '@services/menu/menu.interface';
import { MenuService } from '@services/menu/menu.service';
import { IconComponent } from '@ui/icon/icon.component';
import { InputComponent } from '@ui/input/input.component';
import { MaterialModule } from '../../../../modules/material/material.module';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-menu-create-update',
  standalone: true,
  imports: [
    MaterialModule,
    FormsModule,
    CommonModule,
    InputComponent,
    IconComponent,
    ReactiveFormsModule
  ],
  templateUrl: './menu-create-update.component.html',
  styleUrl: './menu-create-update.component.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class MenuCreateUpdateComponent {
  menuForm: FormGroup;
  isSubmitting: boolean = false;

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

  constructor(
    private messageService: MessageService,
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<RoleCreateUpdateComponent>,
    private menuService: MenuService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.menuForm = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(50)]],
      url: ['', [Validators.required, Validators.maxLength(50)]],
      icon: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    if (this.data.id) {
      this.loadDataRole(this.data.id);
    }
  }


  onIconSelected(icon: string): void {
    this.menuForm.get('icon')?.setValue(icon);
  }


  loadDataRole(id: number): void {
    this.menuService.getMenuById(id).subscribe({
      next: (response) => {
        this.menu = response.data;
        this.menuForm.patchValue({
          name: this.menu.displayName,
          url: this.menu.url,
          icon: this.menu.iconName
        });
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'Lỗi', detail: err });
      },
    });
  }

  onSubmit(): void {
    if (this.menuForm.invalid) {
      this.menuForm.markAllAsTouched();
      return;
    }

    if (this.data.id) {
      this.isSubmitting = true;
      this.updateHandle(this.data.id)
    } else {
      this.isSubmitting = true;
      this.createHandle()
    }
  }

  createHandle(): void {
    const input: MenuCreateInput = {
      displayName: this.menuForm.get('name')?.value,
      iconName: this.menuForm.get('icon')?.value,
      url: this.menuForm.get('url')?.value,
    };

    this.menuService.createMenu(input).subscribe({
      next: (response) => {
        this.messageService.add({ severity: 'success', summary: 'Thành công', detail: response.message });
        this.isSubmitting = false;
        this.close()
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'Lỗi', detail: err });
        this.isSubmitting = false;
      }
    });
  }

  updateHandle(id: number): void {
    const input: MenuUpdateInput = {
      displayName: this.menuForm.get('name')?.value, // Lấy giá trị từ form
      iconName: this.menuForm.get('icon')?.value, // Lấy giá trị từ form
      url: this.menuForm.get('url')?.value, // Lấy giá trị từ form
      status: true // Hoặc false, tùy thuộc vào logic của bạn
    };

    this.menuService.updateMenu(input, id).subscribe({
      next: (response) => {
        this.messageService.add({ severity: 'success', summary: 'Thành công', detail: response.message });
        this.isSubmitting = false;
        this.close()
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'Lỗi', detail: err });
        this.isSubmitting = false;
      }
    });
  }

  close(): void {
    this.data.load();
    this.dialogRef.close();
  }
}
