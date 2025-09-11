import { NgModule } from '@angular/core';
import { HeaderModule } from '../../shared/components/general-data-table/header.module';
import { SharedModule } from '../../shared/components/shared.module';
import { UserListComponent } from './user-list/user-list.component';
import { UserRoutingModule } from './user-routing.module';
import { UserEditComponent } from './user-create-edit/user-edit.component';
import { UserCreateComponent } from './user-create-edit/user-create.component';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { UserDbSetupComponent } from './user-db-setup/user-db-setup.component';
import { ExportEvaluariComponent } from './export-evaluari/export-evaluari.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';

@NgModule({
  declarations: [
    UserListComponent,
    UserEditComponent,
    UserCreateComponent,
    ChangePasswordComponent,
    UserDbSetupComponent,
    ExportEvaluariComponent,
    ForgotPasswordComponent
  ],
  imports: [
    HeaderModule,
    SharedModule,
    UserRoutingModule,
  ],
  exports: [
  ],
  providers: [],
})
export class UserModule { }
