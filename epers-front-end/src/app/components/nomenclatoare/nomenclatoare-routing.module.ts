import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CompartimentEditComponent } from './compartimente/compartiment-edit/compartiment-edit.component';
import { CompartimenteComponent } from './compartimente/compartimente.component';
import { DivizieAddComponent } from './divizii/divizie-add-edit/divizie-add.component';
import { DivizieEditComponent } from './divizii/divizie-add-edit/divizie-edit.component';
import { DiviziiComponent } from './divizii/divizii.component';
import { LocatieAddComponent } from './locatii/locatii/locatie-add/locatie-add.component';
import { LocatieEditComponent } from './locatii/locatii/locatie-add/locatie-edit.component';
import { LocatiiComponent } from './locatii/locatii/locatii.component';
import { PostAddComponent } from './posturi/post-add/post-add.component';
import { PostEditComponent } from './posturi/post-add/post-edit.component';
import { PosturiComponent } from './posturi/posturi.component';
import { SkillAddComponent } from './skills/skill-add-edit/skill-add.component';
import { SkillEditComponent } from './skills/skill-add-edit/skill-edit.component';
import { SkillsComponent } from './skills/skills.component';
import { SetareProfilComponent } from './posturi/setare-profil/setare-profilcomponent';
import { NomObListComponent } from './nom-obiective/nom-ob-list/nom-ob-list.component';
import { NomObiectiveAddComponent } from './nom-obiective/nom-obiective-add-edit/nom-obiective-add.component';
import { NomObiectiveEditComponent } from './nom-obiective/nom-obiective-add-edit/nom-obiective-edit.component';
import { CompartimentAddComponent } from './compartimente/compartiment-edit/compartiment-add.component';
import { CursuriComponent } from './cursuri/cursuri.component';
import { CursuriAddComponent } from './cursuri/cursuri-edit/cursuri-add.component';
import { CursuriEditComponent } from './cursuri/cursuri-edit/cursuri-edit.component';
import { FirmeComponent } from './firme/firme.component';
import { FirmaAddComponent } from './firme/firma-add-edit/firma-add.component';
import { FirmaEditComponent } from './firme/firma-add-edit/firma-edit.component';

const routes: Routes = [
  { path: 'locatii', component: LocatiiComponent },
  {
    path: 'locatii',
    children: [
      { path: 'add', component: LocatieAddComponent },
      { path: 'edit/:id', component: LocatieEditComponent },
    ],
  },
  { path: 'divizii', component: DiviziiComponent },
  {
    path: 'divizii',
    children: [
      { path: 'add', component: DivizieAddComponent },
      { path: 'edit/:id', component: DivizieEditComponent },
    ],
  },
  { path: 'skills', component: SkillsComponent },
  {
    path: 'skills',
    children: [
      { path: 'add', component: SkillAddComponent },
      { path: 'edit/:id', component: SkillEditComponent },
    ],
  },
  { path: 'compartimente', component: CompartimenteComponent },
  {
    path: 'compartimente',
    children: [
      { path: 'add', component: CompartimentAddComponent },
      { path: 'edit/:id', component: CompartimentEditComponent },
    ],
  },
  { path: 'posturi', component: PosturiComponent },
  {
    path: 'posturi',
    children: [
      { path: 'add', component: PostAddComponent },
      { path: 'edit/:id', component: PostEditComponent },
      { path: 'profilPost/:id', component: SetareProfilComponent }
    ],
  },
  { path: 'nomObiective', component: NomObListComponent },
  {
    path: 'nomObiective',
    children: [
      { path: 'add', component: NomObiectiveAddComponent },
      { path: 'edit/:id', component: NomObiectiveEditComponent }
    ],
  },
  { path: 'cursuri', component: CursuriComponent },
  {
    path: 'cursuri',
    children: [
      { path: 'add', component: CursuriAddComponent },
      { path: 'edit/:id', component: CursuriEditComponent }
    ],
  },
  { path: 'firme', component: FirmeComponent },
  {
    path: 'firme',
    children: [
      { path: 'add', component: FirmaAddComponent },
      { path: 'edit/:id', component: FirmaEditComponent },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class NomenclatoareRoutingModule {}
