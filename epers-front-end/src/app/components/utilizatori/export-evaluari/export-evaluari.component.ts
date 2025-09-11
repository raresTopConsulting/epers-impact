import { Component } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { Subscription } from 'rxjs';
import { ExportRapoarteService } from 'src/app/services/evaluare/export-rapoarte.service';

@UntilDestroy()
@Component({
    selector: 'export-evaluari',
    templateUrl: './export-evaluari.component.html',
    standalone: false
})
export class ExportEvaluariComponent {
  anul: number;
  idAngajatStart: number;
  idAngajatStop: number;
  years = Array.from({ length: 50 }, (_, i) => 2022 + i);
  isWritingFiles: boolean = false;
  successMessage: string;
  errorMessage: string;
  generateRapEvalSub: Subscription;

  constructor(private exportRapoarteService: ExportRapoarteService) { }

  generateRaportPdfSiExcel() {
    this.isWritingFiles = true;

    this.generateRapEvalSub = this.exportRapoarteService.exortEvaluariToPdfAndExcel(this.anul).pipe(untilDestroyed(this)).subscribe({
      next: (data) => {
        this.isWritingFiles = false;
        this.successMessage = data?.raspuns;
      }, error: (err) => {
        this.errorMessage = err;
        this.isWritingFiles = false;
      }
    });
  }

  generateRaportPdfSiExcelBetween() {
    this.isWritingFiles = true;

    this.generateRapEvalSub = this.exportRapoarteService.exortEvaluariToPdfAndExcelForAngajatiBetween(this.anul, this.idAngajatStart, this.idAngajatStop)
      .pipe(untilDestroyed(this)).subscribe({
        next: (data) => {
          this.isWritingFiles = false;
          this.successMessage = data?.raspuns;
        }, error: (err) => {
          this.errorMessage = err;
          this.isWritingFiles = false;
        }
      });
  }

}
