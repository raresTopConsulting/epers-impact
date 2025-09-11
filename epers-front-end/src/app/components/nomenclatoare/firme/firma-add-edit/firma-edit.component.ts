import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { NFirme } from 'src/app/models/nomenclatoare/NFirme';
import { FirmeService } from 'src/app/services/nomenclatoare/firme.service';
import { DropdownStateService } from 'src/app/states/dropdown/dropdown-state.service';

@UntilDestroy({ checkProperties: true })
@Component({
    selector: 'firma-edit',
    templateUrl: './firma-add-edit.component.html',
    standalone: false
})
export class FirmaEditComponent implements OnInit {
  firma: NFirme;
  id: number;
  isEditMode: boolean = false;
  firmaSub: Subscription;

  constructor(private firmeService: FirmeService,
    private toastr: ToastrService,
    private ddStateService: DropdownStateService,
    private router: Router,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.params.subscribe((param: Params) => {
      this.id = +param['id'];
      this.firmaSub = this.firmeService.get(this.id).subscribe((firma) => {
        this.firma = firma;
      });
    });
  }
  save() {
    this.firmeService.update(this.firma).subscribe({
      next: () => {
        this.firmeService.upsertDDFirme();
        this.ddStateService.upsertDropdowns();
        this.toastr.success('Firma a fost modificată cu succes!', 'Succes!');
        setTimeout(() => {
          this.goToFirme();
        }, 1000);
      }, error: (err) => {
        this.toastr.error(
          `Firma nu a putut fi modificată!, ${err.message}`,
          'Eroare!'
        );
      }
    });
  }
  onCancel() {
    this.goToFirme();
  }
  private goToFirme() {
    this.router.navigate(['/firme']);
  }

}
