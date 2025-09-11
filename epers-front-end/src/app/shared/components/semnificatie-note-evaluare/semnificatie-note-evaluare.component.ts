import { Component } from '@angular/core';

export interface SemnificatieNoteEvaluare {
  nota: string;
  semnificatie: string;
}

const CODIFICARE_NOTE: SemnificatieNoteEvaluare[] = [
  {nota: "1 - 1,5", semnificatie: 'Insuficient' },
  {nota: "1,6 - 2,5", semnificatie: 'Necesită efort susținut pentru dezvoltare' },
  {nota: "2,6 - 3,5", semnificatie: 'Necesită dezvoltare' },
  {nota: "3,6 - 4,5", semnificatie: 'Exemplar' },
  {nota: "4,6 - 5", semnificatie: 'Excepțional' },
];

@Component({
    selector: 'semnificatie-note-evaluare',
    templateUrl: './semnificatie-note-evaluare.component.html',
    standalone: false
})
export class SemnificatieNoteEvaluareComponent {
  displayedColumns: string[] = ['nota', 'semnificatie'];
  codificareNote = CODIFICARE_NOTE;
}
