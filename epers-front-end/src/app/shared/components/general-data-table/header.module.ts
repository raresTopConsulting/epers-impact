import { NgModule } from '@angular/core';
import { SharedModule } from '../shared.module';
import { TableHeaderComponent } from './table-header/table-header.component';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [
    TableHeaderComponent
  ],
  imports: [
    SharedModule,
    RouterModule
  ],
  exports: [
    TableHeaderComponent
  ],
  providers: [],
})
export class HeaderModule { }
