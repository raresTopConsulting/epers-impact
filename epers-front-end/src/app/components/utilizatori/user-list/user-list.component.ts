import { Component, OnInit, ViewChild } from '@angular/core';
import { Subscription } from 'rxjs';
import { ListaUtilisatoriDisplayModel, ListaUtilizatori } from 'src/app/models/useri/User';
import { ModalDeleteComponent } from 'src/app/shared/modals/modal-delete/modal-delete.component';
import { ActivatedRoute, Router } from '@angular/router';
import { ChangePasswordComponent } from '../change-password/change-password.component';
import { UserStateService } from 'src/app/services/user/user-state.service';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { UntilDestroy } from '@ngneat/until-destroy';
import { DropdownSelection } from 'src/app/models/common/Dorpdown';
import { FirmeService } from 'src/app/services/nomenclatoare/firme.service';
import { HeaderTableFiltersModel } from 'src/app/models/common/HeaderTableFiltersModel';

@UntilDestroy({ checkProperties: true })
@Component({
    selector: 'user-list',
    templateUrl: './user-list.component.html',
    standalone: false
})

export class UserListComponent implements OnInit {
  @ViewChild(ModalDeleteComponent) deleteModal: ModalDeleteComponent;
  listaUtilizatori: ListaUtilizatori[];
  utilizatoriDisplayModel: ListaUtilisatoriDisplayModel;
  pages: number = 0;
  currentPage: number = 1;
  itemsPerPage: number = 10;
  filtru: string = '';
  filtruIdFirma: number | null = null;
  ddFirme: DropdownSelection[];
  minId: number = 0;
  maxId: number = 0;
  isSync: boolean = false;
  idFirmaLoggedInUser: number | null;

  firmeSub: Subscription = new Subscription();
  registerSincronSub: Subscription = new Subscription();
  userSub: Subscription = new Subscription();
  userPassSub: Subscription = new Subscription();
  tableFiltersSub: Subscription = new Subscription();

  constructor(private userService: UserStateService,
    private router: Router,
    private route: ActivatedRoute,
    private firmeService: FirmeService,
    private dialog: MatDialog) {

    this.getDDFirme();
    
    this.tableFiltersSub = this.userService.tableFiltersObs$.subscribe((data: HeaderTableFiltersModel) => {
      this.filtru = data?.filter;
      this.filtruIdFirma = data?.filterFirma;
      this.currentPage = data?.currentPage;
      this.itemsPerPage = data?.itemsPerPage;
    });

    this.getListaUtilizatori(this.currentPage, this.itemsPerPage, this.filtru, this.filtruIdFirma);
    this.idFirmaLoggedInUser = JSON.parse(localStorage.getItem('user'))?.idFirma;
  }

  ngOnInit(): void {
    this.isSync = false;

  }

  getListaUtilizatori(page: number, itemsPerPage: number, filter: string, idFrima: number | null) {
    this.userService.listaUtilizatori(page, itemsPerPage, filter, idFrima);

    this.userSub = this.userService.userListDisplayModelObs$.subscribe(response => {
      this.listaUtilizatori = response?.utilizatori;
      this.pages = response?.pages;
      this.currentPage = response?.currentPage;
    });
  }

  nextPage(nxtPg: number) {
    this.getListaUtilizatori(nxtPg, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  previousPage(prvPg: number) {
    this.getListaUtilizatori(prvPg, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  lastPage(lstPg: number) {
    this.getListaUtilizatori(lstPg, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  changeItemsPerPage(itmsPg: number) {
    this.currentPage = 1;
    this.itemsPerPage = itmsPg;
    this.getListaUtilizatori(this.currentPage, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  filter(fltr: string) {
    this.filtru = fltr;
    this.currentPage = 1;
    this.getListaUtilizatori(this.currentPage, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  openDeleteDialog(id: number) {
    const dialogRef = this.dialog.open(ModalDeleteComponent, {
      data: { entityName: 'Utilizatorul', entityId: id },
    });
    dialogRef.afterClosed().subscribe(() => {
      this.getListaUtilizatori(this.currentPage, this.itemsPerPage, this.filtru, this.filtruIdFirma);
    });
  }

  openChangePasswordDialog(id: number, username: string) {
    const changePassDialogConfig = new MatDialogConfig;
    changePassDialogConfig.disableClose = false;
    changePassDialogConfig.autoFocus = false;

    changePassDialogConfig.data = {
      userId: id,
      newPass: '',
      confPass: '',
      username: username
    };

    this.dialog.open(ChangePasswordComponent, changePassDialogConfig);
    const dialogRef = this.dialog.open(ChangePasswordComponent, changePassDialogConfig);

    dialogRef.afterClosed().subscribe((passChangeReq) => {
      if (passChangeReq && passChangeReq.newPass && passChangeReq.confPass) {
        console.log("succes");
      }
    });
  }

  navToAdd() {
    this.router.navigate(['./add'], { relativeTo: this.route });
  }

  filterFirma(idFirma: number) {
    this.filtruIdFirma = idFirma;
    this.currentPage = 1;
    this.pages = 0;
    this.getListaUtilizatori(this.currentPage, this.itemsPerPage, this.filtru, this.filtruIdFirma);
  }

  private getDDFirme() {
    this.firmeSub = this.firmeService.firmeSelections$.subscribe((data) => {
      this.ddFirme = data;
    });
  }

  registerFromSincron() {
    this.isSync = true;
    this.registerSincronSub = this.userService.registerFromSincron(this.minId, this.maxId).subscribe(() => {
      this.isSync = false;
    });
  }

}
