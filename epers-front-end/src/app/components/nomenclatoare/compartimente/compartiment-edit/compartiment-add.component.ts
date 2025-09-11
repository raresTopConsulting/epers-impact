import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UntilDestroy } from '@ngneat/until-destroy';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { DropdownSelection } from 'src/app/models/common/Dorpdown';
import { NCompartimentDisplay } from 'src/app/models/nomenclatoare/NCompartimente';
import { NLocatii } from 'src/app/models/nomenclatoare/NLocatii';
import { CompartimenteService } from 'src/app/services/nomenclatoare/compartimente.service';
import { FirmeService } from 'src/app/services/nomenclatoare/firme.service';
import { LocatiiService } from 'src/app/services/nomenclatoare/locatii.service';
import { DropdownStateService } from 'src/app/states/dropdown/dropdown-state.service';

@UntilDestroy({ checkProperties: true })
@Component({
    selector: 'compartiment-add',
    templateUrl: './compartiment-add-edit.component.html',
    standalone: false
})
export class CompartimentAddComponent implements OnInit {
  compartiment: NCompartimentDisplay;
  locatii: NLocatii[];
  isEditMode: boolean = false;
  compartimenteSelection: DropdownSelection[];
  ddFirmeSelectionList: DropdownSelection[];
  loggedInUserHasFirma: boolean = false;
  compartimentSub: Subscription;
  compSelectionSub: Subscription;
  locSelectionSub: Subscription;
  firmeSub: Subscription;
  firmaLoggedInUserSub: Subscription;

  constructor(private firmeService: FirmeService,
    private compartimenteService: CompartimenteService,
    private ddStateService: DropdownStateService,
    private locatieService: LocatiiService,
    private toastr: ToastrService,
    private router: Router) {
    this.compartiment = {} as NCompartimentDisplay;
  }

  ngOnInit() {
    this.firmeSub = this.firmeService.firmeSelections$.subscribe((data) => {
      this.ddFirmeSelectionList = data;;
    });

    this.firmaLoggedInUserSub = this.firmeService.firmaLoggedInUser$.subscribe((data) => {
      if (data) {
        this.loggedInUserHasFirma = true;
        this.compartiment.firma = data.Text;
        this.compartiment.idFirma = data.IdFirma;
      }
    });

    this.getLocalitati(this.compartiment.idFirma);
    this.getCompartimente(this.compartiment.idFirma);
  }

  save() {
    this.compartimenteService.add(this.compartiment).subscribe({
      next: () => {
        this.ddStateService.upsertDropdowns();
        this.toastr.success(
          'Compartimentul a fost adăugat cu succes!',
          'Succes!'
        );
        setTimeout(() => {
          this.goToCompartimente();
        }, 500);
      }, error: (err) => {
        this.toastr.error(
          `Compartimentul nu a putut fi adăugat!, ${err.message}`,
          'Eroare!'
        );
      }
    });
  }


  firmaChanged(idFirma: number | null) {
    this.getLocalitati(idFirma);
    this.getCompartimente(idFirma);
  }

  private getLocalitati(filterIdFirma: number | null) {
    this.locSelectionSub = this.locatieService.getLocatiiData().subscribe((data) => {
      if (filterIdFirma) {
        this.locatii = data.filter(lc => lc.idFirma === +filterIdFirma);
      } else {
        this.locatii = data;
      }
    });
  }

  private getCompartimente(filterIdFirma: number | null) {
    this.compSelectionSub = this.ddStateService.appDropdownsSubject$.subscribe((data) => {
      if (filterIdFirma) {
        this.compartimenteSelection = data?.DdCompartimente?.filter(cmp => cmp.IdFirma === +filterIdFirma);
      } else {
        this.compartimenteSelection = data?.DdCompartimente;
      }
    });
  }

  onCancel() {
    this.goToCompartimente();
  }

  private goToCompartimente() {
    this.router.navigate(['/compartimente']);
  }
}
