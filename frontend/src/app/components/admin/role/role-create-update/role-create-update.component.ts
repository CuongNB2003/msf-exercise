import { PermissionService } from './../../../../services/permission/permission.service';
import { MenuService } from './../../../../services/menu/menu.service';
import { CommonModule } from '@angular/common';
import { Component, CUSTOM_ELEMENTS_SCHEMA, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MaterialModule } from '@ui/material/material.module';
import { InputComponent } from '@ui/input/input.component';
import { RoleInput, RoleResponse } from '@services/role/role.interface';
import { RoleService } from '@services/role/role.service';
import { MessageService } from 'primeng/api';
import { log } from 'console';
import { MenuResponse } from '@services/menu/menu.interface';
import { PermissionResponse } from '@services/permission/permission.interface';

@Component({
  selector: 'app-role-create-update',
  standalone: true,
  imports: [
    MaterialModule,
    FormsModule,
    CommonModule,
    InputComponent,
    ReactiveFormsModule
  ],
  templateUrl: './role-create-update.component.html',
  styleUrl: './role-create-update.component.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class RoleCreateUpdateComponent implements OnInit {
  createRoleForm: FormGroup;
  menus: MenuResponse[] = [];
  permissions: PermissionResponse[] = [];
  groupedPermissions: { [key: string]: PermissionResponse[] } = {};
  selectedGroup: string = '';

  selectedMenus: { [key: number]: boolean } = {};
  selectedPermission: { [key: number]: boolean } = {};
  isSubmitting: boolean = false;
  role: RoleResponse = {
    id: 0,
    name: '',
    countUser: 0,
    createdAt: new Date(),
    total: 0,
    menus: [],
    permissions: []
  };

  constructor(
    private messageService: MessageService,
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<RoleCreateUpdateComponent>,
    private roleService: RoleService,
    private menuService: MenuService,
    private permissionService: PermissionService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.createRoleForm = this.fb.group({
      name: ['', [Validators.required]]
    });
  }

  ngOnInit(): void {
    this.loadMenu();
    this.loadPermission();
    if (this.data.id) {
      this.loadDataRole(this.data.id);
    }
  }

  loadMenu(): void {
    this.menuService.getMenuAll(1, 999).subscribe({
      next: (response) => {
        this.menus = response.data.data;
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: err });
      },
    });
  }

  loadPermission(): void {
    this.permissionService.getPermissionAll(1, 999).subscribe({
      next: (response) => {
        this.permissions = response.data.data; // Lưu danh sách quyền

        // Nhóm quyền theo groupName
        this.groupedPermissions = this.groupPermissions(this.permissions);

        // Tự động chọn group đầu tiên
        const firstGroupKey = Object.keys(this.groupedPermissions)[0];
        if (firstGroupKey) {
          this.selectGroup(firstGroupKey); // Gán group đầu tiên làm selectedGroup
        }
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: err });
      },
    });
  }

  // Phương thức nhóm quyền
  groupPermissions(permissions: PermissionResponse[]): { [key: string]: PermissionResponse[] } {
    const grouped = permissions.reduce((groups, permission) => {
      const groupName = permission.permissionName.split('.')[0]; // Lấy phần trước dấu chấm
      if (!groups[groupName]) {
        groups[groupName] = []; // Tạo nhóm nếu chưa tồn tại
      }
      groups[groupName].push(permission); // Thêm quyền vào nhóm
      return groups;
    }, {} as { [key: string]: PermissionResponse[] });

    // Sắp xếp các nhóm theo thứ tự A-Z
    return Object.keys(grouped)
      .sort() // Sắp xếp tên các nhóm theo thứ tự bảng chữ cái
      .reduce((sortedGroups, key) => {
        sortedGroups[key] = grouped[key];
        return sortedGroups;
      }, {} as { [key: string]: PermissionResponse[] });
  }


  // Phương thức chọn group
  selectGroup(groupName: string): void {
    this.selectedGroup = groupName; // Cập nhật selectedGroup
  }


  loadDataRole(id: number): void {
    this.roleService.getRoleById(id).subscribe({
      next: (response) => {
        this.role = response.data;
        this.createRoleForm.patchValue({
          name: this.role.name
        });

        this.role.menus.forEach(menu => {
          this.selectedMenus[menu.id] = true;
        });

        this.role.permissions.forEach(permission => {
          this.selectedPermission[permission.id] = true;
        });
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: err });
      },
    });
  }

  onSubmit(): void {
    if (this.createRoleForm.invalid) {
      this.createRoleForm.markAllAsTouched();
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
    console.log(name);

    const input: RoleInput = {
      name: this.createRoleForm.get('name')?.value,
      description: "",
      menuIds: this.getSelectedMenuIds(),
      permissionIds: this.getSelectedPermissionIds(),

    };

    this.roleService.createRole(input).subscribe({
      next: (response) => {
        this.messageService.add({ severity: 'success', summary: 'Success', detail: response.message });
        this.isSubmitting = false;
        this.close()
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: err });
        this.isSubmitting = false;
      }
    });
  }

  updateHandle(id: number): void {
    const input: RoleInput = {
      name: this.createRoleForm.get('name')?.value,
      description: "",
      menuIds: this.getSelectedMenuIds(),
      permissionIds: this.getSelectedPermissionIds(),

    };

    this.roleService.updateRole(input, id).subscribe({
      next: (response) => {
        this.messageService.add({ severity: 'success', summary: 'Success', detail: response.message });
        this.isSubmitting = false;
        this.close()
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: err });
        this.isSubmitting = false;
      }
    });
  }

  onCheckboxPermissionChange(isChecked: boolean, selectedRole: PermissionResponse): void {
    this.selectedPermission[selectedRole.id] = isChecked;
    console.log("Selected Roles:", this.selectedPermission);
  }

  getSelectedPermissionIds(): number[] {
    const selectedIds = this.permissions.filter(
      permission => this.selectedPermission[permission.id]).map(permission => permission.id
      );
    console.log("Selected Role IDs:", selectedIds);
    return selectedIds;
  }

  onCheckboxMenuChange(isChecked: boolean, selectedRole: MenuResponse): void {
    this.selectedMenus[selectedRole.id] = isChecked;
    console.log("Selected Roles:", this.selectedMenus);
  }

  getSelectedMenuIds(): number[] {
    const selectedIds = this.menus.filter(menu => this.selectedMenus[menu.id]).map(menu => menu.id);
    console.log("Selected Role IDs:", selectedIds);
    return selectedIds;
  }

  close(): void {
    this.data.load();
    this.dialogRef.close();
  }
}
