import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { UserPasswordManagementService } from 'src/app/services/user/user-password-management.service';

@Component({
    selector: 'forgot-password',
    templateUrl: './forgot-password.component.html',
    standalone: false
})
export class ForgotPasswordComponent {
  emailAddress: string;
  successMessage: string;

  constructor(private userPasswordMngmntService: UserPasswordManagementService,
    private toastr: ToastrService
  ) { }

  sendEmailResetPassword() {
    this.userPasswordMngmntService.forgotPassword(this.emailAddress).subscribe({
      next: (data: any) => {
        this.toastr.success(data?.message)
      },
      error: (err) => {
        this.toastr.error(err?.message);
      }
    });
  }

}
