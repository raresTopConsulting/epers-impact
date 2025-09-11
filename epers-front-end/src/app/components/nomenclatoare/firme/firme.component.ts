import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UntilDestroy } from '@ngneat/until-destroy';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { HeaderTableFiltersModel } from 'src/app/models/common/HeaderTableFiltersModel';
import { NFirme } from 'src/app/models/nomenclatoare/NFirme';
import { FirmeService } from 'src/app/services/nomenclatoare/firme.service';

@UntilDestroy({ checkProperties: true })
@Component({
    selector: 'firme',
    templateUrl: './firme.component.html',
    standalone: false
})

export class FirmeComponent implements OnInit {
  firme: NFirme[] = [];
  headers: string[] = [];
  pages: number = 0;
  currentPage: number = 1;
  itemsPerPage: number = 10;
  filtru: string = '';
  firmeSub: Subscription = new Subscription();
  tableFiltersSub: Subscription = new Subscription();

  constructor(private firmeService: FirmeService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private toastr: ToastrService) { }

  ngOnInit() {
    this.headers = [
      'Denumire',
      'Cod Fiscal',
      'Tip Întreprindere',
      'Stare',
      'Modificați',
      'Ștergeți',
    ];

    this.tableFiltersSub = this.firmeService.tableFiltersObs$.subscribe((data: HeaderTableFiltersModel) => {
      this.filtru = data?.filter;
      this.currentPage = data?.currentPage;
      this.itemsPerPage = data?.itemsPerPage;
    });

    this.getFirme(this.currentPage, this.itemsPerPage, this.filtru);
  }

  deleteFirma(id: number) {
    this.firmeService.delete(id).subscribe({
      next: () => {
        setTimeout(() => {
          window.location.reload();
        }, 1000);
        this.toastr.success('Firma a fost ștersă cu succes!', 'Succes!');
      }, error: (error) => {
        this.toastr.error(
          `Firma nu a putut fi ștersă!, ${error.message}`,
          'Eroare!'
        );
      }
    });
  }

  navToAdd() {
    this.router.navigate(['add'], { relativeTo: this.activatedRoute });
  }

  nextPage(nxtPg: number) {
    this.getFirme(nxtPg, this.itemsPerPage, this.filtru);
  }

  previousPage(prvPg: number) {
    this.getFirme(prvPg, this.itemsPerPage, this.filtru);
  }

  lastPage(lstPg: number) {
    this.getFirme(lstPg, this.itemsPerPage, this.filtru);
  }

  changeItemsPerPage(itmsPg: number) {
    this.currentPage = 1;
    this.itemsPerPage = itmsPg;
    this.getFirme(this.currentPage, this.itemsPerPage, this.filtru);
  }

  filter(fltr: string) {
    this.filtru = fltr;
    this.currentPage = 1;
    this.getFirme(this.currentPage, this.itemsPerPage, this.filtru);
  }

  private getFirme(page: number, itemsPerPage: number, filter: string) {
    this.firmeService.getAll(page, itemsPerPage, filter);
    this.firmeSub = this.firmeService.firmeDisplayModelObs$.subscribe((firmeDisp) => {
      if (firmeDisp) {
        this.firme = firmeDisp.firme;
        this.pages = firmeDisp.pages;
        this.currentPage = firmeDisp.currentPage;
      }
    });
  }
}
