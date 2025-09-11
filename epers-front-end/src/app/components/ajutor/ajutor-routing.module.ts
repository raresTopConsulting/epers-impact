import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DictionareExplicativeCompetenteComponent } from './dictionare-explicative-competente/dictionare-explicative-competente.component';
import { GhidUtilizareComponent } from './ghid-utilizare/ghid-utilizare.component';
import { GhidDezvoltareCompetenteComponent } from './ghid-dezvoltare-competente/ghid-dezvoltare-competente.component';
import { GhidUtilizareVideoComponent } from './ghid-utilizare-video/ghid-utilizare-video.component';
import { ProceduraEvalPerformanteComponent } from './procedura-eval-performante/procedura-eval-performante.component';
import { SetareaEvaluareaObiectivelorComponent } from './setarea-evaluarea-obiectivelor/setarea-evaluarea-obiectivelor.component';
import { ProceduraEvaluareComponent } from './procedura-evaluare/procedura-evaluare.component';


const routes: Routes = [
  { path: 'dictionareExplicative', component: DictionareExplicativeCompetenteComponent },
  { path: 'ghidUtilizare', component: GhidUtilizareComponent },
  { path: 'ghidDezvoltare', component: GhidDezvoltareCompetenteComponent },
  { path: 'ghidUtilizareVideo', component: GhidUtilizareVideoComponent },
  { path: 'proceduraEvalPerformante', component: ProceduraEvalPerformanteComponent },
  { path: 'setareaEvaluareObiectivelor', component: SetareaEvaluareaObiectivelorComponent },
  { path: 'proceduraEvaluare', component: ProceduraEvaluareComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AjutorRoutingModule {}
