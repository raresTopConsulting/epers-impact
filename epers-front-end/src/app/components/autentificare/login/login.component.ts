import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { LoggedInUserData, UserAuthenticationRequest } from 'src/app/models/useri/User';
import { AutentificareService } from 'src/app/services/autentificare/autentificare.service';

@Component({
  selector: 'login',
  templateUrl: './login.component.html',
  standalone: false
})
export class LoginComponent {
  user: UserAuthenticationRequest = {} as UserAuthenticationRequest;
  errorMessage: string;
  isLoginInProgress: boolean = false;

  constructor(private authService: AutentificareService,
    private toastr: ToastrService,
    private router: Router) { }

  onLogin() {
    this.isLoginInProgress = true;
    this.authService.login(this.user).subscribe({
      next: (data: LoggedInUserData) => {
        if (data && data?.id) {
          this.router.navigate(['./']);
        } else {
          this.toastr.error("Utilizatorul sau parola nu sunt corecte!");
        }
        this.isLoginInProgress = false;
      },
      error: (err) => {
        this.isLoginInProgress = false;
        if (err.status === 401 && err.error?.code === 'INVALID_CREDENTIALS') {
          this.toastr.error(err.error.message);
        } else if (err.status === 0) {
          this.toastr.error("Nu se poate contacta serverul. Verificați conexiunea.");
        } else {
          this.toastr.error("A apărut o eroare neașteptată.");
          console.error("Login error:", err);
        }
      },
    });
  }

  onCancel() {
    this.user.username = 'nume_utilizator';
    this.user.password = 'parola';
  }
}
