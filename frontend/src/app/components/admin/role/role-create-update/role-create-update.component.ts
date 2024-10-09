import { CommonModule } from '@angular/common';
import { Component, CUSTOM_ELEMENTS_SCHEMA, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MaterialModule } from '@ui/material/material.module';
import { InputComponent } from '@ui/input/input.component';
import { InputRole, RoleResponse } from '@services/role/role.interface';
import { RoleService } from '@services/role/role.service';
import { MessageService } from 'primeng/api';
import { log } from 'console';

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
  // roles: RoleResponse[] = [];
  // selectedRoles: { [key: number]: boolean } = {};
  isSubmitting: boolean = false;
  role: RoleResponse = {
    id: 0,
    name: '',
    countUser: 0,
    createdAt: new Date()
  }

  constructor(
    private messageService: MessageService,
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<RoleCreateUpdateComponent>,
    public roleService: RoleService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.createRoleForm = this.fb.group({
      name: ['', [Validators.required]]
    });
  }

  ngOnInit(): void {
    // this.loadRoles();
    if (this.data.id) {
      this.loadDataRole(this.data.id);
    }
  }

  // loadRoles(): void {
  //   this.roleService.getAll(1, 999).subscribe({
  //     next: (response) => {
  //       this.roles = response.data.data;
  //     },
  //     error: (err) => {
  //       alert(`Không lấy được dữ liệu: ${err}`);
  //     },
  //     complete: () => console.log("Lấy dữ liệu role thành công")
  //   });
  // }

  loadDataRole(id: number): void {
    this.roleService.getRoleById(id).subscribe({
      next: (response) => {
        this.role = response.data;
        this.createRoleForm.patchValue({
          name: this.role.name
        });

        // this.selectedRoles[this.user.role.id] = true;
        // Đẩy dữ liệu vào hộp kiểm
        // this.user.role.forEach(role => {
        //   this.selectedRoles[role.id] = true;
        // });
      },
      error: (err) => {
        alert(`Không lấy được dữ liệu: ${err}`);
      },
      complete: () => console.log("Lấy dữ liệu người dùng theo id thành công")
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
    const name = this.createRoleForm.get('name')?.value;
    console.log(name);

    const input: InputRole = {
      name: name
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
    const name = this.createRoleForm.get('name')?.value;
    const input: InputRole = {
      name: name
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

  // onCheckboxChange(isChecked: boolean, selectedRole: RoleResponse): void {
  //   this.selectedRoles[selectedRole.id] = isChecked;
  //   console.log("Selected Roles:", this.selectedRoles);
  // }

  // getSelectedRoleIds(): number[] {
  //   const selectedIds = this.roles.filter(role => this.selectedRoles[role.id]).map(role => role.id);
  //   console.log("Selected Role IDs:", selectedIds);
  //   return selectedIds;
  // }

  close(): void {
    this.data.load();
    this.dialogRef.close();
  }
}
