import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { UntilDestroy } from '@ngneat/until-destroy';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { DropdownSelection } from 'src/app/models/common/Dorpdown';
import { HeaderTableFiltersModel } from 'src/app/models/common/HeaderTableFiltersModel';
import { NCompartimentDisplay } from 'src/app/models/nomenclatoare/NCompartimente';
import { CompartimenteService } from 'src/app/services/nomenclatoare/compartimente.service';
import { FirmeService } from 'src/app/services/nomenclatoare/firme.service';
import { LocatiiService } from 'src/app/services/nomenclatoare/locatii.service';

@UntilDestroy({ checkProperties: true })
@Component({
    selector: 'app-compartimente',
    templateUrl: './compartimente.component.html',
    standalone: false
})
export class CompartimenteComponent implements OnInit {
  compartimente: NCompartimentDisplay[] = [];
  headers: string[] = [];
  pages: number = 0;
  currentPage: number = 1;
  itemsPerPage: number = 10;
  filtru: string = '';
  filtruIdFirma: number | null = null;
  ddFirme: DropdownSelection[];
  idFirmaLoggedInUser: number | null;
  compartimentSub: Subscription;
  locatieSub: Subscription;
  firmeSub: Subscription;
  tableFiltersSub: Subscription;

  constructor(private firmeService: FirmeService,
    private compartimenteService: CompartimenteService,
    private locatieService: LocatiiService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private toastr: ToastrService
  ) {
    this.idFirmaLoggedInUser = JSON.parse(localStorage.getItem('user'))?.idFirma;
    this.getDDFirme();
  }

  ngOnInit() {
    this.headers = [
      'Denumire',
      'Locatie',
      'Subcompartiment',
      'Stare',
      'Firma',
      'Modificați',
      'Ștergeți',
    ];

    this.tableFiltersSub = this.compartimenteService.tableFiltersObs$.subscribe((data: HeaderTableFiltersModel) => {
      this.filtru = data?.filter;
      this.filtruIdFirma = data?.filterFirma;
      this.currentPage = data?.currentPage;
      this.itemsPerPage = data?.itemsPerPage;
    });

    this.getCompartimente(this.currentPage, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  deleteCompartiment(id: number) {
    this.compartimenteService.delete(id).subscribe({
      next: () => {
        setTimeout(() => {
          window.location.reload();
        }, 1000);
        this.toastr.success(
          'Compartimentul a fost șters cu succes!',
          'Succes!'
        );
      }, error: (error) => {
        this.toastr.error(
          `Compartimentul nu a putut fi șters!, ${error.message}`,
          'Eroare!'
        );
      }
    }
    );
  }

  navToAdd() {
    this.router.navigate(['add'], { relativeTo: this.activatedRoute });
  }

  nextPage(nxtPg: number) {
    this.getCompartimente(nxtPg, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  previousPage(prvPg: number) {
    this.getCompartimente(prvPg, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  lastPage(lstPg: number) {
    this.getCompartimente(lstPg, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  changeItemsPerPage(itmsPg: number) {
    this.currentPage = 1;
    this.itemsPerPage = itmsPg;
    this.getCompartimente(this.currentPage, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  filter(fltr: string) {
    this.filtru = fltr;
    this.currentPage = 1;
    this.getCompartimente(this.currentPage, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  private getCompartimente(page: number, itemsPerPage: number, filter: string, idFirma: number | null) {
    this.compartimenteService.getAllPaginated(page, itemsPerPage, filter, idFirma);
    this.compartimentSub = this.compartimenteService.compartimenteDisplayModelObs$.subscribe((resp) => {
      this.locatieSub = this.locatieService.getLocatiiData()
        .subscribe((locatii) => {
          this.compartimente = resp?.compartimente.map((compartiment) => {
            return {
              ...compartiment,
              locatie:
                locatii?.find(
                  (locatie) => locatie.id === compartiment.id_Locatie
                )?.denumire || '',
            };
          });
        });
      this.pages = resp?.pages;
      this.currentPage = resp?.currentPage;
    });
  }

  filterFirma(idFirma: number) {
    this.filtruIdFirma = idFirma;
    this.currentPage = 1;
    this.pages = 0;
    this.getCompartimente(this.currentPage, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  private getDDFirme() {
    this.firmeSub = this.firmeService.firmeSelections$.subscribe((data) => {
      this.ddFirme = data;
    });
  }
}
