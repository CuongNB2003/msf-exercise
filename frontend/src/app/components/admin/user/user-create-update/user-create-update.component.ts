import { CommonModule } from '@angular/common';
import { Component, CUSTOM_ELEMENTS_SCHEMA, Inject, input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { RoleResponse } from '@services/role/role.interface';
import { RoleService } from '@services/role/role.service';
import { InputCreateUser, InputUpdateUser, UserResponse } from '@services/user/user.interface';
import { UserService } from '@services/user/user.service';
import { InputComponent } from '@ui/input/input.component';
import { MaterialModule } from '@ui/material/material.module';

@Component({
  selector: 'app-user-create-update',
  standalone: true,
  imports: [
    MaterialModule,
    FormsModule,
    CommonModule,
    InputComponent,
    ReactiveFormsModule
  ],
  templateUrl: './user-create-update.component.html',
  styleUrls: ['./user-create-update.component.scss'],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class UserCreateUpdateComponent implements OnInit {
  createUserForm: FormGroup;
  roles: RoleResponse[] = [];
  selectedRoles: { [key: number]: boolean } = {};
  user: UserResponse = {
    id: 0,
    name: '',
    email: '',
    roleId: 0,
    avatar: '',
    createdAt: new Date(),
    role: {
      id: 0,
      name: ''
    }
  };

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<UserCreateUpdateComponent>,
    public roleService: RoleService,
    private userService: UserService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.createUserForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      name: ['', [Validators.required]]
    });
  }

  ngOnInit(): void {
    this.loadRoles();
    if (this.data.id) {
      this.loadDataUser(this.data.id);
    }
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

  loadDataUser(id: number): void {
    this.userService.getUserById(id).subscribe({
      next: (response) => {
        this.user = response.data;
        this.createUserForm.patchValue({
          email: this.user.email,
          name: this.user.name
        });

        this.selectedRoles[this.user.role.id] = true;
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
    if (this.data.id) {
      this.updateHandle(this.data.id)
    } else {
      this.createHandle()
    }
  }

  createHandle(): void {
    const selectedRoleIds: number[] = this.getSelectedRoleIds();
    if (selectedRoleIds.length === 0) {
      alert('Vui lòng chọn ít nhất một vai trò.');
      return;
    }
    const input: InputCreateUser = {
      email: this.createUserForm.get('email')?.value,
      roleId: selectedRoleIds[0],
      avatar: "string",
    };
    console.log(input.email);
    console.log(input.roleId);
    this.userService.createUser(input).subscribe({
      next: (response) => {
        this.close()
      },
      error: (err) => {
        alert(`Không thể thêm người dùng: ${err}`);
      }
    });
  }

  updateHandle(id: number): void {
    const selectedRoleIds: number[] = this.getSelectedRoleIds();
    if (selectedRoleIds.length === 0) {
      alert('Vui lòng chọn ít nhất một vai trò.');
      return;
    }

    const email = this.createUserForm.get('email')?.value;
    const name = this.createUserForm.get('name')?.value;

    if (!email || !name) {
      alert('Vui lòng điền đầy đủ thông tin.');
      return;
    }

    const input: InputUpdateUser = {
      email: email,
      name: name,
      roleId: selectedRoleIds[0], // Giả sử bạn chỉ chọn một vai trò
      avatar: this.user.avatar // Hoặc bạn có thể lấy từ form nếu có trường avatar
    };

    console.log(input.email);
    console.log(input.roleId);
    console.log(input.name);
    console.log(input.avatar);

    this.userService.updateUser(input, id).subscribe({
      next: (response) => {
        this.close()
      },
      error: (err) => {
        alert(`Không thể sửa người dùng: ${err}`);
      }
    });
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
    this.data.load();
    this.dialogRef.close();
  }
}
