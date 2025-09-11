import { Component, Input, OnInit } from '@angular/core';
import { UntilDestroy } from '@ngneat/until-destroy';
import { Subscription } from 'rxjs';
import { environment } from 'src/environments/environment';

@UntilDestroy({checkProperties: true})
@Component({
    selector: 'navbar-demo',
    templateUrl: './navbar-demo.component.html',
    standalone: false
})
export class NavbarDemoComponent implements OnInit {
  @Input() isUserAuth: boolean;
  @Input() userRoleId: number;
  @Input() isUserAdmin: boolean;
  @Input() userId: number;
  companyName: string;
  userStateServiceSub: Subscription;

  constructor() { }

  ngOnInit() {
    this.companyName = environment.companyName;
  }

}
