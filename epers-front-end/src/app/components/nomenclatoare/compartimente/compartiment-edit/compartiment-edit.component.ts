import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { UntilDestroy } from '@ngneat/until-destroy';
import { ToastrService } from 'ngx-toastr';
import { Subscription, tap } from 'rxjs';
import { DropdownSelection } from 'src/app/models/common/Dorpdown';
import { NCompartimentDisplay } from 'src/app/models/nomenclatoare/NCompartimente';
import { NLocatii } from 'src/app/models/nomenclatoare/NLocatii';
import { CompartimenteService } from 'src/app/services/nomenclatoare/compartimente.service';
import { FirmeService } from 'src/app/services/nomenclatoare/firme.service';
import { LocatiiService } from 'src/app/services/nomenclatoare/locatii.service';
import { DropdownStateService } from 'src/app/states/dropdown/dropdown-state.service';

@UntilDestroy({ checkProperties: true })
@Component({
    selector: 'compartiment-edit',
    templateUrl: './compartiment-add-edit.component.html',
    standalone: false
})
export class CompartimentEditComponent implements OnInit {
  compartiment: NCompartimentDisplay;
  locatii: NLocatii[];
  isEditMode: boolean = true;
  compartimenteSelection: DropdownSelection[];
  ddFirmeSelectionList: DropdownSelection[];
  private id: number;
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

    this.route.params.subscribe((params) => {
      this.id = +params['id'];
      this.compartimentSub = this.compartimenteService.get(this.id).pipe(tap(resp => {
        this.compartiment = resp;
      })).subscribe(() => {
        this.getLocalitati(this.compartiment?.idFirma);
        this.getCompartimente(this.compartiment?.idFirma);
      });
    });
  }

  save() {
    this.compartimenteService.update(this.compartiment).subscribe({
      next: () => {
        this.ddStateService.upsertDropdowns();
        this.toastr.success(
          'Compartimentul a fost modificat cu succes!',
          'Succes!'
        );
        setTimeout(() => {
          this.goToCompartimente();
        }, 1000);
      }, error: (err) => {
        this.toastr.error(
          `Compartimentul nu a putut fi modificat!, ${err.message}`,
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
        const idCompartimentSelectat = this.compartiment.id;
        let selectieCompartimente = data?.DdCompartimente.filter(cmp => cmp.IdFirma === +filterIdFirma && cmp?.Id !== idCompartimentSelectat);
        this.compartimenteSelection = selectieCompartimente

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
