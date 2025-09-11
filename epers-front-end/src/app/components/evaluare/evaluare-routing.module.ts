import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ListaSubalterniComponent } from './lista-subalterni/lista-subalterni.component';
import { AutoevaluareComponent } from './evaluari/autoevaluare.component';
import { MentiuniComponent } from './mentiuni/mentiuni.component';
import { EvaluareSubalternComponent } from './evaluari/evaluare-subaltern.component';
import { EvaluareFinalaComponent } from './evaluari/evaluare-finala.component';
import { IsotricEvaluariComponent } from './isotric-evaluari/isotric-evaluari.component';
import { IsotricEvaluariPersonaleComponent } from './isotric-evaluari/isotric-evaluari-personale.component';

const routes: Routes = [
  { path: 'listaSubalterni', component: ListaSubalterniComponent },
  { path: 'autoevaluare', component: AutoevaluareComponent },
  { path: 'evaluareSubaltern/:id', component: EvaluareSubalternComponent },
  { path: 'evaluareFinala/:id', component: EvaluareFinalaComponent },
  { path: 'mentiuni/:id', component: MentiuniComponent },
  { path: 'istoric/:id', component: IsotricEvaluariComponent },
  { path: 'istoricEvPersonala', component: IsotricEvaluariPersonaleComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class EvaluareRoutingModule {}
