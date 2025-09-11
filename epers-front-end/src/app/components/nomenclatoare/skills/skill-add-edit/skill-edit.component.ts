import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
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
    selector: 'skill-edit',
    templateUrl: './skill-add-edit.component.html',
    standalone: false
})
export class SkillEditComponent implements OnInit {
  skill: NSkills;
  tipComp: SelectionBox[];
  isEditMode: boolean = true;
  loggedInUserHasFirma: boolean = false;
  ddFirmeSelectionList: DropdownSelection[];
  firmeSub: Subscription;
  skillSub: Subscription;
  tipListSub: Subscription;
  firmaLoggedInUserSub: Subscription;
  private id: number;

  constructor(private firmeService: FirmeService,
    private skillsService: SkillsService,
    private ddService: DropdownStateService,
    private selectionBoxService: SelectionBoxStateService,
    private toastr: ToastrService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.firmeSub = this.firmeService.firmeSelections$.subscribe((data) => {
      this.ddFirmeSelectionList = data;;
    });

    this.firmaLoggedInUserSub = this.firmeService.firmaLoggedInUser$.subscribe((data) => {
      if (data) {
        this.loggedInUserHasFirma = true;
      }
    });

    this.tipListSub = this.selectionBoxService.selections$.subscribe((data) => {
      this.tipComp = data?.TipCompetenteSelection;
    });

    this.route.params.subscribe((param: Params) => {
      this.id = +param['id'];
      this.skillSub = this.skillsService.get(this.id).subscribe((locatie) => {
        this.skill = locatie;
      });
    });
  }

  save() {
    this.skillsService.update(this.id, this.skill).subscribe({
      next: () => {
        this.ddService.upsertDropdowns();
        this.selectionBoxService.upsertSelections();
        this.toastr.success(
          'Competența a fost modificată cu succes!',
          'Succes!'
        );
        setTimeout(() => {
          this.goToCompetente();
        }, 1000);
      }, error: (err) => {
        this.toastr.error(
          `Competența nu a putut fi modificată!, ${err.message}`,
          'Eroare!'
        );
      }
    });
  }

  onCancel() {
    this.goToCompetente();
  }

  private goToCompetente() {
    this.router.navigate(['/skills']);
  }
}
