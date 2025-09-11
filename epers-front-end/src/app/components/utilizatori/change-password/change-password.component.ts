import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UntilDestroy } from '@ngneat/until-destroy';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { PasswordChange, UserCreateModel } from 'src/app/models/useri/User';
import { UserPasswordManagementService } from 'src/app/services/user/user-password-management.service';
import { UserStateService } from 'src/app/services/user/user-state.service';

@UntilDestroy({checkProperties: true})
@Component({
    selector: 'change-password',
    templateUrl: './change-password.component.html',
    standalone: false
})
export class ChangePasswordComponent implements OnInit {
  passwordChange: PasswordChange;
  userId: number;
  user: UserCreateModel;
  userPassSub: Subscription;
  userServiceSub: Subscription;

  constructor(private userPasswordService: UserPasswordManagementService,
    private toastr: ToastrService,
    private userService: UserStateService,
    private router: Router,
    private route: ActivatedRoute) {
    this.passwordChange = {} as PasswordChange;
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.userId = +params['id'];
      this.passwordChange.userId = this.userId;
      this.userServiceSub = this.userService.get(this.userId).subscribe((data) => {
        if (data) {
          this.user = data;
          this.passwordChange.username = data.username;
        }
      });
    });
  }

  onChangePassword() {
    this.userPassSub = this.userPasswordService.changePassword(this.passwordChange).subscribe({
      next: () => {
        this.toastr.success("Parola a fost schimbată cu succes!");
        setTimeout(() => {
          this.router.navigate(['/']);
        }, 1000);

      }, error: (err) => {
        this.toastr.error(err?.error?.message);
      }
    });
  }
}

