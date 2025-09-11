import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UntilDestroy } from '@ngneat/until-destroy';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { DropdownSelection } from 'src/app/models/common/Dorpdown';
import { NDivizii } from 'src/app/models/nomenclatoare/NDvizii';
import { DiviziiService } from 'src/app/services/nomenclatoare/divizii.service';
import { FirmeService } from 'src/app/services/nomenclatoare/firme.service';
import { DropdownStateService } from 'src/app/states/dropdown/dropdown-state.service';

@UntilDestroy({checkProperties: true})
@Component({
    selector: 'divizie-add',
    templateUrl: './divizie-add-edit.component.html',
    standalone: false
})
export class DivizieAddComponent {
  divizie: NDivizii;
  isEditMode: boolean = false;
  loggedInUserHasFirma: boolean = false;
  ddFirmeSelectionList: DropdownSelection[];
  firmeSub: Subscription;
  firmaLoggedInUserSub: Subscription;

  constructor(private firmeService: FirmeService,
    private divizieService: DiviziiService,
    private toastr: ToastrService,
    private ddStateService: DropdownStateService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.divizie = {
      id: 0,
      denumire: '',
      descriere: '',
      dataIn: null,
      dataSf: null,
      activ: false,
      updateDate: null,
      updateId: "",
      firma: '',
      idFirma: null
    };
    this.firmeSub = this.firmeService.firmeSelections$.subscribe((data) => {
      this.ddFirmeSelectionList = data;;
    });

    this.firmaLoggedInUserSub = this.firmeService.firmaLoggedInUser$.subscribe((data) => {
      if (data) {
        this.loggedInUserHasFirma = true;
        this.divizie.firma = data.Text;
        this.divizie.idFirma = data.IdFirma;
      }
    });
  }

  save() {
    this.divizieService.add(this.divizie).subscribe({
      next: () => {
        this.ddStateService.upsertDropdowns();
        this.toastr.success('Divizia a fost adaugata cu succes!', 'Succes!');
        setTimeout(() => {
          this.goToDivizii();
        }, 500);
      }, error: (err) => {
        this.toastr.error(
          `Divizia nu a putut fi adaugata!, ${err.message}`,
          'Eroare!'
        );
      }
    });
  }

  onCancel() {
    this.goToDivizii();
  }

  private goToDivizii() {
    this.router.navigate(['/divizii'], { relativeTo: this.route });
  }

}
