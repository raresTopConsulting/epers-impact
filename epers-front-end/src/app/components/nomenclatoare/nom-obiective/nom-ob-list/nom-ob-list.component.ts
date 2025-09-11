import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UntilDestroy } from '@ngneat/until-destroy';
import { Subscription } from 'rxjs';
import { SelectionBox } from 'src/app/models/SelectionBox';
import { DropdownSelection } from 'src/app/models/common/Dorpdown';
import { HeaderTableFiltersModel } from 'src/app/models/common/HeaderTableFiltersModel';
import { AfisareNObiective } from 'src/app/models/nomenclatoare/NObiective';
import { FirmeService } from 'src/app/services/nomenclatoare/firme.service';
import { NobiectiveService } from 'src/app/services/nomenclatoare/nobiective.service';
import { SelectionBoxStateService } from 'src/app/states/selectionBox/selectionBox.service';

@UntilDestroy({ checkProperties: true })
@Component({
    selector: 'nom-ob-list',
    templateUrl: './nom-ob-list.component.html',
    standalone: false
})
export class NomObListComponent {
  nObiective: AfisareNObiective[];
  pages: number = 0;
  currentPage: number = 1;
  itemsPerPage: number = 10;
  filtru: string = '';
  frecventeSelection: SelectionBox[] = [];
  filtruIdFirma: number | null = null;
  ddFirme: DropdownSelection[];
  idFirmaLoggedInUser: number | null;
  nomObiectiveSub: Subscription;
  frecventeSub: Subscription;
  firmeSub: Subscription;
  tableFiltersSub: Subscription;

  constructor(private nomObService: NobiectiveService,
    private router: Router,
    private firmeService: FirmeService,
    private activatedRoute: ActivatedRoute,
    private selectionBoxStateService: SelectionBoxStateService) {
    this.idFirmaLoggedInUser = JSON.parse(localStorage.getItem('user'))?.idFirma;
    this.getDDFirme();
    this.getFrecventeObSelection();

    this.tableFiltersSub = this.nomObService.tableFiltersObiect$.subscribe((data: HeaderTableFiltersModel) => {
      this.filtru = data?.filter;
      this.filtruIdFirma = data?.filterFirma;
      this.currentPage = data?.currentPage;
      this.itemsPerPage = data?.itemsPerPage;
    });

    this.getNomObiective(this.currentPage, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  navToAdd() {
    this.router.navigate(['add'], { relativeTo: this.activatedRoute });
  }

  nextPage(nxtPg: number) {
    this.getNomObiective(nxtPg, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  previousPage(prvPg: number) {
    this.getNomObiective(prvPg, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  lastPage(lstPg: number) {
    this.getNomObiective(lstPg, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  changeItemsPerPage(itmsPg: number) {
    this.currentPage = 1;
    this.itemsPerPage = itmsPg;
    this.getNomObiective(this.currentPage, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  filter(fltr: string) {
    this.filtru = fltr;
    this.currentPage = 1;
    this.getNomObiective(this.currentPage, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  filterFirma(idFirma: number) {
    this.filtruIdFirma = idFirma;
    this.currentPage = 1;
    this.pages = 0;
    this.getNomObiective(this.currentPage, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  private getNomObiective(page: number, itemsPerPage: number, filter: string, filtruIdFirma: number | null) {
    this.nomObService.getAllPaginated(page, itemsPerPage, filter, filtruIdFirma);
    this.nomObiectiveSub = this.nomObService.afisareNObDisplayModelObs$.subscribe((resp) => {
      this.nObiective = resp?.afisNomObiectiveData;
      this.pages = resp?.pages;
      this.currentPage = resp?.currentPage;
    });
  }

  private getFrecventeObSelection() {
    this.frecventeSub = this.selectionBoxStateService.selections$.subscribe((data) => {
      if (data) {
        this.frecventeSelection = data?.FrecventaObiectiveSelection;
      }
    });
  }

  private getDDFirme() {
    this.firmeSub = this.firmeService.firmeSelections$.subscribe((data) => {
      this.ddFirme = data;
    });
  }

}
