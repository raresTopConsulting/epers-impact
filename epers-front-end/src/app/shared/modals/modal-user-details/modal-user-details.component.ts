import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { UntilDestroy } from '@ngneat/until-destroy';
import { Subscription } from 'rxjs';
import { AfisareUserDetails } from 'src/app/models/common/UserDetails';
import { HeaderService } from 'src/app/services/common/header.service';

@UntilDestroy({checkProperties: true})
@Component({
    selector: 'modal-user-details',
    templateUrl: './modal-user-details.component.html',
    standalone: false
})
export class ModalUserDetailsComponent implements OnInit {
  userDetails: AfisareUserDetails;
  userSub: Subscription;

  constructor(public dialogRef: MatDialogRef<ModalUserDetailsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private headerService: HeaderService) { }

  ngOnInit() {
    this.dialogRef.updateSize('50%', '40%');
    this.userSub = this.headerService.getUserDetails(this.data.id).subscribe((data) => {
      this.userDetails = data;
    });
  }

  onClose() {
    this.dialogRef.close();
  }

}
