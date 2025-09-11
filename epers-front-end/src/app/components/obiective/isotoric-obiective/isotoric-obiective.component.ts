import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UntilDestroy } from '@ngneat/until-destroy';
import { Subscription } from 'rxjs';
import { TipPDF } from 'src/app/models/common/TipPDF';
import { Obiective } from 'src/app/models/obiective/Obiective';
import { PdfService } from 'src/app/services/common/pdf.service';
import { ObiectiveService } from 'src/app/services/obiective/obiective.service';

@UntilDestroy({checkProperties: true})
@Component({
    selector: 'app-isotoric-obiective',
    templateUrl: './isotoric-obiective.component.html',
    standalone: false
})
export class IsotoricObiectiveComponent implements OnInit {
  istoricObiective: Obiective[] = [];
  idAngajat: number;
  pages: number = 0;
  currentPage: number = 1;
  itemsPerPage: number = 10;
  filtru: string = '';
  istoricObPdf: TipPDF = TipPDF.OBIECTIVEISTORIC;
  obiectiveSub: Subscription;
  pdfServiceSub: Subscription;

  constructor(private obService: ObiectiveService,
    private route: ActivatedRoute,
    private pdfService: PdfService) {
    this.route.params.subscribe(params => {
      this.idAngajat = +params['id'];
      this.getIstoricOb(this.currentPage, this.itemsPerPage, this.filtru);
    });
  }

  ngOnInit() {
  }

  nextPage(nxtPg: number) {
    this.getIstoricOb(nxtPg, this.itemsPerPage, this.filtru);
  }

  previousPage(prvPg: number) {
    this.getIstoricOb(prvPg, this.itemsPerPage, this.filtru);
  }

  lastPage(lstPg: number) {
    this.getIstoricOb(lstPg, this.itemsPerPage, this.filtru);
  }

  changeItemsPerPage(itmsPg: number) {
    this.currentPage = 1;
    this.itemsPerPage = itmsPg;
    this.getIstoricOb(this.currentPage, this.itemsPerPage, this.filtru);
  }

  filter(fltr: string) {
    this.filtru = fltr;
    this.currentPage = 1;
    this.getIstoricOb(this.currentPage, this.itemsPerPage, this.filtru);
  }


  generatePDF() {
    this.pdfServiceSub = this.pdfService.getObiectiveIstoricPdf(this.idAngajat, null).subscribe(data => {
      const blob = this.pdfService.base64ToBlob(data.fileContents, data.contentType);
      const fileURL = URL.createObjectURL(blob);
      window.open(fileURL, '_blank');
    });
  }

  private getIstoricOb(pg: number, itmsPg: number, fltr: string) {
    this.obiectiveSub?.unsubscribe();
    this.obiectiveSub = this.obService.getIstoricObiective(this.idAngajat, pg, itmsPg, fltr).subscribe(
      (data) => {
        this.istoricObiective = data.listaObActuale;
        this.pages = data.pages;
        this.currentPage = data.currentPage;
      });
  }
}
