import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UntilDestroy } from '@ngneat/until-destroy';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { DropdownSelection } from 'src/app/models/common/Dorpdown';
import { HeaderTableFiltersModel } from 'src/app/models/common/HeaderTableFiltersModel';
import { NCursuri } from 'src/app/models/nomenclatoare/NCursuri';
import { CursuriService } from 'src/app/services/nomenclatoare/cursuri.service';
import { FirmeService } from 'src/app/services/nomenclatoare/firme.service';

@UntilDestroy({ checkProperties: true })
@Component({
    selector: 'cursuri',
    templateUrl: './cursuri.component.html',
    standalone: false
})
export class CursuriComponent implements OnInit {
  cursuri: NCursuri[] = [];
  headers: string[] = [];
  pages: number = 0;
  currentPage: number = 1;
  itemsPerPage: number = 10;
  filtru: string = '';
  filtruIdFirma: number | null = null;
  ddFirme: DropdownSelection[];
  idFirmaLoggedInUser: number | null;
  cursuriSub: Subscription;
  firmeSub: Subscription;
  tableFiltersSub: Subscription;

  constructor(private cursuriService: CursuriService,
    private router: Router,
    private firmeService: FirmeService,
    private activatedRoute: ActivatedRoute,
    private toastr: ToastrService) {
    this.idFirmaLoggedInUser = JSON.parse(localStorage.getItem('user'))?.idFirma;

    this.getDDFirme();
  }

  ngOnInit() {
    this.headers = [
      'Denumire',
      'Organizator',
      'Activ',
      'Online',
      'Locație/Link',
      'Firma',
      'Modificați',
      'Ștergeți',
    ];

    this.tableFiltersSub = this.cursuriService.tableFiltersObs$.subscribe((data: HeaderTableFiltersModel) => {
      this.filtru = data?.filter;
      this.filtruIdFirma = data?.filterFirma;
      this.currentPage = data?.currentPage;
      this.itemsPerPage = data?.itemsPerPage;
    });

    this.getCursuri(this.currentPage, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  deleteCurs(id: number) {
    this.cursuriService.delete(id).subscribe({
      next: () => {
        setTimeout(() => {
          window.location.reload();
        }, 1000);
        this.toastr.success(
          'Cursul a fost șters cu succes!',
          'Succes!'
        );
      }, error: (error) => {
        this.toastr.error(
          `Cursul nu a putut fi șters!, ${error.message}`,
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
    this.getCursuri(nxtPg, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  previousPage(prvPg: number) {
    this.getCursuri(prvPg, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  lastPage(lstPg: number) {
    this.getCursuri(lstPg, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  changeItemsPerPage(itmsPg: number) {
    this.currentPage = 1;
    this.itemsPerPage = itmsPg;
    this.getCursuri(this.currentPage, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  filter(fltr: string) {
    this.filtru = fltr;
    this.currentPage = 1;
    this.getCursuri(this.currentPage, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  private getCursuri(page: number, itemsPerPage: number, filter: string, idFirma: number | null) {
    this.cursuriService.getAll(page, itemsPerPage, filter, idFirma);
    this.cursuriSub = this.cursuriService.cursuriDisplayModelObs$.subscribe((resp) => {
      this.cursuri = resp?.cursuri;
      this.pages = resp?.pages;
      this.currentPage = resp?.currentPage;
    });
  }

  filterFirma(idFirma: number) {
    this.filtruIdFirma = idFirma;
    this.currentPage = 1;
    this.pages = 0;
    
    this.getCursuri(this.currentPage, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  private getDDFirme() {
    this.firmeSub = this.firmeService.firmeSelections$.subscribe((data) => {
      this.ddFirme = data;
    });
  }

}
