import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UntilDestroy } from '@ngneat/until-destroy';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { DropdownSelection } from 'src/app/models/common/Dorpdown';
import { HeaderTableFiltersModel } from 'src/app/models/common/HeaderTableFiltersModel';
import { NDivizii } from 'src/app/models/nomenclatoare/NDvizii';
import { DiviziiService } from 'src/app/services/nomenclatoare/divizii.service';
import { FirmeService } from 'src/app/services/nomenclatoare/firme.service';

@UntilDestroy({checkProperties: true})
@Component({
    selector: 'divizii',
    templateUrl: './divizii.component.html',
    standalone: false
})
export class DiviziiComponent implements OnInit {
  divizii: NDivizii[] = [];
  headers: string[] = [];
  pages: number = 0;
  filtru: string = '';
  currentPage: number = 1;
  itemsPerPage: number = 10;
  filtruIdFirma:  number | null = null;
  ddFirme: DropdownSelection[];
  idFirmaLoggedInUser: number | null;
  firmeSub: Subscription;
  diviziiSub: Subscription;
  tableFiltersSub: Subscription;

  constructor(private firmeService: FirmeService,
    private diviziiService: DiviziiService,
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
      'Descriere',
      'Stare activă / inactivă',
      'Firma',
      'Modificați',
      'Ștergeți',
    ];

    this.tableFiltersSub = this.diviziiService.tableFiltersDiviziiObs$.subscribe((data: HeaderTableFiltersModel) => {
      this.filtru = data?.filter;
      this.filtruIdFirma = data?.filterFirma;
      this.currentPage = data?.currentPage;
      this.itemsPerPage = data?.itemsPerPage;
    });

    this.getDivizii(this.currentPage, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  deleteDvizie(id: number) {
    this.diviziiService.delete(id).subscribe({
      next: () => {
        setTimeout(() => {
          window.location.reload();
        }, 1000);
        this.toastr.success('Divizia a fost ștersă cu succes!', 'Succes!');
      }, error: (error) => {
        this.toastr.error(
          `Divizia nu a putut fi ștersă!, ${error.message}`,
          'Eroare!'
        );
      }
    });
  }

  navToAdd() {
    this.router.navigate(['add'], { relativeTo: this.activatedRoute });
  }

  nextPage(nxtPg: number) {
    this.getDivizii(nxtPg, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  previousPage(prvPg: number) {
    this.getDivizii(prvPg, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  lastPage(lstPg: number) {
    this.getDivizii(lstPg, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  changeItemsPerPage(itmsPg: number) {
    this.currentPage = 1;
    this.itemsPerPage = itmsPg;
    this.getDivizii(this.currentPage, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  filter(fltr: string) {
    this.filtru = fltr;
    this.currentPage = 1;
    this.getDivizii(this.currentPage, this.itemsPerPage, this.filtru, this.filtruIdFirma)
  }

  private getDivizii(currentPage: number, itemsPerPage: number, filtru: string, idFirma: number | null) {
    this.diviziiService.getAllPaginated(currentPage, itemsPerPage, filtru, idFirma);
    this.diviziiSub = this.diviziiService.diviziiDisplayModelObs$.subscribe({
        next:(response) => {
          this.divizii = response?.divizii;
          this.pages = response?.pages;
          this.currentPage = response?.currentPage;
        }, error:(error) => {
          this.toastr.error(error.error.message);
        }
      });
  }

  filterFirma(idFirma: number) {
    this.filtruIdFirma = idFirma;
    this.currentPage = 1;
    this.pages = 0;
    
    this.getDivizii(this.currentPage, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  private getDDFirme() {
    this.firmeSub = this.firmeService.firmeSelections$.subscribe((data) => {
      this.ddFirme = data;
    });
  }

}
