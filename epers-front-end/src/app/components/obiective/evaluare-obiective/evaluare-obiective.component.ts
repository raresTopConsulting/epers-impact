import { Component } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { UntilDestroy } from '@ngneat/until-destroy';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { Obiective } from 'src/app/models/obiective/Obiective';
import { EmailObiectiveService } from 'src/app/services/email/email-obiective.service';
import { ObiectiveService } from 'src/app/services/obiective/obiective.service';

@UntilDestroy({checkProperties: true})
@Component({
    selector: 'app-evaluare-obiective',
    templateUrl: './evaluare-obiective.component.html',
    standalone: false
})
export class EvaluareObiectiveComponent {
  obiectiveActuale: Obiective[] = [];
  idAngajat: number;
  pages: number = 0;
  currentPage: number = 1;
  itemsPerPage: number = 10;
  filtru: string = '';
  seEvalObCalitativ: boolean [] = [];
  isSaving: boolean = false;
  obiectiveActSub: Subscription;

  constructor(private obService: ObiectiveService,
    private route: ActivatedRoute,
    private toastr: ToastrService,
    private emailService: EmailObiectiveService,
    private router: Router) {
    this.route.params.subscribe(params => {
      this.idAngajat = +params['id'];
      this.getObActive(this.currentPage, this.itemsPerPage, this.filtru);
    });
  }

  save() {
    this.isSaving = true;
    let obiectiveEvaluate: Obiective[] = [];

    this.obiectiveActuale.forEach(elem => {
      if (elem.valoareRealizata) {
        obiectiveEvaluate.push(elem);
      }
      if (elem.isRealizat === true || elem.isRealizat === false) {
        obiectiveEvaluate.push(elem);
      }
    });
    this.obService.evaluareObiective(obiectiveEvaluate).subscribe({
      next: () => {
        this.isSaving = false;
        this.toastr.success("Obiectivele au fost evaluate cu succes!");
        this.emailService.sendEmailObiectiveEvaluate(this.idAngajat);
        setTimeout(() => {
          this.router.navigate(['../../listaSubalterni'], { relativeTo: this.route });
        }, 500);
      },
      error: (err) => {
        this.isSaving = false;
        this.toastr.error(err?.error?.message);
      }
    });
  }

  nextPage(nxtPg: number) {
    this.getObActive(nxtPg, this.itemsPerPage, this.filtru);
  }

  previousPage(prvPg: number) {
    this.getObActive(prvPg, this.itemsPerPage, this.filtru);
  }

  lastPage(lstPg: number) {
    this.getObActive(lstPg, this.itemsPerPage, this.filtru);
  }

  changeItemsPerPage(itmsPg: number) {
    this.currentPage = 1;
    this.itemsPerPage = itmsPg;
    this.getObActive(this.currentPage, this.itemsPerPage, this.filtru);
  }

  filter(fltr: string) {
    this.filtru = fltr;
    this.currentPage = 1;
    this.getObActive(this.currentPage, this.itemsPerPage, this.filtru);
  }

  private getObActive(pg: number, itmsPg: number, fltr: string) {
    this.obiectiveActSub?.unsubscribe();
    this.obiectiveActSub = this.obService.getObiectiveActuale(this.idAngajat, pg, itmsPg, fltr).subscribe(
      (data) => {
        this.obiectiveActuale = data.listaObActuale;
        this.obiectiveActuale = this.obiectiveActuale.map((ob) => ({
          ...ob,
          isRealizat: null
        }));
        this.pages = data.pages;
        this.currentPage = data.currentPage;
      });
  }

}
