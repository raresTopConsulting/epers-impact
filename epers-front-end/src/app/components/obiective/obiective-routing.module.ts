import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ListaSubalterniObComponent } from './lista-subalterni-ob/lista-subalterni-ob.component';
import { SetareObiectiveComponent } from './setare-obiective/setare-obiective.component';
import { ObiectiveActualeComponent } from './obiective-actuale/obiective-actuale.component';
import { IsotoricObiectiveComponent } from './isotoric-obiective/isotoric-obiective.component';
import { EvaluareObiectiveComponent } from './evaluare-obiective/evaluare-obiective.component';

const routes: Routes = [
  { path: 'listaSubalterni', component: ListaSubalterniObComponent },
  { path: 'setareObiective/:id', component: SetareObiectiveComponent },
  { path: 'actuale/:id', component: ObiectiveActualeComponent },
  { path: 'istoric/:id', component: IsotoricObiectiveComponent },
  { path: 'evaluare/:id', component: EvaluareObiectiveComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ObiectiveRoutingModule {}
