import { Component } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { UntilDestroy } from '@ngneat/until-destroy';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { DropdownSelection } from 'src/app/models/common/Dorpdown';
import { NSkills } from 'src/app/models/nomenclatoare/NSkills';
import { SelectionBox } from 'src/app/models/SelectionBox';
import { FirmeService } from 'src/app/services/nomenclatoare/firme.service';
import { SkillsService } from 'src/app/services/nomenclatoare/skills.service';
import { DropdownStateService } from 'src/app/states/dropdown/dropdown-state.service';
import { SelectionBoxStateService } from 'src/app/states/selectionBox/selectionBox.service';

@UntilDestroy({ checkProperties: true })
@Component({
    selector: 'skill-add',
    templateUrl: './skill-add-edit.component.html',
    standalone: false
})
export class SkillAddComponent {
  tipComp: SelectionBox[];
  skill: NSkills;
  isEditMode: boolean = false;
  loggedInUserHasFirma: boolean = false;
  ddFirmeSelectionList: DropdownSelection[];
  tipCompSub: Subscription;
  firmeSub: Subscription;
  firmaLoggedInUserSub: Subscription;

  constructor(private firmeService: FirmeService,
    private skillService: SkillsService,
    private toastr: ToastrService,
    private selectionBoxService: SelectionBoxStateService,
    private router: Router,
    private ddService: DropdownStateService,
    private route: ActivatedRoute
  ) {
    this.skill = {
      id: 0,
      denumire: "",
      descriere: "",
      dataIn: null,
      dataSf: null,
      tip: null,
      detalii: null,
      activ: false,
      firma: "",
      idFirma: null
    };

    this.tipCompSub = this.selectionBoxService.selections$.subscribe((data) => {
      this.tipComp = data?.TipCompetenteSelection;
    });
    this.firmeSub = this.firmeService.firmeSelections$.subscribe((data) => {
      this.ddFirmeSelectionList = data;;
    });

    this.firmaLoggedInUserSub = this.firmeService.firmaLoggedInUser$.subscribe((data) => {
      if (data) {
        this.loggedInUserHasFirma = true;
        this.skill.firma = data.Text;
        this.skill.idFirma = data.IdFirma;
      }
    });
  }

  save() {
    this.skillService.add(this.skill).subscribe({
      next: () => {
        this.selectionBoxService.upsertSelections();
        this.ddService.upsertDropdowns();
        this.toastr.success('Competența a fost adaugată cu succes!', 'Succes!');
        setTimeout(() => {
          this.goToLocatii();
        }, 500);
      }, error: (error) => {
        this.toastr.error(
          `Competența nu a putut fi adaugată!, ${error.message}`,
          'Eroare!'
        );
      }
    });
  }

  onCancel() {
    this.goToLocatii();
  }

  private goToLocatii() {
    this.router.navigate(['/skills'], { relativeTo: this.route });
  }

}
