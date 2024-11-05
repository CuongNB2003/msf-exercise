import { CommonModule } from '@angular/common';
import { Component, CUSTOM_ELEMENTS_SCHEMA, Inject, input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { RoleResponse } from '@services/role/role.interface';
import { RoleService } from '@services/role/role.service';
import { InputCreateUser, InputUpdateUser, UserResponse } from '@services/user/user.interface';
import { UserService } from '@services/user/user.service';
import { InputComponent } from '@ui/input/input.component';
import { MaterialModule } from '../../../../modules/material/material.module';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-user-create-update',
  standalone: true,
  imports: [
    MaterialModule,
    FormsModule,
    CommonModule,
    InputComponent,
    ReactiveFormsModule,
  ],
  templateUrl: './user-create-update.component.html',
  styleUrls: ['./user-create-update.component.scss'],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class UserCreateUpdateComponent implements OnInit {
  createUserForm!: FormGroup;
  roles: RoleResponse[] = [];
  selectedRoles: { [key: number]: boolean } = {};
  isSubmitting: boolean = false;
  user: UserResponse = {
    id: 0,
    name: '',
    email: '',
    avatar: '',
    createdAt: new Date(),
    roles: [
      {
        id: 0,
        name: ''
      }
    ]
  };

  constructor(
    private messageService: MessageService,
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<UserCreateUpdateComponent>,
    public roleService: RoleService,
    private userService: UserService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  ngOnInit(): void {
    this.createForm();
    this.loadRoles();
    if (this.data.id) {
      this.loadDataUser(this.data.id);
    }
  }

  createForm(): void {
    if (this.data.id) {
      this.createUserForm = this.fb.group({
        email: ['', [Validators.required, Validators.email, Validators.maxLength(50)]],
        name: ['', [Validators.required, Validators.maxLength(50)]]
      });
    } else {
      this.createUserForm = this.fb.group({
        email: ['', [Validators.required, Validators.email, Validators.maxLength(50)]]
      });
    }
  }

  loadRoles(): void {
    this.roleService.getRoleAll(1, 999).subscribe({
      next: (response) => {
        this.roles = response.data.data;
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'Lỗi', detail: err });
      },
    });
  }

  loadDataUser(id: number): void {
    this.userService.getUserById(id).subscribe({
      next: (response) => {
        this.user = response.data;
        this.createUserForm.patchValue({
          email: this.user.email,
          name: this.user.name
        });

        // Đẩy dữ liệu vào hộp kiểm
        this.user.roles.forEach(role => {
          this.selectedRoles[role.id] = true;
        });
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'Lỗi', detail: err });
      },
    });
  }

  onSubmit(): void {
    if (this.createUserForm.invalid) {
      this.createUserForm.markAllAsTouched();
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
    const input: InputCreateUser = {
      email: this.createUserForm.get('email')?.value,
      avatar: "string",
      roleIds: this.getSelectedRoleIds()
    };
    this.userService.createUser(input).subscribe({
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
    const input: InputUpdateUser = {
      email: this.createUserForm.get('email')?.value,
      name: this.createUserForm.get('name')?.value,
      avatar: this.user.avatar,
      roleIds: this.getSelectedRoleIds()
    };

    this.userService.updateUser(input, id).subscribe({
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

  onCheckboxChange(isChecked: boolean, selectedRole: RoleResponse): void {
    this.selectedRoles[selectedRole.id] = isChecked;
  }

  getSelectedRoleIds(): number[] {
    const selectedIds = this.roles.filter(role => this.selectedRoles[role.id]).map(role => role.id);
    return selectedIds;
  }

  close(): void {
    this.data.load();
    this.dialogRef.close();
  }
}
