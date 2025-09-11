import { Component, Inject, Input, OnInit, Output } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

import { DeleteModalData } from 'src/app/models/common/DeleteModalData';
import { UserStateService } from 'src/app/services/user/user-state.service';

@Component({
    selector: 'modal-delete',
    templateUrl: './modal-delete.component.html',
    standalone: false
})
export class ModalDeleteComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<ModalDeleteComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DeleteModalData,
    private userService: UserStateService) { }

  ngOnInit() {
  }

  onClose() {
    this.dialogRef.close();
  }

  onDelete() {
    this.userService.delete(this.data.entityId).subscribe(response => {
      if (response) {
      }
    });
  }

}
