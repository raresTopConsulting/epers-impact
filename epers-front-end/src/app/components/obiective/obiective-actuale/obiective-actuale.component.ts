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
    selector: 'obiective-actuale',
    templateUrl: './obiective-actuale.component.html',
    standalone: false
})
export class ObiectiveActualeComponent implements OnInit {
  obiectiveActuale: Obiective[] = [];
  idAngajat: number;
  matricolaAngajat: string;
  anul: number;
  pages: number = 0;
  currentPage: number = 1;
  itemsPerPage: number = 10;
  filtru: string = '';
  obActualePdf: TipPDF = TipPDF.OBIECTIVEACTUALE;
  obiectiveActSub: Subscription;
  pdfServiceSub: Subscription;

  constructor(private obService: ObiectiveService,
    private pdfService: PdfService,
    private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.idAngajat = +params['id'];
      this.getObActive(this.currentPage, this.itemsPerPage, this.filtru);
    });
  }

  nextPage(nxtPg: number) {
    this.getObActive(nxtPg, this.itemsPerPage, this.filtru);
  }

  previousPage(prvPg: number) {
    this.getObActive(prvPg, this.itemsPerPage, this.filtru);
  }

  lastPage(lstPg: number) {
    this.getObActive(lstPg, this.itemsPerPage, this.filtru);
  }

  changeItemsPerPage(itmsPg: number) {
    this.currentPage = 1;
    this.itemsPerPage = itmsPg;
    this.getObActive(this.currentPage, this.itemsPerPage, this.filtru);
  }

  filter(fltr: string) {
    this.filtru = fltr;
    this.currentPage = 1;
    this.getObActive(this.currentPage, this.itemsPerPage, this.filtru);
  }

  generatePDF() {
    this.pdfServiceSub = this.pdfService.getObiectiveActualePdf(this.idAngajat, null).subscribe(data => {
      const blob = this.pdfService.base64ToBlob(data.fileContents, data.contentType);
      const fileURL = URL.createObjectURL(blob);
      window.open(fileURL, '_blank');
    });
  }

  private getObActive(pg: number, itmsPg: number, fltr: string) {
    this.obiectiveActSub = this.obService.getObiectiveActuale(this.idAngajat, pg, itmsPg, fltr).subscribe(
      (data) => {
        this.obiectiveActuale = data.listaObActuale;
        this.pages = data.pages;
        this.currentPage = data.currentPage;
        if (data.listaObActuale && data.listaObActuale.length > 0) {
          this.anul = new Date(data.listaObActuale[0].dataIn).getFullYear();
          this.matricolaAngajat = data.listaObActuale[0].matricolaAngajat;
        }
      });
  }

}
