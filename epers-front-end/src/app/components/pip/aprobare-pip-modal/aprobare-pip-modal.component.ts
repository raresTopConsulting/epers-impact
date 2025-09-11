import { AfterViewInit, Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { PipDisplayAddEditModel } from 'src/app/models/pip/PlanInbaunatirePerformante';
import { PipService } from 'src/app/services/pip/pip.service';
import { Router } from '@angular/router';
import { MatStepper } from '@angular/material/stepper';
import { UntilDestroy } from '@ngneat/until-destroy';
import { EmailPipService } from 'src/app/services/email/email-pip.service';

@UntilDestroy({checkProperties: true})
@Component({
    selector: 'aprobare-pip-modal',
    templateUrl: './aprobare-pip-modal.component.html',
    standalone: false
})
export class AprobarePipModalComponent implements OnInit, AfterViewInit {
  idAngajat: number;
  numePrenumeAngajat: string;
  observatii: string;
  isPipInCursDeAprobare: boolean = false;
  pipDisplay: PipDisplayAddEditModel;
  aprobarePipMessage: string;
  currentYear: number;
  errorMessage: string;
  currentStep: number = 0;

  private isStartPipSuccesfull: boolean = false;
  pipDetailsDisplaySub: Subscription;

  @ViewChild("stepper", { static: false }) private stepper: MatStepper;

  constructor(private emailPipService: EmailPipService,
    private pipService: PipService,
    private dialogRef: MatDialogRef<AprobarePipModalComponent>,
    private router: Router,
    @Inject(MAT_DIALOG_DATA) data) {

    this.idAngajat = data.idAngajat;
    this.numePrenumeAngajat = data.numePrenumeAngajat;
    this.currentYear = new Date().getFullYear();
  }

  ngOnInit(): void {
    this.isPipInCursDeAprobare = true;
    this.pipDetailsDisplaySub = this.pipService.getPipDisplay(this.idAngajat, this.currentYear).subscribe({
      next: (data) => {
        this.pipDisplay = data;
      }, error: (err) => {
        this.errorMessage = err.error;
      }
    });
  }

  ngAfterViewInit() {
    this.stepper.selectionChange.subscribe((res: any) => {
      this.currentStep = res?.selectedIndex;
    });
  }

  aprobarePipProccess() {
    this.aprobarePipMessage = "";
    this.pipDisplay.idStare = 2;
    this.pipDisplay.updateId = JSON.parse(localStorage.getItem('user')).matricola;

    this.pipService.updatePip(this.pipDisplay).subscribe({
      next: () => {
        this.aprobarePipMessage = "Planul de Îmbunătățire a Performanțelor a fost aprobat cu succes!";
        this.isPipInCursDeAprobare = false;
        this.emailPipService.sendEmailPipAprobatToAgajatAngManager(this.idAngajat);
        
      }, error: (err) => {
        this.aprobarePipMessage = err?.error;
      }
    });
  }

  respingerePipProcess() {
    this.aprobarePipMessage = "";
    this.pipDisplay.idStare = 3;
    this.pipDisplay.updateId = JSON.parse(localStorage.getItem('user')).matricola;
    this.pipService.updatePip(this.pipDisplay).subscribe({
      next: () => {
        this.aprobarePipMessage = `Planul de Îmbunătățire a Performanțelor a fost respins! Au fost salvate următoarele observații: ${this.pipDisplay.observatiiHr}`;
        this.isPipInCursDeAprobare = false;
        this.emailPipService.sendEmailPipRespinsToAngajatAndManager(this.idAngajat);
      }, error: (err) => {
        this.aprobarePipMessage = err?.error;
      }
    });
  }

  close() {
    if (!this.isStartPipSuccesfull) {
      this.dialogRef.close();
    } else {
      this.closeAndGoToPipList();
    }
  }

  private closeAndGoToPipList() {
    this.router.navigate(['../../pip/lista-subalterni-with-pip']);
    this.dialogRef.close();
  }

}
