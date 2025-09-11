import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UntilDestroy } from '@ngneat/until-destroy';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { SelectionBox } from 'src/app/models/SelectionBox';
import { DropdownSelection } from 'src/app/models/common/Dorpdown';
import { NObiective } from 'src/app/models/nomenclatoare/NObiective';
import { FirmeService } from 'src/app/services/nomenclatoare/firme.service';
import { NobiectiveService } from 'src/app/services/nomenclatoare/nobiective.service';
import { DropdownStateService } from 'src/app/states/dropdown/dropdown-state.service';
import { SelectionBoxStateService } from 'src/app/states/selectionBox/selectionBox.service';

@UntilDestroy({ checkProperties: true })
@Component({
  selector: 'nom-obiective-add',
  templateUrl: './nom-obiective-add-edit.component.html',
  standalone: false
})
export class NomObiectiveAddComponent implements OnInit {
  isEditMode: boolean = false;
  nObiectiv: NObiective = {} as NObiective;
  ddPosturi: DropdownSelection[] = [];
  ddCompartimente: DropdownSelection[] = [];
  allDdPosturi: DropdownSelection[];
  allDdCompartimente: DropdownSelection[];
  frecventeSelection: SelectionBox[] = [];
  tipuriSelection: SelectionBox[] = [];
  ddFirmeSelectionList: DropdownSelection[];
  loggedInUserHasFirma: boolean = false;
  bonusMinIsPercentage: boolean = false;
  bonusTargetIsPercentage: boolean = false;
  bonusMaxIsPercentage: boolean = false;
  ddSub: Subscription;
  selectionsSub: Subscription;
  tipuriSub: Subscription;
  firmeSub: Subscription;
  firmaLoggedInUserSub: Subscription;

  constructor(private ddStateService: DropdownStateService,
    private selectionBoxStateService: SelectionBoxStateService,
    private nObiectiveService: NobiectiveService,
    private toastr: ToastrService,
    private router: Router,
    private firmeService: FirmeService,
    private route: ActivatedRoute) {

    this.nObiectiv = {} as NObiective;
    this.selectionsSub = this.selectionBoxStateService.selections$.subscribe((data) => {
      if (data) {
        this.frecventeSelection = data.FrecventaObiectiveSelection;
        this.tipuriSelection = data.TipObiectiveSelection;
      }
    });

    this.firmeSub = this.firmeService.firmeSelections$.subscribe((data) => {
      this.ddFirmeSelectionList = data;;
    });

    this.firmaLoggedInUserSub = this.firmeService.firmaLoggedInUser$.subscribe((data) => {
      if (data) {
        this.loggedInUserHasFirma = true;
        this.nObiectiv.firma = data.Text;
        this.nObiectiv.idFirma = data.IdFirma;
      }
    });
  }

  ngOnInit(): void {
    this.ddSub = this.ddStateService.appDropdownsSubject$.subscribe((data) => {
      this.ddPosturi = data?.DdPosturi;
      this.ddCompartimente = data?.DdCompartimente;
      this.allDdPosturi = data?.DdPosturi;
      this.allDdCompartimente = data?.DdCompartimente;
      this.filterDropdowns(this.nObiectiv.idFirma);
    });
  }

  onSubmit() {
    this.nObiectiv.updateId = JSON.parse(localStorage.getItem('user')).id.toString();

    if (this.bonusMinIsPercentage || this.bonusTargetIsPercentage || this.bonusMaxIsPercentage) {
      this.nObiectiv.isBonusProcentual = true;
    } else {
      this.nObiectiv.isBonusProcentual = false;
    }

    this.nObiectiveService.add(this.nObiectiv).subscribe({
      next: () => {
        this.ddStateService.upsertDropdowns();
        this.toastr.success('Obiectivul a fost adaugăt cu succes!', 'Succes!');
        setTimeout(() => {
          this.goToNomObiective();
        }, 500);
      }, error: (err) => {
        this.toastr.error(
          `Obiectivul nu a putut fi adaugăt!, ${err.message}`,
          'Eroare!'
        );
      }
    });
  }
  private goToNomObiective() {
    this.router.navigate(['/nomObiective'], { relativeTo: this.route });
  }

  private filterDropdowns(filterIdFirma: number | null) {
    if (filterIdFirma) {
      this.ddCompartimente = this.allDdCompartimente.filter(x => x.IdFirma === +filterIdFirma);
      this.ddPosturi = this.allDdPosturi.filter(x => x.IdFirma === +filterIdFirma);
    }
  }

  firmaChanged(idFirma: number | null) {
    this.filterDropdowns(idFirma);
  }

}
