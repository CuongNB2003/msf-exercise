import { CommonModule } from '@angular/common';
import { Component, CUSTOM_ELEMENTS_SCHEMA, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MaterialModule } from '@ui/material/material.module';
import { InputComponent } from '@ui/input/input.component';
import { RoleResponse } from '@services/role/role.interface';
import { RoleService } from '@services/role/role.service';

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
  createUserFrom: FormGroup;
  roles: RoleResponse[] = [];
  selectedRoles: { [key: number]: boolean } = {};

  constructor(private fb: FormBuilder, public dialogRef: MatDialogRef<RoleCreateUpdateComponent>, public roleService: RoleService) {
    this.createUserFrom = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      name: ['', [Validators.required]]
    });
  }


  ngOnInit(): void {
    this.loadRoles();
  }

  loadRoles(): void {
    this.roleService.getAll(1, 999).subscribe({
      next: (response) => {
        this.roles = response.data.data;
      },
      error: (err) => {
        alert(`Không lấy được dữ liệu: ${err}`);
      },
      complete: () => console.log("Lấy dữ liệu role thành công")
    });
  }

  createHandle(): void {
    const selectedRoleIds: number[] = this.getSelectedRoleIds();
    console.log("Selected Role IDs:", selectedRoleIds);
  }

  onCheckboxChange(isChecked: boolean, selectedRole: RoleResponse): void {
    this.selectedRoles[selectedRole.id] = isChecked;
    console.log("Selected Roles:", this.selectedRoles); // Kiểm tra trạng thái của selectedRoles
  }

  getSelectedRoleIds(): number[] {
    const selectedIds = this.roles.filter(role => this.selectedRoles[role.id]).map(role => role.id);
    console.log("Selected Role IDs:", selectedIds); // Kiểm tra danh sách các ID đã chọn
    return selectedIds;
  }


  close(): void {
    this.dialogRef.close();
  }
}
