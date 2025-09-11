import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ListaSubalterniConcluziiComponent } from './lista-subalterni-concluzii/lista-subalterni-concluzii.component';
import { ConcluzieAddComponent } from './concluzie-add/concluzie-add.component';
import { IstoricConcluziiComponent } from './istoric-concluzii/istoric-concluzii.component';

const routes: Routes = [
  { path: 'listaSubalterni', component: ListaSubalterniConcluziiComponent },
  { path: 'add/:id', component: ConcluzieAddComponent },
  { path: 'istoric/:id', component: IstoricConcluziiComponent },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ConcluziiRoutingModule {}
