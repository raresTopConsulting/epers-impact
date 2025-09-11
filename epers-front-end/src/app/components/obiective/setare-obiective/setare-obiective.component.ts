import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { SelectionBox } from 'src/app/models/SelectionBox';
import { ObiectivTemplate } from 'src/app/models/obiective/Obiective';
import { SetareObiective } from 'src/app/models/obiective/SetareOb';
import { ObiectiveService } from 'src/app/services/obiective/obiective.service';
import { HeaderService } from 'src/app/services/common/header.service';
import { SelectionBoxStateService } from 'src/app/states/selectionBox/selectionBox.service';
import { UserStateService } from 'src/app/services/user/user-state.service';
import { SubalterniDropdown } from 'src/app/models/useri/User';
import { UntilDestroy } from '@ngneat/until-destroy';
import { EmailObiectiveService } from 'src/app/services/email/email-obiective.service';

@UntilDestroy({checkProperties: true})
@Component({
    selector: 'setare-obiective',
    templateUrl: './setare-obiective.component.html',
    standalone: false
})
export class SetareObiectiveComponent implements OnInit {
  idAngajat: number;
  frecventeSelection: SelectionBox[];
  setareOb: SetareObiective;
  selectedObFromNomId: number;
  isAddObIndividual: boolean = false;
  isAddObDepartamental: boolean = false;
  isAddObCorporariv: boolean = false;
  isFrecvAndValIdentic: boolean = true;
  selectedDataDin: Date;
  selectedFrecventa: string = "";
  isAddObFromNomenclator: boolean = false;
  listaSubalterniDD: SubalterniDropdown[];
  subalterniSelectatiSetareOb: SubalterniDropdown[] = [];
  subalternSelectat: SubalterniDropdown;
  idFirma: number | null;
  isSaving: boolean = false;
  frecventeSub: Subscription;
  headerSub: Subscription;
  ddSublaterniSub: Subscription;
  userSub: Subscription;

  constructor(private route: ActivatedRoute,
    private router: Router,
    private selectionBoxStateService: SelectionBoxStateService,
    private obiectiveService: ObiectiveService,
    private toastr: ToastrService,
    private userService: UserStateService,
    private emailService: EmailObiectiveService,
    private headerService: HeaderService) {

    this.setareOb = {} as SetareObiective;
    this.setareOb.obiectivTemplate = [];
    this.route.params.subscribe(params => {
      this.idAngajat = +params['id'];
    });
  }

  ngOnInit() {
    this.userSub = this.userService.get(this.idAngajat).subscribe((data) => {
      this.idFirma = data?.idFirma;
    });

    this.getFrecventeObSelection();
    this.initSetObiectivModel();
    this.getDropdownSubalterni();
  }

  submitForm() {
    this.isSaving = true;
    let listaIdSublaterniSelectati = this.subalterniSelectatiSetareOb.map((x) => {
      return x.idAngajat;
    });
    this.obiectiveService.setareObiectiv(this.setareOb, listaIdSublaterniSelectati).subscribe({
      next: () => {
        this.isSaving = false;
        this.toastr.success("Obiectivele au fost salvate cu succes!");
        this.emailService.sendEmailObiectiveSelectate(this.idAngajat);
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

  flagFrecvAndDataChanged() {
    if (!this.isFrecvAndValIdentic) {
      this.selectedDataDin = null;
      this.selectedFrecventa = null;
    }
  }

  addObiectivTemplate(obiectiv: ObiectivTemplate) {
    this.isAddObIndividual = false;
    this.isAddObDepartamental = false;
    this.isAddObCorporariv = false;

    if (this.isFrecvAndValIdentic) {
      this.selectedDataDin = obiectiv.dataIn;
      this.selectedFrecventa = obiectiv.frecventa;
    } else {
      this.selectedDataDin = null;
      this.selectedFrecventa = null;
    }
    this.setareOb.obiectivTemplate.push(obiectiv);
  }

  addObiectivTemplateFromNom(obFromNom: ObiectivTemplate) {
    this.setareOb.obiectivTemplate.push(obFromNom);
  }

  removeOb(index: number) {
    this.setareOb.obiectivTemplate.splice(index, 1);
  }

  hideAddObForm() {
    this.isAddObIndividual = false;
    this.isAddObDepartamental = false;
    this.isAddObCorporariv = false;
  }

  hideAddObFormNom() {
    this.isAddObFromNomenclator = false;
  }

  onAddObFromNom() {
    this.isAddObFromNomenclator = true;
  }

  onSelectSubalt(idSelAngajat: string) {
    if (idSelAngajat === "none") {
      this.subalterniSelectatiSetareOb = [];
    }
    else if (idSelAngajat === "all") {
      this.subalterniSelectatiSetareOb = [...this.listaSubalterniDD];
    } else {
      let dataSelectedSubalt = this.listaSubalterniDD.find(subalt => subalt.idAngajat === +idSelAngajat);
      if (!this.subalterniSelectatiSetareOb.find(sel => sel.idAngajat === dataSelectedSubalt.idAngajat)) {
        this.subalterniSelectatiSetareOb.push(dataSelectedSubalt);
      }
    }
  }

  onAddObIndiv() {
    this.isAddObIndividual = true;
    this.isAddObCorporariv = false;
    this.isAddObDepartamental = false;
  }

  onAddObDep() {
    this.isAddObDepartamental = true;
    this.isAddObIndividual = false;
    this.isAddObCorporariv = false;
  }

  onAddObCorp() {
    this.isAddObCorporariv = true;
    this.isAddObIndividual = false;
    this.isAddObDepartamental = false;
  }

  private getDropdownSubalterni() {
    this.ddSublaterniSub = this.userService.getDropdownSubalterni().subscribe((data) => {
      this.listaSubalterniDD = data;
    });
  }

  private getFrecventeObSelection() {
    this.frecventeSub = this.selectionBoxStateService.selections$.subscribe((data) => {
      if (data) {
        this.frecventeSelection = data?.FrecventaObiectiveSelection;
      }
    });
  }

  private initSetObiectivModel() {
    this.setareOb.idAngajat = this.idAngajat.toString();
    this.headerSub = this.headerService.getHeaderData(this.idAngajat).subscribe((data) => {
      this.setareOb.idSuperior = data.idSuperior.toString();
      this.setareOb.matricolaAngajat = data.matricola;
      this.setareOb.matricolaSuperior = data.matricolaSef;
      this.setareOb.idCompartiment = data.idCompartiment;
      this.setareOb.idPost = data.idPost;
      this.subalterniSelectatiSetareOb.push({
        idAngajat: this.idAngajat,
        matricolaAngajat: data.matricola,
        numePrenume: data.numePrenume,
        postAngajat: data.denumirePost,
        cor: data.cor,
        hideStartPip: false
      });
    });
  }

}
