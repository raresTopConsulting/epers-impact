import { Component, Input } from '@angular/core';

@Component({
    selector: 'save-button',
    templateUrl: './save-button.component.html',
    standalone: false
})
export class SaveButtonComponent {
  @Input() disabled: boolean;

  constructor() { }

}
