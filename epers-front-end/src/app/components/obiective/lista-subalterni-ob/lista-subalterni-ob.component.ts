import { Component } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { UntilDestroy } from '@ngneat/until-destroy';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { DropdownSelection } from 'src/app/models/common/Dorpdown';
import { HeaderTableFiltersModel } from 'src/app/models/common/HeaderTableFiltersModel';
import { SubalterniDropdown } from 'src/app/models/useri/User';
import { FirmeService } from 'src/app/services/nomenclatoare/firme.service';
import { ObiectiveService } from 'src/app/services/obiective/obiective.service';
import { PipAddComponent } from '../../pip/pip-edit/pip-add.component';
import { ModalPipDetailsComponent } from '../../pip/modal-pip-details/modal-pip-details.component';
import { DropdownStateService } from 'src/app/states/dropdown/dropdown-state.service';
import { SelectionBoxStateService } from 'src/app/states/selectionBox/selectionBox.service';

@UntilDestroy({ checkProperties: true })
@Component({
  selector: 'lista-subalterni-ob',
  templateUrl: './lista-subalterni-ob.component.html',
  standalone: false
})
export class ListaSubalterniObComponent {
  listaSubalterni: SubalterniDropdown[];
  pages: number = 0;
  currentPage: number = 1;
  itemsPerPage: number = 10;
  filtruIdFirma: number | null = null;
  ddFirme: DropdownSelection[];
  filtru: string = '';
  idFirmaLoggedInUser: number | null;

  firmeSub: Subscription;
  listSubalterniSub: Subscription;
  tableFiltersSub: Subscription;
  stareActualaPipSub: Subscription;

  constructor(private obService: ObiectiveService,
    private firmeService: FirmeService,
    private ddStateService: DropdownStateService,
    private selectionBoxStateService: SelectionBoxStateService,
    private dialog: MatDialog,
    private toastr: ToastrService) {
    this.idFirmaLoggedInUser = JSON.parse(localStorage.getItem('user'))?.idFirma;
    this.getDDFirme();

    this.tableFiltersSub = this.obService.tableFiltersObs$.subscribe((data: HeaderTableFiltersModel) => {
      this.filtru = data?.filter;
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
    this.obService.getListaSubalterni(pg, itmsPg, fltr, idFrima);
    this.listSubalterniSub = this.obService.obiectiveDisplayModelObs$.subscribe({
      next: (data) => {
        this.listaSubalterni = data?.listaSubalterni;
        this.pages = data?.pages;
        this.currentPage = data?.currentPage;
      },
      error: (error) => {
        this.toastr.error(
          `Lista subalternilor nu a putut fi preluată!, ${error.message}`,
          'Eroare!'
        );
      }
    });
  }

  openPipAddModal(idAngajat: number) {
    const dialogConfig = new MatDialogConfig();

    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = '60%';
    dialogConfig.height = '80%';

    dialogConfig.data = {
      idAngajat: idAngajat
    };

    this.dialog.open(PipAddComponent, dialogConfig);
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


}
