import { Component } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { ListaSubalterniPipModel } from 'src/app/models/pip/PlanInbaunatirePerformante';
import { PdfService } from 'src/app/services/common/pdf.service';
import { PipService } from 'src/app/services/pip/pip.service';
import { ModalPipDetailsComponent } from '../modal-pip-details/modal-pip-details.component';
import { ToastrService } from 'ngx-toastr';
import { FirmeService } from 'src/app/services/nomenclatoare/firme.service';
import { UntilDestroy } from '@ngneat/until-destroy';
import { DropdownSelection } from 'src/app/models/common/Dorpdown';

@UntilDestroy({ checkProperties: true })
@Component({
    selector: 'istoric-lista-subalterni-with-pip',
    templateUrl: './istoric-lista-subalterni-with-pip.component.html',
    standalone: false
})
export class IstoricListaSubalterniWithPipComponent {
  listaSubalterni: ListaSubalterniPipModel[];
  pages: number = 0;
  currentPage: number = 1;
  itemsPerPage: number = 10;
  filtru: string = '';
  filtruIdFirma: number | null = null;
  ddFirme: DropdownSelection[];
  years = Array.from({ length: 50 }, (_, i) => 2024 + i);
  yearFilter: number;
  idFirmaLoggedInUser: number | null;
  listSubalterniSub: Subscription;
  pdfServiceSub: Subscription;
  firmeSub: Subscription;

  constructor(private pipService: PipService,
    private pdfService: PdfService,
    private toastr: ToastrService,
    private firmeService: FirmeService,
    private dialog: MatDialog) {
    this.getDDFirme();
    this.idFirmaLoggedInUser = JSON.parse(localStorage.getItem('user'))?.idFirma;
    this.currentPage = 1;
    this.yearFilter = new Date().getFullYear();
    this.getListaSubalterni(this.currentPage, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  nextPage(nxtPg: number) {
    this.getListaSubalterni(nxtPg, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  previousPage(prvPg: number) {
    this.getListaSubalterni(prvPg, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  lastPage(lstPg: number) {
    this.getListaSubalterni(lstPg, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  changeItemsPerPage(itmsPg: number) {
    this.currentPage = 1;
    this.itemsPerPage = itmsPg;
    this.getListaSubalterni(this.currentPage, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  filter(fltr: string) {
    this.filtru = fltr;
    this.currentPage = 1;
    this.getListaSubalterni(this.currentPage, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  filterFirma(idFirma: number) {
    this.filtruIdFirma = idFirma;
    this.currentPage = 1;
    this.pages = 0;
    this.getListaSubalterni(this.currentPage, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  openPipDetailsModal(idAngajat: number) {
    const dialogConfig = new MatDialogConfig();

    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = '60%';
    dialogConfig.height = '80%';

    dialogConfig.data = {
      idAngajat: idAngajat
    };

    this.dialog.open(ModalPipDetailsComponent, dialogConfig);
  }

  onChangeYearFilter() {
    this.getListaSubalterni(this.currentPage, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  private getListaSubalterni(pg: number, itmsPg: number, fltr: string, idFirma: number | null) {
    this.listSubalterniSub?.unsubscribe();
    this.listSubalterniSub = this.pipService.getListaSubalterniWithPipIstoric(pg, itmsPg, this.yearFilter, fltr, idFirma).subscribe({
      next: (data) => {
        this.listaSubalterni = data.angajatiPip;
        this.pages = data.pages;
        this.currentPage = data.currentPage;
      }, error: (error) => {
        this.toastr.error(
          `Lista subalternilor nu a putut fi preluată!, ${error.message}`,
          'Eroare!'
        );
      }
    });
  }

  private getDDFirme() {
    this.firmeSub = this.firmeService.firmeSelections$.subscribe((data) => {
      this.ddFirme = data;
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