import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { UntilDestroy } from '@ngneat/until-destroy';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { DropdownSelection } from 'src/app/models/common/Dorpdown';
import { UserCreateModel, UserEditModel } from 'src/app/models/useri/User';
import { FirmeService } from 'src/app/services/nomenclatoare/firme.service';
import { UserStateService } from 'src/app/services/user/user-state.service';
import { DropdownStateService } from 'src/app/states/dropdown/dropdown-state.service';

@UntilDestroy({ checkProperties: true })
@Component({
    selector: 'user-edit',
    templateUrl: './user-create-edit.component.html',
    standalone: false
})
export class UserEditComponent implements OnInit {
  user: UserCreateModel;
  userEdit: UserEditModel;
  userId: number;
  ddLocatii: DropdownSelection[];
  ddPosturi: DropdownSelection[];
  ddCompartimente: DropdownSelection[];
  ddRoluri: DropdownSelection[];
  ddUseri: DropdownSelection[];
  allDdLocatii: DropdownSelection[];
  allDdPosturi: DropdownSelection[];
  allDdCompartimente: DropdownSelection[];
  allddUseri: DropdownSelection[];
  ddFirmeSelectionList: DropdownSelection[];
  errorPassword: boolean = false;
  isEditMode: boolean = true;
  loggedInUserHasFirma: boolean = false;
  firmeSub: Subscription;
  ddSub: Subscription;

  constructor(private ddStateService: DropdownStateService,
    private userService: UserStateService,
    private toastr: ToastrService,
    private firmeService: FirmeService,
    private router: Router,
    private route: ActivatedRoute) {
      
    this.firmeSub = this.firmeService.firmeSelections$.subscribe((data) => {
      this.ddFirmeSelectionList = data;;
    });
    
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
    });
  }

  ngOnInit() {
    this.user = {} as UserCreateModel;

    this.route.params.subscribe(params => {
      this.userId = +params['id'];
      this.userService.get(this.userId).subscribe((data) => {
        this.user = data;
        this.filterDropdowns(data?.idFirma);
      });
    });
  }

  onSubmit() {
    this.userEdit = { ...this.user };
    this.userService.updateUserData(this.userEdit).subscribe({
      next: () => {
        this.toastr.success("Datele utilizatorului au fost modificate cu succes!");
        setTimeout(() => {
          this.router.navigate(['../../'], { relativeTo: this.route });
        }, 500);
      }
    });
  }

  onCancel() {
    this.router.navigate(['../../'], { relativeTo: this.route });
  }

  firmaChanged(idFirma: number | null) {
    this.filterDropdowns(idFirma);
  }

  private filterDropdowns(filterIdFirma: number | null) {
    if (filterIdFirma) {
      this.ddCompartimente = this.allDdCompartimente.filter(x => x.IdFirma === +filterIdFirma);
      this.ddLocatii = this.allDdLocatii.filter(x => x.IdFirma === +filterIdFirma);
      this.ddPosturi = this.allDdPosturi.filter(x => x.IdFirma === +filterIdFirma);
      this.ddUseri = this.allddUseri.filter(x => x.IdFirma === +filterIdFirma);
    }
  }

}
