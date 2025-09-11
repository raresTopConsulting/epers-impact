import { Component, Input, OnInit } from '@angular/core';
import { UntilDestroy } from '@ngneat/until-destroy';
import { Subscription } from 'rxjs';
import { PipDisplayAddEditModel } from 'src/app/models/pip/PlanInbaunatirePerformante';
import { PdfService } from 'src/app/services/common/pdf.service';
import { PipService } from 'src/app/services/pip/pip.service';

@UntilDestroy({checkProperties: true})
@Component({
    selector: 'pip-details',
    templateUrl: './pip-details.component.html',
    standalone: false
})
export class PipDetailsComponent implements OnInit {
  @Input() idAngajat: number;
  years = Array.from({ length: 50 }, (_, i) => 2022 + i);
  yearFilter: number;

  errorMessage: string;
  pipDetailsDisplay: PipDisplayAddEditModel;
  pipDetailsDisplaySub: Subscription;
  pdfServiceSub: Subscription;

  constructor(private pipService: PipService,
    private pdfService: PdfService,
  ) {
    this.yearFilter = new Date().getFullYear();
  }

  ngOnInit() {
    this.getPip(this.idAngajat, this.yearFilter);
  }

  onSelectYear(selectedYear: number) {
    this.getPip(this.idAngajat, selectedYear);
  }

  private getPip(idAngajat, selectedYear) {
    this.pipDetailsDisplaySub = this.pipService.getPipDisplay(idAngajat, selectedYear).subscribe({
      next: (data) => {
        this.pipDetailsDisplay = data;
        this.errorMessage = "";
      }, error: (err) => {
        this.errorMessage = err.error;
      }
    });
  }

  generatePDFPIP(idAngajat: number) {
    this.pdfServiceSub = this.pdfService.getPipPdf(idAngajat, this.yearFilter).subscribe(data => {
      const blob = this.pdfService.base64ToBlob(data.fileContents, data.contentType);
      const fileURL = URL.createObjectURL(blob);
      window.open(fileURL, '_blank');
    });
  }

}
