import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { TipPDF } from 'src/app/models/common/TipPDF';
import { Concluzie } from 'src/app/models/concluzii/Concluzie';
import { ConcluziiService } from 'src/app/services/concluzii/concluzii.service';
import { PipService } from 'src/app/services/pip/pip.service';
import { ModalPipDetailsComponent } from '../../pip/modal-pip-details/modal-pip-details.component';
import { EvaluareService } from 'src/app/services/evaluare/evaluare.service';
import { AfisareEvalCalificativFinal } from 'src/app/models/evaluare/AfisareEval';
import { PdfService } from 'src/app/services/common/pdf.service';
import { UntilDestroy } from '@ngneat/until-destroy';

@UntilDestroy({checkProperties: true})
@Component({
    selector: 'istoric-concluzii',
    templateUrl: './istoric-concluzii.component.html',
    standalone: false
})
export class IstoricConcluziiComponent implements OnInit {
  idAngajat: number;
  concluzie: Concluzie;
  years = Array.from({ length: 50 }, (_, i) => 2022 + i);
  yearFilter: number;
  concluziiPdf: TipPDF = TipPDF.CONCLUZII;
  userHasPip: boolean;
  istoricEvalCalificativ: AfisareEvalCalificativFinal;

  private pipServiceSub: Subscription;
  private concluziiSub: Subscription;
  private istoricEvalCalifFinSub: Subscription;
  private pdfServiceSub: Subscription;

  constructor(private route: ActivatedRoute,
    private pipService: PipService,
    private concluzieService: ConcluziiService,
    private dialog: MatDialog,
    private pdfService: PdfService,
    private evalService: EvaluareService) {

    this.concluzie = {} as Concluzie;
    this.route.params.subscribe(params => {
      this.idAngajat = +params['id'];
    });
    this.yearFilter = new Date().getFullYear();
  }

  ngOnInit() {
    this.searchConcluzie();
  }

  searchConcluzie() {
    this.concluziiSub = this.concluzieService.getIstoric(this.idAngajat, this.yearFilter).subscribe((data) => {
      this.concluzie = data;
    });
    this.checkIfHasPip(this.idAngajat, this.yearFilter);
    this.getIstoricEval(this.idAngajat, this.yearFilter);
  }

  openPipDetailsModal() {
    const dialogConfig = new MatDialogConfig();

    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = '60%';
    dialogConfig.height = '80%';

    dialogConfig.data = {
      idAngajat: this.idAngajat
    };

    this.dialog.open(ModalPipDetailsComponent, dialogConfig);
  }

  private getIstoricEval(idAngajat: number, anul: number | null) {
    this.istoricEvalCalifFinSub = this.evalService.getIstoricEvalCalificativFinal(this.idAngajat, anul).subscribe((data) => {
      if (data) {
        this.istoricEvalCalificativ = data;
      }
    });
  }

  private checkIfHasPip(idAngajat: number, anul: number | null) {
    this.pipServiceSub = this.pipService.getStareActualaPip(idAngajat, anul).subscribe((data) => {
      this.userHasPip = !!data.id;
    });
  }

  generatePDF() {
    this.pdfServiceSub = this.pdfService.getEvaluareConcluziiPdf(this.idAngajat, this.yearFilter).subscribe(data => {
      const blob = this.pdfService.base64ToBlob(data.fileContents, data.contentType);
      const fileURL = URL.createObjectURL(blob);
      window.open(fileURL, '_blank');
    });
  }

}
