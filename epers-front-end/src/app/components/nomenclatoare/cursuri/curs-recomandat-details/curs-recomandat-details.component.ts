import { Component, Input, OnInit } from '@angular/core';
import { UntilDestroy } from '@ngneat/until-destroy';
import { Subscription } from 'rxjs';
import { NCursuri } from 'src/app/models/nomenclatoare/NCursuri';
import { CursuriService } from 'src/app/services/nomenclatoare/cursuri.service';

@UntilDestroy({checkProperties: true})
@Component({
    selector: 'curs-recomandat-details',
    templateUrl: './curs-recomandat-details.component.html',
    standalone: false
})
export class CursRecomandatDetailsComponent implements OnInit {
  @Input() idCurs: number;
  curs: NCursuri;
  isEditMode: boolean = false;
  private cursSub: Subscription;
  
  constructor(private cursuriService: CursuriService) { }

  ngOnInit() {
    this.cursSub = this.cursuriService.get(this.idCurs).subscribe((data) => {
      this.curs = data;
    });
  }

}
