import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import { AuthenticationGuardService } from './components/autentificare/authentication-guard.service';
import { HomeComponent } from './components/home/home.component';

const appRoutes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'auth',
    loadChildren: () =>
      import('./components/autentificare/autentificare.module').then(
        (m) => m.AutentificareModule
      ),
  },
  {
    canActivate: [AuthenticationGuardService],
    path: "nomenclatoare",
    loadChildren: () => import('./components/nomenclatoare/nomenclatoare.module').then(m => m.NomenclatoareModule)
  },
  {
    canActivate: [AuthenticationGuardService],
    path: "evaluare",
    loadChildren: () => import('./components/evaluare/evaluare.module').then(m => m.EvaluareModule)
  },
  {
    canActivate: [],
    path: "utilizatori",
    loadChildren: () => import('./components/utilizatori/user.module').then(m => m.UserModule)
  },
  {
    canActivate: [AuthenticationGuardService],
    path: "obiective",
    loadChildren: () => import('./components/obiective/obiective.module').then(m => m.ObiectiveModule)
  },
  {
    canActivate: [AuthenticationGuardService],
    path: "concluzii",
    loadChildren: () => import('./components/concluzii/concluzii.module').then(m => m.ConcluziiModule)
  },
  {
    canActivate: [AuthenticationGuardService],
    path: "ajutor",
    loadChildren: () => import('./components/ajutor/ajutor.module').then(m => m.AjutorModule)
  },
  
];

@NgModule({
  imports: [RouterModule.forRoot(appRoutes, { preloadingStrategy: PreloadAllModules })],
  exports: [RouterModule],
})
export class AppRoutingModule { }
