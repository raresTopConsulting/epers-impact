import { Component, Input, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
    selector: 'navbar-tvr',
    templateUrl: './navbar-tvr.component.html',
    standalone: false
})
export class NavbarTvrComponent implements OnInit {
  @Input() isUserAuth: boolean;
  @Input() userRoleId: number;
  @Input() userId: number;
  loggedInUserIdFirma: number | null;
  companyName: string;

  constructor() { }

  ngOnInit() {
    this.companyName = environment.companyName;
    this.loggedInUserIdFirma = JSON.parse(localStorage.getItem('user'))?.idFirma;
  }

}
