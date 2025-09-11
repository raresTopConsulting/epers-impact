import { Component } from '@angular/core';
import { UntilDestroy } from '@ngneat/until-destroy';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { DropdownSelection } from 'src/app/models/common/Dorpdown';
import { HeaderTableFiltersModel } from 'src/app/models/common/HeaderTableFiltersModel';
import { ConcluziiListaSubalterni } from 'src/app/models/concluzii/ConcluziiListaSubalterni';
import { ConcluziiService } from 'src/app/services/concluzii/concluzii.service';
import { FirmeService } from 'src/app/services/nomenclatoare/firme.service';
import { DropdownStateService } from 'src/app/states/dropdown/dropdown-state.service';
import { SelectionBoxStateService } from 'src/app/states/selectionBox/selectionBox.service';

@UntilDestroy({ checkProperties: true })
@Component({
  selector: 'lista-subalterni-concluzii',
  templateUrl: './lista-subalterni-concluzii.component.html',
  standalone: false
})
export class ListaSubalterniConcluziiComponent {
  listaSubalterni: ConcluziiListaSubalterni[];
  pages: number = 0;
  currentPage: number = 1;
  itemsPerPage: number = 10;
  filtru: string = '';
  filtruIdFirma: number | null = null;
  ddFirme: DropdownSelection[];
  idFirmaLoggedInUser: number | null;
  listSubalterniSub: Subscription;
  firmeSub: Subscription;
  tableFiltersSub: Subscription;

  constructor(private concluziiService: ConcluziiService,
    private firmeService: FirmeService,
    private toastr: ToastrService,
    private ddStateService: DropdownStateService,
    private selectionBoxStateService: SelectionBoxStateService) {
    this.idFirmaLoggedInUser = JSON.parse(localStorage.getItem('user'))?.idFirma;
    this.getDDFirme();

    this.tableFiltersSub = this.concluziiService.tableFiltersObs$.subscribe((data: HeaderTableFiltersModel) => {
      this.filtru = data?.filter;
      this.filtruIdFirma = data?.filterFirma;
      this.currentPage = data?.currentPage;
      this.itemsPerPage = data?.itemsPerPage;
    });

    this.ddStateService.upsertDropdowns();
    this.selectionBoxStateService.upsertSelections();

    this.getListaSubalterni(this.currentPage, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  private getDDFirme() {
    this.firmeSub = this.firmeService.firmeSelections$.subscribe((data) => {
      this.ddFirme = data;
    });
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

  private getListaSubalterni(pg: number, itmsPg: number, fltr: string, idFrima: number | null) {
    this.concluziiService.getListaSubalterni(pg, itmsPg, fltr, idFrima);
    this.listSubalterniSub = this.concluziiService.concluziiDisplayModelObs$.subscribe({
      next: (data) => {
        this.listaSubalterni = data?.listaSubalterni;
        this.pages = data?.pages;
        this.currentPage = data?.currentPage;
      }, error: (error) => {
        this.toastr.error(
          `Lista subalternilor nu a putut fi preluată!, ${error.message}`,
          'Eroare!'
        );
      }
    });
  }

}
