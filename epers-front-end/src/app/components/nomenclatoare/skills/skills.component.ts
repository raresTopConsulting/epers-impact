import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { UntilDestroy } from '@ngneat/until-destroy';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { DropdownSelection } from 'src/app/models/common/Dorpdown';
import { HeaderTableFiltersModel } from 'src/app/models/common/HeaderTableFiltersModel';
import { NSkills } from 'src/app/models/nomenclatoare/NSkills';
import { FirmeService } from 'src/app/services/nomenclatoare/firme.service';
import { SkillsService } from 'src/app/services/nomenclatoare/skills.service';

@UntilDestroy({checkProperties: true})
@Component({
    selector: 'skills',
    templateUrl: './skills.component.html',
    standalone: false
})
export class SkillsComponent implements OnInit {
  skills: NSkills[] = [];
  headers: string[] = [];
  pages: number = 0;
  currentPage: number = 1;
  itemsPerPage: number = 10;
  filtru: string = '';
  filtruIdFirma: number | null = null;
  ddFirme: DropdownSelection[];
  idFirmaLoggedInUser: number | null;
  skillsSub: Subscription;
  firmeSub: Subscription;
  tableFiltersSub: Subscription;

  constructor(private firmeService: FirmeService,
    private skillsService: SkillsService,
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
      'Tip',
      'Stare',
      'Firma',
      'Modificați',
      'Ștergeți',
    ];

    this.tableFiltersSub = this.skillsService.tableFiltersObs$.subscribe((data: HeaderTableFiltersModel) => {
      this.filtru = data?.filter;
      this.filtruIdFirma = data?.filterFirma;
      this.currentPage = data?.currentPage;
      this.itemsPerPage = data?.itemsPerPage;
    });

    this.getCompetente(this.currentPage, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  deleteSkill(id: number) {
    this.skillsService.delete(id).subscribe({
      next: () => {
        setTimeout(() => {
          window.location.reload();
        }, 1000);
        this.toastr.success('Competența a fost ștersă cu succes!', 'Succes!');
      }, error: (error) => {
        this.toastr.error(
          `Competența nu a putut fi ștersă!, ${error.message}`,
          'Eroare!'
        );
      }
    });
  }

  navToAdd() {
    this.router.navigate(['add'], { relativeTo: this.activatedRoute });
  }

  nextPage(nxtPg: number) {
    this.getCompetente(nxtPg, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  previousPage(prvPg: number) {
    this.getCompetente(prvPg, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  lastPage(lstPg: number) {
    this.getCompetente(lstPg, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  changeItemsPerPage(itmsPg: number) {
    this.currentPage = 1;
    this.itemsPerPage = itmsPg;
    this.getCompetente(this.currentPage, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  filter(fltr: string) {
    this.filtru = fltr;
    this.currentPage = 1;
    this.getCompetente(this.currentPage, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  private getCompetente(page: number, itemsPerPage: number, filter: string, idFirma: number | null) {
    this.skillsService.getAllPaginated(page, itemsPerPage, filter, idFirma);
    this.skillsSub = this.skillsService.skillsDisplayModelObs$.subscribe((resp) => {
        this.skills = resp?.skills;
        this.pages = resp?.pages;
        this.currentPage = resp?.currentPage;
      });
  }

  filterFirma(idFirma: number) {
    this.filtruIdFirma = idFirma;
    this.currentPage = 1;
    this.pages = 0;
    this.getCompetente(this.currentPage, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  private getDDFirme() {
    this.firmeSub = this.firmeService.firmeSelections$.subscribe((data) => {
      this.ddFirme = data;
    });
  }
}
