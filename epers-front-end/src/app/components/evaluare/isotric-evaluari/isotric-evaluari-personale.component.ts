import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { TipPDF } from 'src/app/models/common/TipPDF';
import { AfisareSkillsEvalModel } from 'src/app/models/evaluare/AfisareEval';
import { EvaluareTemplate } from 'src/app/models/evaluare/Evaluare';
import { EvaluareService } from 'src/app/services/evaluare/evaluare.service';
import { PipService } from 'src/app/services/pip/pip.service';
import { ModalPipDetailsComponent } from '../../pip/modal-pip-details/modal-pip-details.component';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { PdfService } from 'src/app/services/common/pdf.service';
import { UntilDestroy } from '@ngneat/until-destroy';

@UntilDestroy({checkProperties: true})
@Component({
    selector: 'isotric-evaluari-persoanle',
    templateUrl: './isotric-evaluari.component.html',
    standalone: false
})
export class IsotricEvaluariPersonaleComponent implements OnInit {
  idAngajat: number;
  skillsEval: AfisareSkillsEvalModel;
  evaluare: EvaluareTemplate = {} as EvaluareTemplate;
  valoriIdeale: number[] = [1, 2, 3, 4, 5];
  years = Array.from({ length: 50 }, (_, i) => 2022 + i);
  yearFilter: number;
  displayChart: boolean = true;
  isIstoricEvalPersonala: boolean = true;
  istoricEvalPdf: TipPDF = TipPDF.ISTORICEVALUARE;
  chartImage: any;
  userHasPip: boolean;

  private pipServiceSub: Subscription;
  private evalureServiceSub: Subscription;
  private pdfEvalSiConclServiceSub: Subscription;
  private pdfEvalServiceSub: Subscription;

  constructor(private evalService: EvaluareService,
    private pipService: PipService,
    private pdfService: PdfService,
    private dialog: MatDialog) {

    this.idAngajat = JSON.parse(localStorage.getItem('user')).id;
    this.yearFilter = new Date().getFullYear();
  }

  ngOnInit() {
    this.getIstoricEval(this.idAngajat, this.yearFilter);
    this.checkIfHasPip(this.idAngajat, this.yearFilter);
  }

  searchEval() {
    this.getIstoricEval(this.idAngajat, this.yearFilter)
    this.checkIfHasPip(this.idAngajat, this.yearFilter);
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
    this.evalureServiceSub = this.evalService.getIstoricEval(idAngajat, anul).subscribe((data) => {
      this.evaluare = data as unknown as EvaluareTemplate;

      if (data?.dateEval[0]?.val || data?.dateEval[0]?.valIndiv) {
        this.displayChart = true;
        // Code to add chart to the PDF
        // const canvas = this.chartIstEval.getChart();
        // this.chartImage = canvas?.toDataURL('image/jpeg', 1.0);
      } else {
        this.displayChart = false;
      }
    });
  }

  private checkIfHasPip(idAngajat: number, anul: number | null) {
    this.pipServiceSub = this.pipService.getStareActualaPip(idAngajat, anul).subscribe((data) => {
      this.userHasPip = !!data.id;
    });
  }

  generatePDFEvalSiConcluzii() {
    this.pdfEvalSiConclServiceSub = this.pdfService.getEvaluareConcluziiPdf(this.idAngajat, this.yearFilter).subscribe(data => {
      const blob = this.pdfService.base64ToBlob(data.fileContents, data.contentType);
      const fileURL = URL.createObjectURL(blob);
      window.open(fileURL, '_blank');
    });
  }

  generatePDFEval(){
    this.pdfEvalServiceSub = this.pdfService.getEvaluarePdf(this.idAngajat, this.yearFilter).subscribe(data => {
      const blob = this.pdfService.base64ToBlob(data.fileContents, data.contentType);
      const fileURL = URL.createObjectURL(blob);
      window.open(fileURL, '_blank');
    });
  }

}
