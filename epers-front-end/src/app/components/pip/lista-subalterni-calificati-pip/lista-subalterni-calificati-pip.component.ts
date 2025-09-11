import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { ListaSubalterniCalificatiPipModel } from 'src/app/models/pip/ListaSubalterniCalificatiPipModel';
import { PipService } from 'src/app/services/pip/pip.service';
import { AprobarePipModalComponent } from '../aprobare-pip-modal/aprobare-pip-modal.component';
import { DropdownSelection } from 'src/app/models/common/Dorpdown';
import { UntilDestroy } from '@ngneat/until-destroy';
import { FirmeService } from 'src/app/services/nomenclatoare/firme.service';

@UntilDestroy({checkProperties: true})
@Component({
    selector: 'lista-subalterni-calificati-pip',
    templateUrl: './lista-subalterni-calificati-pip.component.html',
    standalone: false
})
export class ListaSubalterniCalificatiPipComponent implements OnInit {
  listaSubalterni: ListaSubalterniCalificatiPipModel[];
  pages: number = 0;
  currentPage: number = 1;
  itemsPerPage: number = 10;
  filtru: string = '';
  filtruIdFirma: number | null = null;
  ddFirme: DropdownSelection[];
  years = Array.from({ length: 50 }, (_, i) => 2024 + i);
  yearFilter: number;
  loggedInUserIdRol: number;
  showListaAngajatiCuMedieDePip: boolean = false;
  idFirmaLoggedInUser: number | null;
  listSubalterniSub: Subscription;
  firmeSub: Subscription;

  constructor(private pipService: PipService,
    private toastr: ToastrService,
    private firmeService: FirmeService,
    private dialog: MatDialog) {
    this.getDDFirme();
    this.idFirmaLoggedInUser = JSON.parse(localStorage.getItem('user'))?.idFirma;
    this.currentPage = 1;
    this.yearFilter = new Date().getFullYear();
    this.loggedInUserIdRol = JSON.parse(localStorage.getItem('user')).idRol;
  }

  ngOnInit(): void {
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

  openPipStartModal(idAngajat: number) {
    const dialogConfig = new MatDialogConfig();

    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = '80%';
    dialogConfig.height = '95%';

    dialogConfig.data = {
      idAngajat: idAngajat,
      numePrenumeAngajat: this.listaSubalterni.find(x => x.idAngajat == idAngajat)?.numePrenume
    };

    this.dialog.open(AprobarePipModalComponent, dialogConfig);
  }

  actualizareSubalterniThatNeedPip() {
    this.pipService.actualizareListaSublaterniThatNeedPip(this.yearFilter).subscribe({
      next: (data) => {
        this.toastr.success(data?.message);
        setTimeout(() => {
          this.getListaSubalterni(this.currentPage, this.itemsPerPage, null, this.filtruIdFirma);
          window.location.reload();
        }, 1000);
      }, error: (error) => {
        this.toastr.error(
          `Lista subalternilor nu a putut fi actualizată!`,
          'Eroare!'
        );
      }
    });
  }

  onChangeYearFilter() {
    this.getListaSubalterni(this.currentPage, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  private getDDFirme() {
    this.firmeSub = this.firmeService.firmeSelections$.subscribe((data) => {
      this.ddFirme = data;
    });
  }

  onTipListaChange() {
    this.showListaAngajatiCuMedieDePip = !this.showListaAngajatiCuMedieDePip;
    this.getListaSubalterni(this.currentPage, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  private getListaSubalterni(pg: number, itmsPg: number, fltr: string, idFirma: number | null) {
    if (!this.showListaAngajatiCuMedieDePip) {
      this.listSubalterniSub = this.pipService.getListaSubalterniPentruAprobare(pg, itmsPg, this.yearFilter, fltr, idFirma).subscribe({
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
    } else {
      this.listSubalterniSub = this.pipService.getListaSubalterniCalificatiForPIP(pg, itmsPg, this.yearFilter, fltr, idFirma).subscribe({
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
  }
}