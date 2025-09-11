import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { LoginComponent } from './login/login.component';
import { LogoutComponent } from './logout/logout.component';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { SharedModule } from '../../shared/components/shared.module';
import { RouterModule } from '@angular/router';
import { ForgotPasswordComponent } from '../utilizatori/forgot-password/forgot-password.component';

@NgModule({
  declarations: [
    LoginComponent,
    LogoutComponent,
    UserProfileComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    SharedModule,
    RouterModule.forChild([
      { path: 'login', component: LoginComponent },
      { path: 'forgotPassword', component: ForgotPasswordComponent },
      
    ]),
  ],
  exports: [
    LoginComponent,
    LogoutComponent,
    UserProfileComponent
  ],
})
export class AutentificareModule {}