import { Component, OnInit } from '@angular/core';
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
    selector: 'app-divizie-edit',
    templateUrl: './divizie-add-edit.component.html',
    standalone: false
})
export class DivizieEditComponent implements OnInit {
  divizie: NDivizii = {} as NDivizii;
  isEditMode: boolean = true;
  private id: number;
  loggedInUserHasFirma: boolean = false;
  ddFirmeSelectionList: DropdownSelection[];
  firmeSub: Subscription;
  diviziiSub: Subscription;
  firmaLoggedInUserSub: Subscription;

  constructor(private firmeService: FirmeService,
    private diviziiService: DiviziiService,
    private ddStateService: DropdownStateService,
    private toastr: ToastrService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.firmeSub = this.firmeService.firmeSelections$.subscribe((data) => {
      this.ddFirmeSelectionList = data;;
    });
  }

  ngOnInit() {
    this.route.params.subscribe((params) => {
      this.id = +params['id'];
      this.diviziiSub = this.diviziiService.get(this.id).subscribe((divizie) => {
          this.divizie = divizie;
        });
    });
    this.firmaLoggedInUserSub = this.firmeService.firmaLoggedInUser$.subscribe((data) => {
      if (data) {
        this.loggedInUserHasFirma = true;
      }
    });
  }

  save() {
    this.diviziiService.update(this.divizie).subscribe({
      next: () => {
        this.ddStateService.upsertDropdowns();
        this.toastr.success('Divizia a fost modificată cu succes!', 'Succes!');
        setTimeout(() => {
          this.goToDivizii();
        }, 1000);
      }, error: (err) => {
        this.toastr.error(
          `Divizia nu a putut fi modificată!, ${err.message}`,
          'Eroare!'
        );
      }
    });
  }

  onCancel() {
    this.goToDivizii();
  }

  private goToDivizii() {
    this.router.navigate(['/divizii']);
  }

}
