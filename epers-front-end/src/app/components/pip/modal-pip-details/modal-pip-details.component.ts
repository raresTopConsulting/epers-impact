import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { UntilDestroy } from '@ngneat/until-destroy';
import { Subscription } from 'rxjs';
import { PipDisplayAddEditModel } from 'src/app/models/pip/PlanInbaunatirePerformante';
import { PdfService } from 'src/app/services/common/pdf.service';
import { PipService } from 'src/app/services/pip/pip.service';

@UntilDestroy({checkProperties: true})
@Component({
    selector: 'modal-pip-details',
    templateUrl: './modal-pip-details.component.html',
    standalone: false
})
export class ModalPipDetailsComponent {
  idAngajat: number;
  pipDetailsDisplay: PipDisplayAddEditModel;

  pipDetailsDisplaySub: Subscription;
  pdfServiceSub: Subscription;

  constructor(private pipService: PipService,
    private pdfService: PdfService,
    private dialogRef: MatDialogRef<ModalPipDetailsComponent>,
    @Inject(MAT_DIALOG_DATA) data) {

    this.idAngajat = data.idAngajat;
    this.pipDetailsDisplaySub = this.pipService.getPipDisplay(this.idAngajat, null).subscribe((data) => {
      this.pipDetailsDisplay = data;
    });
  }

  generatePDFPIP(idAngajat: number) {
    this.pdfServiceSub = this.pdfService.getPipPdf(idAngajat, null).subscribe(data => {
      const blob = this.pdfService.base64ToBlob(data.fileContents, data.contentType);
      const fileURL = URL.createObjectURL(blob);
      window.open(fileURL, '_blank');
    });
  }

  close() {
    this.dialogRef.close();
  }

}
