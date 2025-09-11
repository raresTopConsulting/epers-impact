import { NgModule } from '@angular/core';
import { LocatiiComponent } from './locatii/locatii/locatii.component';
import { LocatieAddComponent } from './locatii/locatii/locatie-add/locatie-add.component';
import { LocatieEditComponent } from './locatii/locatii/locatie-add/locatie-edit.component';
import { HeaderModule } from '../../shared/components/general-data-table/header.module';
import { SharedModule } from '../../shared/components/shared.module';
import { NomenclatoareRoutingModule } from './nomenclatoare-routing.module';
import { DiviziiComponent } from './divizii/divizii.component';
import { DivizieAddComponent } from './divizii/divizie-add-edit/divizie-add.component';
import { DivizieEditComponent } from './divizii/divizie-add-edit/divizie-edit.component';
import { PosturiComponent } from './posturi/posturi.component';
import { PostAddComponent } from './posturi/post-add/post-add.component';
import { PostEditComponent } from './posturi/post-add/post-edit.component';
import { SkillsComponent } from './skills/skills.component';
import { SkillAddComponent } from './skills/skill-add-edit/skill-add.component';
import { SkillEditComponent } from './skills/skill-add-edit/skill-edit.component';
import { CompartimenteComponent } from './compartimente/compartimente.component';
import { CompartimentEditComponent } from './compartimente/compartiment-edit/compartiment-edit.component';
import { SetareProfilComponent } from './posturi/setare-profil/setare-profilcomponent';
import { NomObListComponent } from './nom-obiective/nom-ob-list/nom-ob-list.component';
import { NomObiectiveAddComponent } from './nom-obiective/nom-obiective-add-edit/nom-obiective-add.component';
import { NomObiectiveEditComponent } from './nom-obiective/nom-obiective-add-edit/nom-obiective-edit.component';
import { CompartimentAddComponent } from './compartimente/compartiment-edit/compartiment-add.component';
import { CursuriComponent } from './cursuri/cursuri.component';
import { CursuriAddComponent } from './cursuri/cursuri-edit/cursuri-add.component';
import { CursuriEditComponent } from './cursuri/cursuri-edit/cursuri-edit.component';
import { CursRecomandatDetailsComponent } from './cursuri/curs-recomandat-details/curs-recomandat-details.component';
import { FirmeComponent } from './firme/firme.component';
import { FirmaAddComponent } from './firme/firma-add-edit/firma-add.component';
import { FirmaEditComponent } from './firme/firma-add-edit/firma-edit.component';

@NgModule({
  declarations: [
    LocatiiComponent,
    LocatieAddComponent,
    LocatieEditComponent,
    DiviziiComponent,
    DivizieAddComponent,
    DivizieEditComponent,
    PosturiComponent,
    PostAddComponent,
    PostEditComponent,
    SkillsComponent,
    SkillAddComponent,
    SkillEditComponent,
    CompartimenteComponent,
    CompartimentEditComponent,
    CompartimentAddComponent,
    SetareProfilComponent,
    NomObListComponent,
    NomObiectiveAddComponent,
    NomObiectiveEditComponent,
    CursuriComponent,
    CursuriAddComponent,
    CursuriEditComponent,
    CursRecomandatDetailsComponent,
    FirmeComponent,
    FirmaAddComponent,
    FirmaEditComponent
  ],
  imports: [
    HeaderModule,
    SharedModule,
    NomenclatoareRoutingModule
  ],
  exports: [
    CursRecomandatDetailsComponent
  ],
  providers: [],
})
export class NomenclatoareModule { }
