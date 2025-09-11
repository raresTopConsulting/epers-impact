import { NgModule } from '@angular/core';
import { HeaderModule } from '../../shared/components/general-data-table/header.module';
import { SharedModule } from '../../shared/components/shared.module';
import { EvaluareRoutingModule } from './evaluare-routing.module';
import { ListaSubalterniComponent } from './lista-subalterni/lista-subalterni.component';
import { AutoevaluareComponent } from './evaluari/autoevaluare.component';
import { MentiuniComponent } from './mentiuni/mentiuni.component';
import { EvaluareSubalternComponent } from './evaluari/evaluare-subaltern.component';
import { EvaluareFinalaComponent } from './evaluari/evaluare-finala.component';
import { IsotricEvaluariComponent } from './isotric-evaluari/isotric-evaluari.component';
import { ChartIstoricEvaluariComponent } from './isotric-evaluari/chart-istoric-evaluari/chart-istoric-evaluari.component';
import { IsotricEvaluariPersonaleComponent } from './isotric-evaluari/isotric-evaluari-personale.component';
import { NomenclatoareModule } from '../nomenclatoare/nomenclatoare.module';

@NgModule({
  declarations: [
    ListaSubalterniComponent,
    AutoevaluareComponent,
    EvaluareSubalternComponent,
    EvaluareFinalaComponent,
    MentiuniComponent,
    IsotricEvaluariComponent,
    IsotricEvaluariPersonaleComponent,
    ChartIstoricEvaluariComponent
  ],
  imports: [
    HeaderModule,
    SharedModule,
    EvaluareRoutingModule,
    NomenclatoareModule
  ],
  exports: [
  ],
  providers: [],
})
export class EvaluareModule { }
