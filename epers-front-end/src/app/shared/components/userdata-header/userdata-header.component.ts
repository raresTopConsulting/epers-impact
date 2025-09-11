import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { Subscription } from 'rxjs';
import { AfisareHeaderModel } from 'src/app/models/common/UserHeader';
import { HeaderService } from 'src/app/services/common/header.service';
import { ModalUserDetailsComponent } from '../../modals/modal-user-details/modal-user-details.component';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { UntilDestroy } from '@ngneat/until-destroy';

@UntilDestroy({checkProperties: true})
@Component({
    selector: 'userdata-header',
    templateUrl: './userdata-header.component.html',
    standalone: false
})
export class UserdataHeaderComponent implements OnInit {
  @Input() idAngajat: number;
  headerData: AfisareHeaderModel;
  headerSub: Subscription;
  @ViewChild(ModalUserDetailsComponent) userDetailsModal: ModalUserDetailsComponent;

  constructor(private headerService: HeaderService,
    private dialog: MatDialog) {
    this.headerData = {} as AfisareHeaderModel;
  }

  ngOnInit() {
    this.headerSub = this.headerService.getHeaderData(this.idAngajat).subscribe((data) => {
      this.headerData = data;
    });
  }

  openUserDetailsDialog(id: number) {
    const dialogConfig = new MatDialogConfig();

    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = '60%';
    dialogConfig.height = '80%';

    dialogConfig.data = {
      id: id
    };
    
    this.dialog.open(ModalUserDetailsComponent, dialogConfig);
  }

}
