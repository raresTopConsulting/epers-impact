import { NgModule } from '@angular/core';
import { HeaderModule } from '../../shared/components/general-data-table/header.module';
import { SharedModule } from '../../shared/components/shared.module';
import { NomenclatoareModule } from '../nomenclatoare/nomenclatoare.module';
import { PipRoutingModule } from './pip-routing.module';
import { ModalPipDetailsComponent } from './modal-pip-details/modal-pip-details.component';
import { PipDetailsComponent } from './pip-details/pip-details.component';
import { PipComponent } from './pip.component';
import { ListaSubalterniWithPipComponent } from './lista-subalterni-with-pip/lista-subalterni-with-pip.component';
import { ListaSubalterniCalificatiPipComponent } from './lista-subalterni-calificati-pip/lista-subalterni-calificati-pip.component';
import { AprobarePipModalComponent } from './aprobare-pip-modal/aprobare-pip-modal.component';
import { PipEditComponent } from './pip-edit/pip-edit.component';
import { IstoricListaSubalterniWithPipComponent } from './istoric-lista-subalterni-with-pip/istoric-lista-subalterni-with-pip.component';
import { PipAddComponent } from './pip-edit/pip-add.component';

@NgModule({
  declarations: [
    PipEditComponent,
    PipAddComponent,
    ModalPipDetailsComponent,
    PipDetailsComponent,
    PipComponent,
    ListaSubalterniWithPipComponent,
    ListaSubalterniCalificatiPipComponent,
    AprobarePipModalComponent,
    IstoricListaSubalterniWithPipComponent
  ],
  imports: [
    HeaderModule,
    SharedModule,
    PipRoutingModule,
    NomenclatoareModule
  ],
  exports: [
    ModalPipDetailsComponent,
    PipDetailsComponent
  ],
  providers: [],
})
export class PipModule { }
