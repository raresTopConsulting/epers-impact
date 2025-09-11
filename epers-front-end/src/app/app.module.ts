import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { HeaderModule } from './shared/components/general-data-table/header.module';
import { SharedModule } from './shared/components/shared.module';
import { NomenclatoareModule } from './components/nomenclatoare/nomenclatoare.module';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { NavbarModule } from './shared/components/navbar/navbar.module';
import { AuthInterceptor } from './services/autentificare/auth.interceptor';
import { DatePipe } from '@angular/common';
import { PipModule } from './components/pip/pip.module';

@NgModule({ declarations: [
        AppComponent,
        HomeComponent
    ],
    bootstrap: [AppComponent], imports: [BrowserModule,
        NavbarModule,
        HeaderModule,
        SharedModule,
        NomenclatoareModule,
        BrowserAnimationsModule,
        AppRoutingModule,
        PipModule,
        ToastrModule.forRoot()], providers: [{
            provide: HTTP_INTERCEPTORS,
            useClass: AuthInterceptor,
            multi: true
        },
        DatePipe, provideHttpClient(withInterceptorsFromDi())] })
export class AppModule { }
