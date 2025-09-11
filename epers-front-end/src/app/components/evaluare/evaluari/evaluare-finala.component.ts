import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UntilDestroy } from '@ngneat/until-destroy';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { Concluzie } from 'src/app/models/concluzii/Concluzie';
import { AfisareEvalCalificativFinal, AfisareSkillsEvalModel } from 'src/app/models/evaluare/AfisareEval';
import { EvaluareCreateModel, EvaluareTemplate } from 'src/app/models/evaluare/Evaluare';
import { ConcluziiService } from 'src/app/services/concluzii/concluzii.service';
import { EmailEvaluareService } from 'src/app/services/email/email-evaluare.service';
import { EvaluareService } from 'src/app/services/evaluare/evaluare.service';

@UntilDestroy({ checkProperties: true })
@Component({
    selector: 'evaluare-finala',
    templateUrl: './evaluare.component.html',
    standalone: false
})
export class EvaluareFinalaComponent implements OnInit {
  idAngajat: number;
  skillsEval: AfisareSkillsEvalModel;
  evaluare: EvaluareTemplate = {} as EvaluareTemplate;
  observatiiAnterioare: string[];
  valoriIdeale: number[] = [1, 2, 3, 4, 5];
  isEvalFinala: boolean = true;
  isAutoEvaluare: boolean = false;
  years = Array.from({ length: 50 }, (_, i) => 2024 + i);
  anSelectat: number = new Date().getFullYear();
  autoEvalSiEvalSefIsDone: boolean = null;
  userHasPip: boolean;
  showAddPip: boolean;
  evalCalificativFinalData: AfisareEvalCalificativFinal;
  canNavigateToConcluzii: boolean;
  showSemnificatieNote: boolean = true;
  contestarePosibila: boolean;
  alreadySaved: boolean = false;
  isSaving: boolean = false;
  concluzie: Concluzie;
  existaConcluzie: boolean;
  evalSub: Subscription;
  concluziiAnulSelectatSub: Subscription;

  constructor(private route: ActivatedRoute,
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

  contestare() {
    this.evalService.contestareEvaluare(this.idAngajat, this.anSelectat).subscribe({
      next: () => {
        this.toastr.success('Contestația a fost efectuată cu succes! Procesul de evaluare finală poate fi reluat!');
        this.getEvaluare();
      }, error: (err) => {
        this.toastr.error(err?.error);
      }
    });
  }

  submitForm() {
    this.isSaving = true;
    this.evaluare.tipEvaluare = 3;
    this.evaluare.anul = this.anSelectat;
    this.evalService.upsertEvaluare(this.evaluare).subscribe({
      next: () => {
        this.isSaving = false;
        this.toastr.success('Evaluarea a fost salvată cu succes!');
        this.alreadySaved = true;
        this.canNavigateToConcluzii = true;
        this.emailService.sendEmailEvaluareFinala(this.idAngajat);
        this.getEvaluare();
      },
      error: (err) => {
        this.toastr.error(err?.error);
        this.isSaving = false;
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
      this.observatiiAnterioare = this.evaluare.dateEval.map(ev => ev.obs);
      this.evaluare.dateEval.map(ev => ev.obs = "");

      if (data.flagFinalizat && data.dateEval.every(ev => !!ev.dataEvalFinala)) {
        this.canNavigateToConcluzii = true;
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
      this.computeValoriIdealePosibile(this.evaluare.dateEval);
    });
  }

  private computeValoriIdealePosibile(dateEval: EvaluareCreateModel[]) {
    const minValIndiv = this.evaluare.dateEval.reduce((prev, current) => {
      return (prev.valIndiv < current.valIndiv) ? prev : current;
    }).valIndiv;
    const minVal = this.evaluare.dateEval.reduce((prev, current) => {
      return (prev.val < current.val) ? prev : current;
    }).val;
    const maxValIndiv = this.evaluare.dateEval.reduce((prev, current) => {
      return (prev.valIndiv > current.valIndiv) ? prev : current;
    }).valIndiv;
    const maxVal = this.evaluare.dateEval.reduce((prev, current) => {
      return (prev.val > current.val) ? prev : current;
    }).val;
    const min = Math.min(minVal, minValIndiv);
    const max = Math.max(maxVal, maxValIndiv);

    this.valoriIdeale = this.valoriIdeale.filter(v => v >= min && v <= max);
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

  private checkIfPassed5Days(dataEvalFinala: Date) {
    const today = new Date();
    const diff = Math.abs(dataEvalFinala.getTime() - today.getTime());
    const diffInDays = Math.ceil(diff / (1000 * 3600 * 24));

    return diffInDays > 5;
  }

}
