import { NgModule } from '@angular/core';
import { HeaderModule } from '../../shared/components/general-data-table/header.module';
import { SharedModule } from '../../shared/components/shared.module';
import { DictionareExplicativeCompetenteComponent } from './dictionare-explicative-competente/dictionare-explicative-competente.component';
import { GhidDezvoltareCompetenteComponent } from './ghid-dezvoltare-competente/ghid-dezvoltare-competente.component';
import { GhidUtilizareComponent } from './ghid-utilizare/ghid-utilizare.component';
import { GhidUtilizareVideoComponent } from './ghid-utilizare-video/ghid-utilizare-video.component';
import { ProceduraEvalPerformanteComponent } from './procedura-eval-performante/procedura-eval-performante.component';
import { SetareaEvaluareaObiectivelorComponent } from './setarea-evaluarea-obiectivelor/setarea-evaluarea-obiectivelor.component';
import { AjutorRoutingModule } from './ajutor-routing.module';
import { ProceduraEvaluareComponent } from './procedura-evaluare/procedura-evaluare.component';

@NgModule({
  declarations: [
    DictionareExplicativeCompetenteComponent,
    GhidDezvoltareCompetenteComponent,
    GhidUtilizareComponent,
    GhidUtilizareVideoComponent,
    ProceduraEvalPerformanteComponent,
    SetareaEvaluareaObiectivelorComponent,
    ProceduraEvaluareComponent
  ],
  imports: [
    HeaderModule,
    SharedModule,
    AjutorRoutingModule
  ],
  exports: [
  ],
  providers: [],
})
export class AjutorModule { }
