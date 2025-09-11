import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { UntilDestroy } from '@ngneat/until-destroy';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { DropdownSelection } from 'src/app/models/common/Dorpdown';
import { HeaderTableFiltersModel } from 'src/app/models/common/HeaderTableFiltersModel';
import { NPosturi } from 'src/app/models/nomenclatoare/NPosturi';
import { FirmeService } from 'src/app/services/nomenclatoare/firme.service';
import { PosturiService } from 'src/app/services/nomenclatoare/posturi.service';

@UntilDestroy({ checkProperties: true })
@Component({
    selector: 'app-posturi',
    templateUrl: './posturi.component.html',
    standalone: false
})
export class PosturiComponent implements OnInit {
  posturi: NPosturi[] = [];
  headers: string[] = [];
  pages: number;
  currentPage: number;
  itemsPerPage: number;
  filtru: string;
  filtruIdFirma: number | null = null;
  ddFirme: DropdownSelection[];
  idFirmaLoggedInUser: number | null;
  posturiSub: Subscription;
  firmeSub: Subscription;
  tableFiltersSub: Subscription;

  constructor(private posturiService: PosturiService,
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
      'COR',
      'Denumire funcție',
      'Nivel post',
      'Stare',
      'Firma',
      'Modificați',
      'Ștergeți',
      'Setați profilul postului'
    ];

    this.tableFiltersSub = this.posturiService.tableFiltersObs$.subscribe((data: HeaderTableFiltersModel) => {
      this.filtru = data?.filter;
      this.filtruIdFirma = data?.filterFirma;
      this.currentPage = data?.currentPage;
      this.itemsPerPage = data?.itemsPerPage;
    });

    this.getPosturi(this.currentPage, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  deletePost(id: number) {
    this.posturiService.delete(id).subscribe({
      next: () => {
        setTimeout(() => {
          window.location.reload();
        }, 1000);
        this.toastr.success('Postul a fost șters cu succes!', 'Succes!');
      }, error: (error) => {
        this.toastr.error(
          `Postul nu a putut fi șters!, ${error.message}`,
          'Eroare!'
        );
      }
    });
  }

  navToAdd() {
    this.router.navigate(['add'], { relativeTo: this.activatedRoute });
  }

  nextPage(nxtPg: number) {
    this.getPosturi(nxtPg, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  previousPage(prvPg: number) {
    this.getPosturi(prvPg, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  lastPage(lstPg: number) {
    this.getPosturi(lstPg, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  changeItemsPerPage(itmsPg: number) {
    this.currentPage = 1;
    this.itemsPerPage = itmsPg;
    this.getPosturi(this.currentPage, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  filter(fltr: string) {
    this.filtru = fltr;
    this.currentPage = 1;
    this.getPosturi(this.currentPage, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  private getPosturi(page: number, itemsPerPage: number, filter: string, idFirma: number | null) {
    this.posturiService.getAllPaginated(page, itemsPerPage, filter, idFirma);

    this.posturiSub = this.posturiService.posturiDisplayModelObs$.subscribe((post) => {
      this.posturi = post?.posturi;
      this.pages = post?.pages;
      this.currentPage = post?.currentPage;
    });
  }

  filterFirma(idFirma: number) {
    this.filtruIdFirma = idFirma;
    this.currentPage = 1;
    this.pages = 0;
    this.getPosturi(this.currentPage, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  private getDDFirme() {
    this.firmeSub = this.firmeService.firmeSelections$.subscribe((data) => {
      this.ddFirme = data;
    });
  }

}
