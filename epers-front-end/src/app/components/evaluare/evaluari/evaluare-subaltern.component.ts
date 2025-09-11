import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UntilDestroy } from '@ngneat/until-destroy';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { Concluzie } from 'src/app/models/concluzii/Concluzie';
import { AfisareSkillsEvalModel } from 'src/app/models/evaluare/AfisareEval';
import { EvaluareTemplate } from 'src/app/models/evaluare/Evaluare';
import { ConcluziiService } from 'src/app/services/concluzii/concluzii.service';
import { EmailEvaluareService } from 'src/app/services/email/email-evaluare.service';
import { EvaluareService } from 'src/app/services/evaluare/evaluare.service';

@UntilDestroy({ checkProperties: true })
@Component({
    selector: 'evaluare-subaltern',
    templateUrl: './evaluare.component.html',
    standalone: false
})
export class EvaluareSubalternComponent implements OnInit {
  idAngajat: number;
  skillsEval: AfisareSkillsEvalModel;
  evaluare: EvaluareTemplate = {} as EvaluareTemplate;
  valoriIdeale: number[] = [1, 2, 3, 4, 5];
  observatiiAnterioare: string[];
  isEvalFinala: boolean = false;
  isAutoEvaluare: boolean = false;
  years = Array.from({ length: 50 }, (_, i) => 2024 + i);
  anSelectat: number = new Date().getFullYear();
  autoEvalSiEvalSefIsDone: boolean = null;
  canNavigateToConcluzii: boolean = false;
  showSemnificatieNote: boolean = true;
  contestarePosibila: boolean = true;
  alreadySaved: boolean = false;
  isSaving: boolean = false;
  concluzie: Concluzie;
  existaConcluzie: boolean;
  evalSub: Subscription;
  concluziiAnulSelectatSub: Subscription;

  constructor(private route: ActivatedRoute,
    private router: Router,
    private evalService: EvaluareService,
    private emailService: EmailEvaluareService,
    private concluzieService: ConcluziiService,
    private toastr: ToastrService) {
    this.route.params.subscribe(params => {
      this.idAngajat = +params['id'];
    });
  }

  ngOnInit() {
    this.getEvaluare();
    this.checkExistaConcluzii(this.idAngajat, this.anSelectat);
  }

  searchEval() {
    this.getEvaluare();
    this.checkExistaConcluzii(this.idAngajat, this.anSelectat);
  }

  submitForm() {
    this.isSaving = true;

    this.evaluare.tipEvaluare = 2;
    this.evaluare.anul = this.anSelectat;
    this.evalService.upsertEvaluare(this.evaluare).subscribe({
      next: () => {
        this.isSaving = false;
        this.toastr.success('Evaluarea a fost salvată cu succes!');
        this.alreadySaved = true;
        this.emailService.sendEmailEvaluareSublatern(this.idAngajat);
        setTimeout(() => {
          this.router.navigate(['../../listaSubalterni'], { relativeTo: this.route });
        }, 500);
      },
      error: (err) => {
        this.isSaving = false;
        this.toastr.error(err?.error);
      }
    });
  }

  contestare() {
    this.evalService.contestareEvaluare(this.idAngajat, this.anSelectat).subscribe({
      next: () => {
        this.toastr.success('Contestația a fost efectuată cu succes! Procesul de evaluare finală poate fi reluat!');
        setTimeout(() => {
          this.router.navigate(['../../listaSubalterni'], { relativeTo: this.route });
        }, 500);
      }, error: (err) => {
        this.toastr.error(err?.error);
      }
    });
  }

  private getEvaluare() {
    this.evalSub = this.evalService.getAfisareSkillsEval(this.idAngajat, this.anSelectat).subscribe((data) => {
      this.evaluare = data as unknown as EvaluareTemplate;
      if (data.dateEval.length > 0) {
        this.autoEvalSiEvalSefIsDone = data.dateEval.every(ev => ev.val && ev.valIndiv);
      }
      this.evaluare.idAngajat = this.idAngajat;
      this.evaluare.dateEval.map((el) => {
        el.val = null,
          el.valIndiv = null,
          el.obs = null
      });

      if (data.flagFinalizat && data.dateEval.every(ev => !!ev.dataEvalFinala)) {
        const evalFinalYear = new Date(data.dateEval[0].dataEvalFinala).getFullYear();
        const evalFinalMonth = new Date(data.dateEval[0].dataEvalFinala).getMonth();
        const evalFinalDay = new Date(data.dateEval[0].dataEvalFinala).getDate();
        let dataEvaluareFinala = new Date(evalFinalYear, evalFinalMonth, evalFinalDay);
        if (this.checkIfPassed5Days(dataEvaluareFinala) || this.existaConcluzie) {
          this.contestarePosibila = false;
        } else {
          this.contestarePosibila = true;
        }
      }
    });
  }

  private checkIfPassed5Days(dataEvalFinala: Date) {
    const today = new Date();
    const diff = Math.abs(dataEvalFinala.getTime() - today.getTime());
    const diffInDays = Math.ceil(diff / (1000 * 3600 * 24));

    return diffInDays > 5;
  }

  private checkExistaConcluzii(idAngajat: number, an: number) {
    this.concluziiAnulSelectatSub = this.concluzieService.getIstoric(idAngajat, an).subscribe((data) => {
      if (data) {
        this.concluzie = data;
      } else {
        this.concluzie = {} as Concluzie;
      }
      this.existaConcluzie = !!this.concluzie?.concluziiAspecteGen || !!this.concluzie?.concluziiEvalCantOb || !!this.concluzie?.concluziiEvalCompActDezProf || !!this.concluzie?.idTraining;
    });
  }

}
