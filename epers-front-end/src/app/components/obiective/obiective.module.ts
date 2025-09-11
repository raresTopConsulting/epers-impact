import { NgModule } from '@angular/core';
import { HeaderModule } from '../../shared/components/general-data-table/header.module';
import { SharedModule } from '../../shared/components/shared.module';
import { ListaSubalterniObComponent } from './lista-subalterni-ob/lista-subalterni-ob.component';
import { ObiectiveRoutingModule } from './obiective-routing.module';
import { SetareObiectiveComponent } from './setare-obiective/setare-obiective.component';
import { ObiectivInputComponent } from './setare-obiective/obiectiv-input/obiectiv-input.component';
import { ObiectivFromDdComponent } from './setare-obiective/obiectiv-from-dd/obiectiv-from-dd.component';
import { ObiectiveActualeComponent } from './obiective-actuale/obiective-actuale.component';
import { IsotoricObiectiveComponent } from './isotoric-obiective/isotoric-obiective.component';
import { EvaluareObiectiveComponent } from './evaluare-obiective/evaluare-obiective.component';

@NgModule({
  declarations: [
    ListaSubalterniObComponent,
    SetareObiectiveComponent,
    ObiectivInputComponent,
    ObiectivFromDdComponent,
    ObiectiveActualeComponent,
    IsotoricObiectiveComponent,
    EvaluareObiectiveComponent
  ],
  imports: [
    HeaderModule,
    SharedModule,
    ObiectiveRoutingModule
  ],
  exports: [
  ],
  providers: [],
})
export class ObiectiveModule { }
