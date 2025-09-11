import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { UntilDestroy } from '@ngneat/until-destroy';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { DropdownSelection } from 'src/app/models/common/Dorpdown';
import { NLocatii } from 'src/app/models/nomenclatoare/NLocatii';
import { SelectionBox } from 'src/app/models/SelectionBox';
import { FirmeService } from 'src/app/services/nomenclatoare/firme.service';
import { LocatiiService } from 'src/app/services/nomenclatoare/locatii.service';
import { DropdownStateService } from 'src/app/states/dropdown/dropdown-state.service';
import { SelectionBoxStateService } from 'src/app/states/selectionBox/selectionBox.service';

@UntilDestroy({ checkProperties: true })
@Component({
    selector: 'locatie-edit',
    templateUrl: './locatie-add-edit.component.html',
    standalone: false
})
export class LocatieEditComponent implements OnInit {
  judete: SelectionBox[];
  locatie: NLocatii;
  isEditMode: boolean = true;
  private id: number;
  seSchimbaSediuFirma: boolean = false;
  wasSetAsSediuPrincipalFirma: boolean = false;
  loggedInUserHasFirma: boolean = false;
  ddFirmeSelectionList: DropdownSelection[];
  firmeSub: Subscription;
  locatieSub: Subscription;
  judeteSub: Subscription;
  firmaLoggedInUserSub: Subscription;

  constructor(private firmeService: FirmeService,
    private locatieService: LocatiiService,
    private toastr: ToastrService,
    private router: Router,
    private ddStateService: DropdownStateService,
    private route: ActivatedRoute,
    private selectionBoxStateService: SelectionBoxStateService) {

    this.judeteSub = this.selectionBoxStateService.selections$.subscribe((data) => {
      this.judete = data?.JudeteSelection;
    });

    this.firmeSub = this.firmeService.firmeSelections$.subscribe((data) => {
      this.ddFirmeSelectionList = data;
    });

    this.firmaLoggedInUserSub = this.firmeService.firmaLoggedInUser$.subscribe((data) => {
      if (data) {
        this.loggedInUserHasFirma = true;
      }
    });
  }

  ngOnInit() {
    this.route.params.subscribe((param: Params) => {
      this.id = +param['id'];

      this.locatieSub = this.locatieService.get(this.id).subscribe((locatie) => {
        this.locatie = locatie;
        this.wasSetAsSediuPrincipalFirma = !!locatie?.isSediuPrincipalFirma ? locatie.isSediuPrincipalFirma : false;
      });
    });
  }

  save() {
    this.locatieService.update(this.locatie).subscribe({
      next: () => {
        this.firmeService.upsertDDFirme();
        this.ddStateService.upsertDropdowns();
        this.toastr.success('Locația a fost modificată cu succes!', 'Succes!');
        setTimeout(() => {
          this.goToLocatii();
        }, 1000);
      }, error: (err) => {
        this.toastr.error(
          `Locația nu a putut fi modificată!, ${err.message}`,
          'Eroare!'
        );
      }
    });
  }
  onCancel() {
    this.goToLocatii();
  }

  schimbareSediuFirma(isSediuFirma) {
    this.locatie.isSediuPrincipalFirma = isSediuFirma;
    if (isSediuFirma && !this.wasSetAsSediuPrincipalFirma) {
      this.seSchimbaSediuFirma = true;
    } else {
      this.seSchimbaSediuFirma = false;
    }
  }

  private goToLocatii() {
    this.router.navigate(['/locatii']);
  }

}
