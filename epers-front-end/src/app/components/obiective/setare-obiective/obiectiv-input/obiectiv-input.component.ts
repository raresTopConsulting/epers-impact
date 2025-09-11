import { Component, EventEmitter, Input, OnChanges, OnInit, Output } from '@angular/core';
import { UntilDestroy } from '@ngneat/until-destroy';
import { Subscription } from 'rxjs';
import { SelectionBox } from 'src/app/models/SelectionBox';
import { ObiectivTemplate } from 'src/app/models/obiective/Obiective';
import { SelectionBoxStateService } from 'src/app/states/selectionBox/selectionBox.service';

@UntilDestroy({ checkProperties: true })
@Component({
  selector: 'obiectiv-input',
  templateUrl: './obiectiv-input.component.html',
  standalone: false
})
export class ObiectivInputComponent implements OnInit, OnChanges {
  obiectivTemplate: ObiectivTemplate;
  frecventeSelection: SelectionBox[];
  frecventeSub: Subscription;
  bonusMinIsPercentage: boolean = false;
  bonusTargetIsPercentage: boolean = false;
  bonusMaxIsPercentage: boolean = false;

  @Output() addedObTemplate: EventEmitter<ObiectivTemplate> = new EventEmitter();
  @Input() isFrecvAndValIdentic: boolean;
  @Input() dataInceputIdentica: Date;
  @Input() frecventaIdentica: string;
  @Input() tipObiectiv: string;

  constructor(private selectionBoxStateService: SelectionBoxStateService) {
    this.obiectivTemplate = {} as ObiectivTemplate;
  }

  ngOnInit() {
    this.getFrecventeObSelection();
  }

  ngOnChanges() {
    if (this.isFrecvAndValIdentic) {
      this.obiectivTemplate.dataIn = this.dataInceputIdentica;
      this.obiectivTemplate.frecventa = this.frecventaIdentica;
    } else {
      this.obiectivTemplate.dataIn = null;
      this.obiectivTemplate.frecventa = null;
    }
    this.obiectivTemplate.tip = this.tipObiectiv;
  }

  onAddObiectiv() {
    if (this.bonusMinIsPercentage || this.bonusTargetIsPercentage || this.bonusMaxIsPercentage) {
      this.obiectivTemplate.isBonusProcentual = true;
    } else if (!this.bonusMinIsPercentage || !this.bonusTargetIsPercentage || !this.bonusMaxIsPercentage) {
      this.obiectivTemplate.isBonusProcentual = false;
    }

    this.addedObTemplate.emit(this.obiectivTemplate);
  }

  private getFrecventeObSelection() {
    this.frecventeSub = this.selectionBoxStateService.selections$.subscribe((data) => {
      this.frecventeSelection = data?.FrecventaObiectiveSelection;
    });
  }

}
