import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
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
    selector: 'locatie-add',
    templateUrl: './locatie-add-edit.component.html',
    standalone: false
})
export class LocatieAddComponent implements OnInit {
  judete: SelectionBox[];
  locatie: NLocatii;
  isEditMode: boolean = false;
  seSchimbaSediuFirma: boolean = false;
  loggedInUserHasFirma: boolean = false;
  ddFirmeSelectionList: DropdownSelection[];
  judeteSub: Subscription;
  firmeSub: Subscription;
  firmaLoggedInUserSub: Subscription;

  constructor(private locatieService: LocatiiService,
    private toastr: ToastrService,
    private selectionBoxStateService: SelectionBoxStateService,
    private router: Router,
    private ddStateService: DropdownStateService,
    private route: ActivatedRoute,
    private firmeService: FirmeService
  ) { }

  ngOnInit() {
    this.locatie = {
      id: 0,
      denumire: "",
      adresa: "",
      localitate: "",
      judet: "",
      tara: "",
      dataIn: null,
      dataSf: null,
      activ: false,
      isSediuPrincipalFirma: false,
      idFirma: 0,
      firma: ""
    };

    this.judeteSub = this.selectionBoxStateService.selections$.subscribe((data) => {
      this.judete = data?.JudeteSelection;
    });

    this.firmeSub = this.firmeService.firmeSelections$.subscribe((data) => {
      this.ddFirmeSelectionList = data;
    });

    this.firmaLoggedInUserSub = this.firmeService.firmaLoggedInUser$.subscribe((data) => {
      if (data) {
        this.loggedInUserHasFirma = true;
        this.locatie.firma = data.Text;
        this.locatie.idFirma = data.IdFirma;
      }
    });
  }

  save() {
    this.locatieService.add(this.locatie).subscribe({
      next: () => {
        this.selectionBoxStateService.upsertSelections();
        this.firmeService.upsertDDFirme();
        this.ddStateService.upsertDropdowns();
        this.toastr.success('Locatia a fost adaugata cu succes!', 'Succes!');
        setTimeout(() => {
          this.goToLocatii();
        }, 500);
      },
      error: (err) => {
        this.toastr.error(
          `Locatia nu a putut fi adaugata!, ${err.message}`,
          'Eroare!'
        );
      }
    });
  }

  onCancel() {
    this.goToLocatii();
  }

  private goToLocatii() {
    this.router.navigate(['/locatii'], { relativeTo: this.route });
  }

  schimbareSediuFirma(isSediuFirma) {
    if (isSediuFirma == true && isSediuFirma != this.locatie.isSediuPrincipalFirma) {
      this.seSchimbaSediuFirma = true;
    } else {
      this.seSchimbaSediuFirma = false;
    }
  }

}
