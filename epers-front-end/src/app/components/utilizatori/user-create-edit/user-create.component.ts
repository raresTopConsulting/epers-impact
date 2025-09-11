import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { UntilDestroy } from '@ngneat/until-destroy';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { DropdownSelection } from 'src/app/models/common/Dorpdown';
import { UserCreateModel } from 'src/app/models/useri/User';
import { FirmeService } from 'src/app/services/nomenclatoare/firme.service';
import { UserStateService } from 'src/app/services/user/user-state.service';
import { DropdownStateService } from 'src/app/states/dropdown/dropdown-state.service';

@UntilDestroy({ checkProperties: true })
@Component({
    selector: 'user-create',
    templateUrl: './user-create-edit.component.html',
    standalone: false
})
export class UserCreateComponent implements OnInit {
  user: UserCreateModel;
  ddLocatii: DropdownSelection[];
  ddPosturi: DropdownSelection[];
  ddCompartimente: DropdownSelection[];
  ddRoluri: DropdownSelection[];
  ddUseri: DropdownSelection[];
  allDdLocatii: DropdownSelection[];
  allDdPosturi: DropdownSelection[];
  allDdCompartimente: DropdownSelection[];
  allddUseri: DropdownSelection[];
  errorPassword: boolean = false;
  isEditMode: boolean = false;
  loggedInUserHasFirma: boolean = false;
  ddFirmeSelectionList: DropdownSelection[];
  firmeSub: Subscription;
  firmaLoggedInUserSub: Subscription;
  ddSub: Subscription;

  constructor(private ddStateService: DropdownStateService,
    private userService: UserStateService,
    private toastr: ToastrService,
    private router: Router,
    private firmeService: FirmeService,
    private route: ActivatedRoute) {

    this.user = {} as UserCreateModel;

    this.firmeSub = this.firmeService.firmeSelections$.subscribe((data) => {
      this.ddFirmeSelectionList = data;;
    });

    this.firmaLoggedInUserSub = this.firmeService.firmaLoggedInUser$.subscribe((data) => {
      if (data) {
        this.loggedInUserHasFirma = true;
        this.user.idFirma = data.IdFirma;
      }
    });
  }

  ngOnInit(): void {
    this.ddSub = this.ddStateService.appDropdownsSubject$.subscribe((data) => {
      this.ddLocatii = data?.DdLocatii;
      this.ddPosturi = data?.DdPosturi;
      this.ddCompartimente = data?.DdCompartimente;
      this.ddUseri = data?.DdUseri;
      this.ddRoluri = data?.DdRoluri;
      this.allDdLocatii = data?.DdLocatii;
      this.allDdPosturi = data?.DdPosturi;
      this.allDdCompartimente = data?.DdCompartimente;
      this.allddUseri = data?.DdUseri;
      this.filterDropdowns(this.user.idFirma);
    });
  }

  onSubmit() {
    if (this.user.password != this.user.confirmPassword) {
      this.errorPassword = true;
    } else {
      this.errorPassword = false;
    }
    if (!this.errorPassword) {
      this.userService.register(this.user).subscribe({
        next: () => {
          this.toastr.success('Utilizatorul a fost creat cu succes!');
          setTimeout(() => {
            this.router.navigate(['../../'], { relativeTo: this.route });
          }, 1000);
        }, error: (err) => {
          this.toastr.error(err.error);
        }
      });
    }
  }

  onCancel() {
    this.router.navigate(['../'], { relativeTo: this.route });
  }

  firmaChanged(idFirma: number | null) {
    this.filterDropdowns(idFirma);
  }

  private filterDropdowns(filterIdFirma: number | null) {
    if (filterIdFirma) {
      this.ddCompartimente = this.allDdCompartimente?.filter(x => x.IdFirma === +filterIdFirma);
      this.ddLocatii = this.allDdLocatii?.filter(x => x.IdFirma === +filterIdFirma);
      this.ddPosturi = this.allDdPosturi?.filter(x => x.IdFirma === +filterIdFirma);
      this.ddUseri = this.allddUseri?.filter(x => x.IdFirma === +filterIdFirma);
    }
  }

}
