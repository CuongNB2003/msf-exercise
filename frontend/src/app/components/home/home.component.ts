import { CommonModule } from '@angular/common';
import { Component, ElementRef, Renderer2 } from '@angular/core';
import { NgIconComponent, provideIcons } from '@ng-icons/core';
import {
  matAccessTimeOutline,
  matCleaningServicesOutline,
  matContactMailOutline,
  matCreateOutline,
  matDehazeOutline,
  matDeleteOutline,
  matDriveFileMoveOutline,
  matFolderOutline,
  matGroups2Outline,
  matInboxOutline,
  matInventory2Outline,
  matKeyboardArrowDownOutline,
  matLocalOfferOutline,
  matMailLockOutline,
  matMailOutlineOutline,
  matMarkEmailReadOutline,
  matMoreHorizOutline,
  matNoteOutline,
  matOutlinedFlagOutline,
  matPersonOffOutline,
  matPrintOutline,
  matPushPinOutline,
  matReplyAllOutline,
  matSendOutline,
  matShieldOutline,
  matUndoOutline,
  matVolumeOffOutline,
} from '@ng-icons/material-icons/outline';
import { NgScrollbarModule } from 'ngx-scrollbar';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [NgIconComponent, NgScrollbarModule, CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
  viewProviders: [
    provideIcons({
      matDehazeOutline,
      matKeyboardArrowDownOutline,
      // -----
      matMailOutlineOutline,
      matDeleteOutline,
      matInventory2Outline,
      matShieldOutline,
      matDriveFileMoveOutline,
      matReplyAllOutline,
      matMarkEmailReadOutline,
      matOutlinedFlagOutline,
      matGroups2Outline,
      matUndoOutline,
      matCleaningServicesOutline,
      matContactMailOutline,
      matLocalOfferOutline,
      matPushPinOutline,
      matAccessTimeOutline,
      matPrintOutline,
      matVolumeOffOutline,
      matPersonOffOutline,
      matMoreHorizOutline,
      matInboxOutline,
      matSendOutline,
      matCreateOutline,
      matMailLockOutline,
      matNoteOutline,
      matFolderOutline,
    }),
  ],
})
export class HomeComponent {
  isLayoutVisible = false;
  isClassic = true;

  constructor(private elementRef: ElementRef, private renderer: Renderer2) { }

  toggleDialogLayout(): void {
    this.isLayoutVisible = !this.isLayoutVisible;
  }

  switchToClassic() {
    this.isClassic = true;
  }

  switchToModern() {
    this.isClassic = false;
  }
}
