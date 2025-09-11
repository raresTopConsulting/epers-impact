import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { Concluzie } from 'src/app/models/concluzii/Concluzie';
import { ConcluziiService } from 'src/app/services/concluzii/concluzii.service';
import { EvaluareService } from 'src/app/services/evaluare/evaluare.service';
import { EvaluareTemplate } from 'src/app/models/evaluare/Evaluare';
import { AfisareEvalCalificativFinal, NoteMinimePip } from 'src/app/models/evaluare/AfisareEval';
import { PipService } from 'src/app/services/pip/pip.service';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { ModalPipDetailsComponent } from '../../pip/modal-pip-details/modal-pip-details.component';
import { PipEditComponent } from '../../pip/pip-edit/pip-edit.component';
import { PipAddComponent } from '../../pip/pip-edit/pip-add.component';
import { UntilDestroy } from '@ngneat/until-destroy';
import { DropdownSelection } from 'src/app/models/common/Dorpdown';
import { DropdownStateService } from 'src/app/states/dropdown/dropdown-state.service';
import { EmailConcluziiService } from 'src/app/services/email/email-concluzii.service';

@UntilDestroy({ checkProperties: true })
@Component({
    selector: 'concluzie-add',
    templateUrl: './concluzie-add.component.html',
    standalone: false
})
export class ConcluzieAddComponent implements OnInit {
  idAngajat: number;
  concluzie: Concluzie;
  idEvaluari: number[];
  cursuriSelection: DropdownSelection[] = [];
  cursuriSelectate: DropdownSelection[] = [];
  cursSelectat: number;
  showAddPip: boolean = false;
  userHasPip: boolean;
  userHasPipNefinalizat: boolean;
  userHasPipRespins: boolean;
  evaluare: EvaluareTemplate;
  istoricEvalCalificativ: AfisareEvalCalificativFinal;
  existaConcluzii: boolean;
  years = Array.from({ length: 50 }, (_, i) => 2022 + i);
  yearFilter: number;
  addConcluziiCantitative: boolean = false;
  idFirmaLoggedInUser: number | null;
  idsEvaluariSubalternSub: Subscription;
  cusrusiSelectionSub: Subscription;
  showAddPipSub: Subscription;
  stareActualaPipSub: Subscription;
  istoricEvalCalifFinSub: Subscription;
  concluziiAnulSelectatSub: Subscription;

  constructor(private route: ActivatedRoute,
    private router: Router,
    private concluzieService: ConcluziiService,
    private emailConcluziiService: EmailConcluziiService,
    private evalService: EvaluareService,
    private pipService: PipService,
    private dialog: MatDialog,
    private ddStateService: DropdownStateService,
    private toastr: ToastrService) {
    this.concluzie = {} as Concluzie;
    this.route.params.subscribe(params => {
      this.idAngajat = +params['id'];
    });
    this.yearFilter = new Date().getFullYear();
    this.idFirmaLoggedInUser = JSON.parse(localStorage.getItem('user'))?.idFirma;
  }

  ngOnInit() {
    this.cusrusiSelectionSub = this.ddStateService.appDropdownsSubject$.subscribe((data) => {
      this.cursuriSelection = data?.DdCursuri;
      if (this.idFirmaLoggedInUser) {
        this.cursuriSelection = data?.DdCursuri?.filter(curs => curs.IdFirma === this.idFirmaLoggedInUser);
      }
    });
    this.searchConcluzie();
    this.showAddPipSub = this.pipService.hasMediaForPip(this.idAngajat, this.yearFilter).subscribe((data) => {
      this.showAddPip = data;
    });
    this.stareActualaPipSub = this.pipService.getStareActualaPip(this.idAngajat, this.yearFilter).subscribe((data) => {
      if (data) {
        this.userHasPip = !!data?.id;
        this.userHasPipNefinalizat = data.id && data.id > 1 && data.id < 4;
        this.userHasPipRespins = data.id === 3
      }
    });
  }

  openPipAddModal() {
    const dialogConfig = new MatDialogConfig();

    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = '60%';
    dialogConfig.height = '80%';

    dialogConfig.data = {
      idAngajat: this.idAngajat
    };

    this.dialog.open(PipAddComponent, dialogConfig);
  }

  openPipDetailsModal() {
    const dialogConfig = new MatDialogConfig();

    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = '60%';
    dialogConfig.height = '80%';

    dialogConfig.data = {
      idAngajat: this.idAngajat
    };

    this.dialog.open(ModalPipDetailsComponent, dialogConfig);
  }

  openPipEditModal() {
    const dialogConfig = new MatDialogConfig();

    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = '60%';
    dialogConfig.height = '80%';

    dialogConfig.data = {
      idAngajat: this.idAngajat
    };

    this.dialog.open(PipEditComponent, dialogConfig);
  }

  searchConcluzie() {
    this.getConcluzii(this.idAngajat, this.yearFilter);
    this.getIstoricEvalCalificativFinal(this.idAngajat, this.yearFilter);
    this.getEvaluariForConcluzie(this.idAngajat, this.yearFilter);
  }

  save() {
    if (this.cursuriSelectate && this.cursuriSelectate.length > 0) {
      this.cursuriSelectate.forEach(elem => {
        if (elem && elem.Id) {
          if (this.concluzie.idTraining === null) {
            this.concluzie.idTraining = '';
          }
          this.concluzie.idTraining += elem.Id.toString() + ",";
        }
      });
    }

    this.concluzieService.addConcluzie(this.concluzie, this.idEvaluari).subscribe({
      next: () => {
        this.toastr.success("Concluziile au fost salvate cu succes!");
        this.emailConcluziiService.sendEmailConcluzii(this.idAngajat);
        setTimeout(() => {
          this.router.navigate(['../../listaSubalterni'], { relativeTo: this.route });
        }, 500);
      },
      error: (err) => {
        this.toastr.error(err?.error?.message);
      }
    })
  }

  selectCurs(idCurs: number) {
    let cursSelectat = this.cursuriSelection.find(c => c.Id === +idCurs);
    if (this.cursuriSelectate.map(c => c.Id !== +idCurs) && this.cursuriSelectate.length < 5) {
      this.cursuriSelectate.push({
        Id: +idCurs,
        Text: cursSelectat.Text,
        IdFirma: cursSelectat.IdFirma,
        Value: cursSelectat.Value
      });
    }
  }

  removeCurs(idCurs: number) {
    const index = this.cursuriSelectate.findIndex(x => x.Id === idCurs);
    this.cursuriSelectate.splice(index, 1);
  }

  private getConcluzii(idAngajat: number, an: number) {
    this.concluziiAnulSelectatSub = this.concluzieService.getIstoric(idAngajat, an).subscribe((data) => {
      if (data) {
        this.concluzie = data;
      } else {
        this.concluzie = {} as Concluzie;
      }
      this.existaConcluzii = !!this.concluzie?.concluziiAspecteGen || !!this.concluzie?.concluziiEvalCantOb || !!this.concluzie?.concluziiEvalCompActDezProf || !!this.concluzie?.idTraining;
    });
  }


  private getIstoricEvalCalificativFinal(idAngajat: number, an: number) {
    this.istoricEvalCalifFinSub = this.evalService.getIstoricEvalCalificativFinal(idAngajat, an).subscribe((data) => {
      if (data) {
        this.istoricEvalCalificativ = data;
        if (data.calificativFinal && data.calificativFinal <= NoteMinimePip.CALIFICATIV_MINIM_NECESITA_EFORT) {
          this.showAddPip = true;
        }
      }
    });
  }

  private getEvaluariForConcluzie(idAngajat: number, an: number) {
    this.idsEvaluariSubalternSub = this.concluzieService.getIdsEvaluariSubaltern(idAngajat, an).subscribe((data) => {
      this.idEvaluari = data;
    });
  }

}
