import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { NavbarComponent } from './navbar.component';
import { SharedModule } from '../shared.module';
import { AutentificareModule } from 'src/app/components/autentificare/autentificare.module';
import { NavbarAquilaComponent } from './navbar-aquila/navbar-aquila.component';
import { NavbarCrevediaComponent } from './navbar-crevedia/navbar-crevedia.component';
import { NavbarDemoComponent } from './navbar-demo/navbar-demo.component';
import { NavbarTvrComponent } from './navbar-tvr/navbar-tvr.component';
import { NavbarImactComponent } from './navbar-impact/navbar-impact.component';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    SharedModule,
    AutentificareModule
  ],
  declarations: [
    NavbarComponent,
    NavbarAquilaComponent,
    NavbarCrevediaComponent,
    NavbarDemoComponent,
    NavbarTvrComponent,
    NavbarImactComponent
  ],
  exports: [NavbarComponent]
})
export class NavbarModule {}
