import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { UntilDestroy } from '@ngneat/until-destroy';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { DropdownSelection } from 'src/app/models/common/Dorpdown';
import { NCursuri } from 'src/app/models/nomenclatoare/NCursuri';
import { CursuriService } from 'src/app/services/nomenclatoare/cursuri.service';
import { FirmeService } from 'src/app/services/nomenclatoare/firme.service';
import { DropdownStateService } from 'src/app/states/dropdown/dropdown-state.service';

@UntilDestroy({checkProperties: true})
@Component({
    selector: 'cursuri-add',
    templateUrl: './cursuri-add-edit.component.html',
    standalone: false
})
export class CursuriAddComponent {
  curs: NCursuri;
  isEditMode: boolean = false;
  loggedInUserHasFirma: boolean = false;
  ddFirmeSelectionList: DropdownSelection[];
  firmeSub: Subscription;
  firmaLoggedInUserSub: Subscription;

  constructor(private cursuriService: CursuriService,
    private ddStateService: DropdownStateService,
    private firmeService: FirmeService,
    private toastr: ToastrService,
    private router: Router) {

    this.curs = {} as NCursuri;
    this.firmeSub = this.firmeService.firmeSelections$.subscribe((data) => {
      this.ddFirmeSelectionList = data;;
    });

    this.firmaLoggedInUserSub = this.firmeService.firmaLoggedInUser$.subscribe((data) => {
      if (data) {
        this.loggedInUserHasFirma = true;
        this.curs.firma = data.Text;
        this.curs.idFirma = data.IdFirma;
      }
    });
  }

  save() {
    this.cursuriService.add(this.curs).subscribe({
      next: () => {
        this.ddStateService.upsertDropdowns();
        this.toastr.success(
          'Cursul a fost adăugat cu succes!',
          'Succes!'
        );
        setTimeout(() => {
          this.goTocursuri();
        }, 500);
      }, error: (error) => {
        this.toastr.error(
          `cursul nu a putut fi adăugat!, ${error.message}`,
          'Eroare!'
        );
      }
    });
  }

  onCancel() {
    this.goTocursuri();
  }

  private goTocursuri() {
    this.router.navigate(['/cursuri']);
  }

}
