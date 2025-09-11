import { Component, Input, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
    selector: 'navbar-crevedia',
    templateUrl: './navbar-crevedia.component.html',
    standalone: false
})
export class NavbarCrevediaComponent implements OnInit {
  @Input() isUserAuth: boolean;
  @Input() userRoleId: number;
  @Input() userId: number;
  companyName: string;

  constructor() { }

  ngOnInit() {
    this.companyName = environment.companyName;
  }
}
