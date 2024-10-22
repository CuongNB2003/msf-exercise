import { Component, CUSTOM_ELEMENTS_SCHEMA, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { UserService } from '@services/user/user.service';
import { MaterialModule } from '../../../../modules/material/material.module';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-user-delete',
  standalone: true,
  imports: [
    MaterialModule,
  ],
  templateUrl: './user-delete.component.html',
  styleUrl: './user-delete.component.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class UserDeleteComponent {
  isSubmitting: boolean = false;

  constructor(
    private messageService: MessageService,
    public dialogRef: MatDialogRef<UserDeleteComponent>,
    public userService: UserService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  deleteHandle(): void {
    this.isSubmitting = true;
    this.userService.deleteUser(this.data.id).subscribe({
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

  close(): void {
    this.data.load();
    this.dialogRef.close();
  }
}
