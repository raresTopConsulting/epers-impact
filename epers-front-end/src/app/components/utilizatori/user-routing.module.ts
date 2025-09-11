import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserListComponent } from './user-list/user-list.component';
import { UserCreateComponent } from './user-create-edit/user-create.component';
import { UserEditComponent } from './user-create-edit/user-edit.component';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { UserDbSetupComponent } from './user-db-setup/user-db-setup.component';
import { ExportEvaluariComponent } from './export-evaluari/export-evaluari.component';

const routes: Routes = [
  { path: '', component: UserListComponent },
  { path: 'add', component: UserCreateComponent },
  { path: 'edit/:id', component: UserEditComponent },
  { path: 'changePassword/:id', component: ChangePasswordComponent },
  { path: 'userDbSetup', component: UserDbSetupComponent }, 
  { path: 'export-evaluari', component: ExportEvaluariComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class UserRoutingModule {}
