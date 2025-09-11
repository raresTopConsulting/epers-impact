import { Component, OnInit } from '@angular/core';
import { UntilDestroy } from '@ngneat/until-destroy';
import { Subscription } from 'rxjs';
import { SubalterniDropdown } from 'src/app/models/useri/User';
import { PipService } from 'src/app/services/pip/pip.service';

@UntilDestroy({checkProperties: true})
@Component({
    selector: 'pip',
    templateUrl: './pip.component.html',
    standalone: false
})
export class PipComponent implements OnInit {
  loggedInUserId: number | null;
  listaSubalterniDD: SubalterniDropdown[];
  subalternSelectat: SubalterniDropdown;
  isPipPersonal: boolean;
  isPipSualterni: boolean;

  ddSublaterniSub: Subscription;

  constructor(private pipService: PipService) {
    this.loggedInUserId = localStorage.getItem("user") ? JSON.parse(localStorage.getItem("user"))?.id : null;
  }

  ngOnInit() {
  }

  showPipPersonal() {
    this.isPipPersonal = true;
    this.isPipSualterni = false;
  }

  showPipSubalterni() {
    this.isPipPersonal = false;
    this.isPipSualterni = true;
  }
}
