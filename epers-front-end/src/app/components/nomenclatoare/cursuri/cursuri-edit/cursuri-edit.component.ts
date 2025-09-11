import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
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
    selector: 'cursuri-edit',
    templateUrl: './cursuri-add-edit.component.html',
    standalone: false
})
export class CursuriEditComponent implements OnInit {
  curs: NCursuri;
  isEditMode: boolean = true;
  ddFirmeSelectionList: DropdownSelection[];
  private id: number;
  loggedInUserHasFirma: boolean = false;
  cursuriSub: Subscription;
  firmeSub: Subscription;
  firmaLoggedInUserSub: Subscription;

  constructor(private cursuriService: CursuriService,
    private toastr: ToastrService,
    private ddStateService: DropdownStateService,
    private firmeService: FirmeService,
    private router: Router,
    private route: ActivatedRoute) {
      this.firmeSub = this.firmeService.firmeSelections$.subscribe((data) => {
        this.ddFirmeSelectionList = data;;
      });

      this.firmaLoggedInUserSub = this.firmeService.firmaLoggedInUser$.subscribe((data) => {
        if (data) {
          this.loggedInUserHasFirma = true;
        }
      });
    }

  ngOnInit() {
    this.route.params.subscribe((params) => {
      this.id = +params['id'];
      this.cursuriSub = this.cursuriService.get(this.id)
        .subscribe((resp) => {
          this.curs = resp;
        });
    });
  }

  save() {
    this.cursuriService.update(this.curs).subscribe({
      next: () => {
        this.ddStateService.upsertDropdowns();
        this.toastr.success(
          'Cursul a fost modificat cu succes!',
          'Succes!'
        );
        setTimeout(() => {
          this.goToCursuri();
        }, 1000);
      },error: (error) => {
        this.toastr.error(
          `Cursul nu a putut fi modificat!, ${error.message}`,
          'Eroare!'
        );
      }
    });
  }

  onCancel() {
    this.goToCursuri();
  }

  private goToCursuri() {
    this.router.navigate(['/cursuri']);
  }
}
