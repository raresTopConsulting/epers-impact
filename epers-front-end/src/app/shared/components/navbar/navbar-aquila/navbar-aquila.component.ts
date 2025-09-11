import { Component, Input, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
    selector: 'navbar-aquila',
    templateUrl: './navbar-aquila.component.html',
    standalone: false
})
export class NavbarAquilaComponent implements OnInit {
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
