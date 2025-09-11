import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { UntilDestroy } from '@ngneat/until-destroy';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { AfisareHeaderModel } from 'src/app/models/common/UserHeader';
import { PipDisplayAddEditModel } from 'src/app/models/pip/PlanInbaunatirePerformante';
import { EmailPipService } from 'src/app/services/email/email-pip.service';
import { PipService } from 'src/app/services/pip/pip.service';

@UntilDestroy({checkProperties: true})
@Component({
    selector: 'pip-add',
    templateUrl: './pip-edit.component.html',
    standalone: false
})
export class PipAddComponent implements OnInit {
  pipDisplay: PipDisplayAddEditModel;
  headerDetails: AfisareHeaderModel;
  idAngajat: number;
  idFirmaLoggedInUser: number | null;
  pipSaveSub: Subscription;
  pipDetailsDisplaySub: Subscription;

  constructor(private emailPipService: EmailPipService,
    private pipService: PipService,
    private dialogRef: MatDialogRef<PipAddComponent>,
    private toastr: ToastrService,
    @Inject(MAT_DIALOG_DATA) data) {
    this.idAngajat = data.idAngajat;
    this.idFirmaLoggedInUser = JSON.parse(localStorage.getItem('user'))?.idFirma;
  }

  ngOnInit() {
    this.pipDetailsDisplaySub = this.pipService.createInitial(this.idAngajat).subscribe((data) => {
      this.pipDisplay = data;
    });
  }

  onSubmit() {
    this.pipDisplay.updateId = JSON.parse(localStorage.getItem('user')).matricola;
    this.pipSaveSub = this.pipService.addPip(this.pipDisplay).subscribe({
      next: () => {
        this.toastr.success("Datele au fost salvate!");
        this.emailPipService.sendEmailCalificatPipToHRandAngajat(this.idAngajat, this.idFirmaLoggedInUser);
        setTimeout(() => {
          window.location.reload();
          this.close();
        }, 1000);
      }, error: (err) => {
        this.toastr.error(err?.error);
      }
    });
  }

  close() {
    this.dialogRef.close();
  }

}
