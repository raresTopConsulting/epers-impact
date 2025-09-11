import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { UntilDestroy } from '@ngneat/until-destroy';
import { Subscription } from 'rxjs';
import { SelectionBox } from 'src/app/models/SelectionBox';
import { DropdownSelection } from 'src/app/models/common/Dorpdown';
import { ObiectivTemplate } from 'src/app/models/obiective/Obiective';
import { ObiectiveService } from 'src/app/services/obiective/obiective.service';
import { DropdownStateService } from 'src/app/states/dropdown/dropdown-state.service';
import { SelectionBoxStateService } from 'src/app/states/selectionBox/selectionBox.service';

@UntilDestroy({ checkProperties: true })
@Component({
  selector: 'obiectiv-from-dd',
  templateUrl: './obiectiv-from-dd.component.html',
  standalone: false
})
export class ObiectivFromDdComponent implements OnInit {
  @Input() idFirma: number | null;
  @Output() addObFromNomenclator: EventEmitter<ObiectivTemplate> = new EventEmitter();

  obTemplateFromNom: ObiectivTemplate = {} as ObiectivTemplate;
  ddObiective: DropdownSelection[];
  frecventeSelection: SelectionBox[];
  selectedObNomFrecventa: string = "";
  selectedObFromNomId: number;
  selectedObNomDataDin: Date;
  bonusMinIsPercentage: boolean = false;
  bonusTargetIsPercentage: boolean = false;
  bonusMaxIsPercentage: boolean = false;

  nomObSub: Subscription;
  frecventeSub: Subscription;
  obServiceSub: Subscription;
  ddSub: Subscription;

  constructor(private ddStateService: DropdownStateService,
    private selectionBoxStateService: SelectionBoxStateService,
    private obService: ObiectiveService) {

    this.frecventeSub = this.selectionBoxStateService.selections$.subscribe((data) => {
      this.frecventeSelection = data?.FrecventaObiectiveSelection;
    });
  }

  ngOnInit() {
    this.ddSub = this.ddStateService.appDropdownsSubject$.subscribe((data) => {
      if (this.idFirma) {
        this.ddObiective = data?.DdObiective.filter(ob => ob.IdFirma === this.idFirma);
      } else {
        this.ddObiective = data?.DdObiective;
      }
    });
  }

  onObFromNomChange() {
    this.obServiceSub = this.obService.getObFromNomOb(this.selectedObFromNomId).subscribe((data) => {
      this.obTemplateFromNom = data as unknown as ObiectivTemplate;
      this.obTemplateFromNom.frecventa = this.frecventeSelection?.find(obf => obf.Id.toString() === data.frecventa)?.Valoare;
      this.selectedObNomFrecventa = data.frecventa;
      // this.bonusMinIsPercentage = this.bonusTargetIsPercentage = this.bonusMaxIsPercentage = this.obTemplateFromNom.isBonusProcentual;
    });
  }

  onAddObiectivFromNom() {
    this.obTemplateFromNom.frecventa = this.selectedObNomFrecventa;
    this.obTemplateFromNom.dataIn = this.selectedObNomDataDin;

    if (this.bonusMinIsPercentage || this.bonusTargetIsPercentage || this.bonusMaxIsPercentage) {
      this.obTemplateFromNom.isBonusProcentual = true;
    } else if (!this.bonusMinIsPercentage || !this.bonusTargetIsPercentage || !this.bonusMaxIsPercentage) {
      this.obTemplateFromNom.isBonusProcentual = false;
    }

    this.addObFromNomenclator.emit(this.obTemplateFromNom);
  }

}
