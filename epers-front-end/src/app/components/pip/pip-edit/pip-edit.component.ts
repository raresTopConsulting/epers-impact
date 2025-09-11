import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { UntilDestroy } from '@ngneat/until-destroy';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { AfisareHeaderModel } from 'src/app/models/common/UserHeader';
import { PipDisplayAddEditModel } from 'src/app/models/pip/PlanInbaunatirePerformante';
import { EmailPipService } from 'src/app/services/email/email-pip.service';
import { PipService } from 'src/app/services/pip/pip.service';

@UntilDestroy({ checkProperties: true })
@Component({
    selector: 'pip-edit',
    templateUrl: './pip-edit.component.html',
    standalone: false
})
export class PipEditComponent implements OnInit {
  pipDisplay: PipDisplayAddEditModel;
  headerDetails: AfisareHeaderModel;
  idAngajat: number;
  idFirmaLoggedInUser: number | null;

  pipSaveSub: Subscription;
  pipDetailsDisplaySub: Subscription;

  constructor(private emailPipService: EmailPipService,
    private pipService: PipService,
    private dialogRef: MatDialogRef<PipEditComponent>,
    private toastr: ToastrService,
    @Inject(MAT_DIALOG_DATA) data) {
    this.idAngajat = data.idAngajat;
    this.idFirmaLoggedInUser = JSON.parse(localStorage.getItem('user'))?.idFirma;
  }

  ngOnInit() {
    this.pipDetailsDisplaySub = this.pipService.getPipDisplay(this.idAngajat, null).subscribe((data) => {
      this.pipDisplay = data;
    });
  }

  onSubmit() {
    // aprobat HR si pip in desfasurare
    if (this.pipDisplay.idStare == 2) {
      if (this.pipDisplay.calificativFinalPip >= this.pipDisplay.calificativMinimPip) {
        // finalizat cu succes
        this.pipDisplay.idStare = 4;
      } else {
        // esuat
        this.pipDisplay.idStare = 5;
      }
    }

    // a fost respins de HR, deci trebuie modificat
    if (this.pipDisplay.idStare == 3) {
    }

    this.pipDisplay.updateId = JSON.parse(localStorage.getItem('user')).matricola;

    this.pipSaveSub = this.pipService.updatePip(this.pipDisplay).subscribe({
      next: () => {
        this.toastr.success("Datele au fost salvate!");
        if (this.pipDisplay.idStare === 4 || this.pipDisplay.idStare === 5) {
          this.emailPipService.sendEmaiPipIncheiat(this.idAngajat, this.idFirmaLoggedInUser);
        } else {
          this.emailPipService.sendEmaiPipStatusUpdate(this.idAngajat, this.idFirmaLoggedInUser);
        }
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
