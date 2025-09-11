import { Component, OnInit } from '@angular/core';
import { UntilDestroy } from '@ngneat/until-destroy';
import { Subscription } from 'rxjs';
import { UserDbSetupService } from 'src/app/services/user/user-db-setup.service';
import { environment } from 'src/environments/environment';

@UntilDestroy({checkProperties: true})
@Component({
    selector: 'app-user-db-setup',
    templateUrl: './user-db-setup.component.html',
    standalone: false
})
export class UserDbSetupComponent implements OnInit {
  message: string;
  updateUserTableInProgress: boolean = false;
  updateUserHierarchyInProgress: boolean = false;
  usesOldUserTalbe: boolean = false;

  addAdminSub: Subscription;
  updateUserTableSub: Subscription;
  updateUserTableHierarchySub: Subscription;
  addIdFirmaToEvaluareTableSub: Subscription;

  constructor(private userDbSetupService: UserDbSetupService) {
    this.usesOldUserTalbe = environment.usesOldUserTalbe;
  }

  ngOnInit() {
  }

  addIdFirmaToEvaluareTable() {
    this.addIdFirmaToEvaluareTableSub = this.userDbSetupService.addIdFirmaToEvaluareCompetente().subscribe((resp: any) =>{
      this.message = resp?.message;
    });
  }

  addAdmin() {
    this.addAdminSub = this.userDbSetupService.addAdmin().subscribe(() => {
      this.message = "Utilizatorul admin a fost adaugat cu succes";
    });
  }

  updateUserTable() {
    this.updateUserTableInProgress = true;
    this.updateUserTableSub = this.userDbSetupService.updateUserDataFromImport24().subscribe(() => {
      this.message = "Tabela noua de Utilizatori a fost populata";
      this.updateUserTableInProgress = false;
    });
  }

  setUserHierarchy() {
    this.updateUserHierarchyInProgress = true;
    this.updateUserTableHierarchySub = this.userDbSetupService.setHiearchy().subscribe(() => {
      this.message = "Ierarhia a fost setata";
      this.updateUserHierarchyInProgress = false;
    })
  }


}
