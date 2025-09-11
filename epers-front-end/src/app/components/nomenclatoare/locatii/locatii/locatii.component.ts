import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UntilDestroy } from '@ngneat/until-destroy';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { DropdownSelection } from 'src/app/models/common/Dorpdown';
import { HeaderTableFiltersModel } from 'src/app/models/common/HeaderTableFiltersModel';
import { NLocatii } from 'src/app/models/nomenclatoare/NLocatii';
import { FirmeService } from 'src/app/services/nomenclatoare/firme.service';
import { LocatiiService } from 'src/app/services/nomenclatoare/locatii.service';

@UntilDestroy({ checkProperties: true })
@Component({
    selector: 'nomenclatoare-locatii',
    templateUrl: './locatii.component.html',
    standalone: false
})
export class LocatiiComponent implements OnInit {
  locatii: NLocatii[] = [];
  headers: string[] = [];
  pages: number = 0;
  currentPage: number = 1;
  itemsPerPage: number = 10;
  filtru: string = '';
  filtruIdFirma: number | null = null;
  ddFirme: DropdownSelection[];
  idFirmaLoggedInUser: number | null;

  locatiiSub: Subscription;
  firmeSub: Subscription;
  tableFiltersSub: Subscription;

  constructor(
    private locatiiService: LocatiiService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private firmeService: FirmeService,
    private toastr: ToastrService) {
    this.idFirmaLoggedInUser = JSON.parse(localStorage.getItem('user'))?.idFirma;
    this.getDDFirme();
  }

  ngOnInit() {
    this.headers = [
      'Denumire',
      'Adresa',
      'Localitate',
      'Judet',
      'Tara',
      'Stare',
      'Firma (Sediu principal firmă)',
      'Modificați',
      'Ștergeți',
    ];
    
    this.tableFiltersSub = this.locatiiService.tableFiltersObs$.subscribe((data: HeaderTableFiltersModel) => {
      this.filtru = data?.filter;
      this.currentPage = data?.currentPage;
      this.itemsPerPage = data?.itemsPerPage;
    });

    
    this.getLocatii(this.currentPage, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  deleteLocatie(id: number) {
    this.locatiiService.delete(id).subscribe({
      next: () => {
        setTimeout(() => {
          window.location.reload();
        }, 1000);
        this.toastr.success('Locatia a fost ștersă cu succes!', 'Succes!');
      }, error: (error) => {
        this.toastr.error(
          `Locatia nu a putut fi ștersă!, ${error.message}`,
          'Eroare!'
        );
      }
    });
  }

  navToAdd() {
    this.router.navigate(['add'], { relativeTo: this.activatedRoute });
  }

  nextPage(nxtPg: number) {
    this.getLocatii(nxtPg, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  previousPage(prvPg: number) {
    this.getLocatii(prvPg, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  lastPage(lstPg: number) {
    this.getLocatii(lstPg, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  changeItemsPerPage(itmsPg: number) {
    this.currentPage = 1;
    this.itemsPerPage = itmsPg;
    this.getLocatii(this.currentPage, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  filter(fltr: string) {
    this.filtru = fltr;
    this.currentPage = 1;
    this.getLocatii(this.currentPage, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  filterFirma(idFirma: number) {
    this.filtruIdFirma = idFirma;
    this.currentPage = 1;
    this.pages = 0;
    this.getLocatii(this.currentPage, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  private getLocatii(page: number, itemsPerPage: number, filter: string, idFirma: number | null) {
    this.locatiiService.getAllPaginated(page, itemsPerPage, filter, idFirma);
    this.locatiiSub = this.locatiiService.locatiiDisplayModelObs$.subscribe((loc) => {
      this.locatii = loc?.locatii;
      this.pages = loc?.pages;
      this.currentPage = loc?.currentPage;
    });
  }

  private getDDFirme() {
    this.firmeSub = this.firmeService.firmeSelections$.subscribe((data) => {
      this.ddFirme = data;
    });
  }

}
