import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NFirme } from 'src/app/models/nomenclatoare/NFirme';
import { FirmeService } from 'src/app/services/nomenclatoare/firme.service';
import { DropdownStateService } from 'src/app/states/dropdown/dropdown-state.service';

@Component({
    selector: 'firma-add',
    templateUrl: './firma-add-edit.component.html',
    standalone: false
})
export class FirmaAddComponent implements OnInit {
  firma: NFirme;
  isEditMode: boolean = false;

  constructor(private firmeService: FirmeService,
    private toastr: ToastrService,
    private router: Router,
    private ddStateService: DropdownStateService,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.firma = {} as NFirme;
  }

  save() {
    this.firmeService.add(this.firma).subscribe({
      next: () => {
        this.firmeService.upsertDDFirme();
        this.ddStateService.upsertDropdowns();
        this.toastr.success('Firma a fost adaugata cu succes!', 'Succes!');
        setTimeout(() => {
          this.goToFirme();
          this.firmeService.upsertDDFirme();
        }, 500);
      },
      error: (err) => {
        this.toastr.error(
          `Firma nu a putut fi adaugata!, ${err.message}`,
          'Eroare!'
        );
      }
    });
  }

  onCancel() {
    this.goToFirme();
  }

  private goToFirme() {
    this.router.navigate(['/firme'], { relativeTo: this.route });
  }

}
