import { Component, Input } from '@angular/core';
import { Concluzie } from 'src/app/models/concluzii/Concluzie';

@Component({
    selector: 'concluzii-details',
    templateUrl: './concluzii-details.component.html',
    standalone: false
})
export class ConcluziiDetailsComponent {
  @Input() concluzie: Concluzie;
}
