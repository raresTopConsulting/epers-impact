import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ListaSubalterniCalificatiPipComponent } from './lista-subalterni-calificati-pip/lista-subalterni-calificati-pip.component';
import { ListaSubalterniWithPipComponent } from './lista-subalterni-with-pip/lista-subalterni-with-pip.component';
import { IstoricListaSubalterniWithPipComponent } from './istoric-lista-subalterni-with-pip/istoric-lista-subalterni-with-pip.component';

const routes: Routes = [
  { path: 'pip/lista-subalterni-calificati-pip', component: ListaSubalterniCalificatiPipComponent },
  { path: 'pip/lista-subalterni-with-pip', component: ListaSubalterniWithPipComponent },
  { path: 'pip/istoric-lista-subalterni-with-pip', component: IstoricListaSubalterniWithPipComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PipRoutingModule { }
