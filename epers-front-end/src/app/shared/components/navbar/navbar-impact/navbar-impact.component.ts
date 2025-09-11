import { Component, Input, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
    selector: 'navbar-impact',
    templateUrl: './navbar-impact.component.html',
    standalone: false
})
export class NavbarImactComponent implements OnInit {
  @Input() isUserAuth: boolean;
  @Input() isUserAdmin: boolean;
  @Input() userRoleId: number;
  @Input() userId: number;
  companyName: string;

  constructor() { }

  ngOnInit() {
    this.companyName = environment.companyName;
  }

}
