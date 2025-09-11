import { NgModule } from '@angular/core';
import { HeaderModule } from '../../shared/components/general-data-table/header.module';
import { SharedModule } from '../../shared/components/shared.module';
import { ConcluziiRoutingModule } from './concluzii-routing.module';
import { ListaSubalterniConcluziiComponent } from './lista-subalterni-concluzii/lista-subalterni-concluzii.component';
import { IstoricConcluziiComponent } from './istoric-concluzii/istoric-concluzii.component';
import { ConcluzieAddComponent } from './concluzie-add/concluzie-add.component';
import { NomenclatoareModule } from '../nomenclatoare/nomenclatoare.module';
import { PipModule } from '../pip/pip.module';
import { ConcluziiDetailsComponent } from './concluzii-details/concluzii-details.component';

@NgModule({
  declarations: [
    ListaSubalterniConcluziiComponent,
    ConcluzieAddComponent,
    IstoricConcluziiComponent,
    ConcluziiDetailsComponent
  ],
  imports: [
    HeaderModule,
    SharedModule,
    ConcluziiRoutingModule,
    NomenclatoareModule,
    PipModule
  ],
  exports: [
  ],
  providers: [],
})
export class ConcluziiModule { }
