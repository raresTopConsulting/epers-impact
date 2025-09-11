import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { NPosturi } from 'src/app/models/nomenclatoare/NPosturi';
import { SetareProfil, TableSetareProfil } from 'src/app/models/nomenclatoare/SetareProfil';
import { PosturiService } from 'src/app/services/nomenclatoare/posturi.service';
import { DropdownStateService } from 'src/app/states/dropdown/dropdown-state.service';
import { UntilDestroy } from '@ngneat/until-destroy';
import { DropdownSelection } from 'src/app/models/common/Dorpdown';

@UntilDestroy({checkProperties: true})
@Component({
    selector: 'setare-profil',
    templateUrl: './setare-profil.component.html',
    standalone: false
})
export class SetareProfilComponent implements OnInit {
  @Input() denumirePost: string;
  currentProfil: SetareProfil;
  tableSetareProfil: TableSetareProfil[] = [];
  post: NPosturi;
  isEdit: boolean;
  ddCompetente: DropdownSelection[];
  idealList = [1, 2, 3, 4, 5];
  showUpdateBtn: boolean = false;
  saveAttempted: boolean = false;
  private idPost: number;
  private listaProfileSetate: SetareProfil[] = [];
  ddSub: Subscription;
  postSub: Subscription;
  profilPostSub: Subscription;

  constructor(private ddStateService: DropdownStateService,
    private postService: PosturiService,
    private route: ActivatedRoute,
    private toastr: ToastrService) {

    this.currentProfil = {} as SetareProfil;
  }

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.idPost = +params['id'];
      this.postSub = this.postService.get(this.idPost).subscribe((postData) => {
        this.post = postData;
        this.ddSub = this.ddStateService.appDropdownsSubject$.subscribe((ddData) => {
          this.ddCompetente = ddData?.DdCompetente.filter(dd => dd?.IdFirma === postData?.idFirma);
        });
      });
      this.getProfilulPostului(this.idPost);
    });
  }

  addToProfil() {
    this.currentProfil.id_Post = this.idPost;
    this.saveAttempted = true;
    if (this.currentProfil.id_Skill && this.currentProfil.ideal) {
      this.postService.addProfilPost(this.currentProfil).subscribe((data) => {
        if (data) {
          this.resetDataAfterSave();
          this.toastr.success('Profilul a fost adăugat cu succes!');
        } else {
          this.toastr.error('A apărut o eroare la adăugare!');
        }
      });
    }
  }

  selectProfilPost(profil: any) {
    if (profil.selected) {
      this.currentProfil = profil.setareProfil;
      this.showUpdateBtn = true;
    } else {
      this.currentProfil = {} as SetareProfil;
      this.tableSetareProfil.forEach(x => x.selected = false);
      this.showUpdateBtn = false;
    }
  }

  updateProfilPost() {
    this.saveAttempted = true;
    this.postService.updateProfilPost(this.currentProfil).subscribe((data) => {
      if (data) {
        this.resetDataAfterSave();
        this.toastr.success('Profilul a fost actualizat cu succes!');
      } else {
        this.toastr.error('A apărut o eroare la actualizare!');
      }
    });
  }

  removeProfilPost(index: any) {
    let deleteId = this.listaProfileSetate[index].id;
    if (deleteId) {
      this.postService.deleteProfiliPost(deleteId).subscribe((data) => {
        if (data) {
          this.getProfilulPostului(this.idPost);
          this.toastr.success('Profilul a fost șters cu succes!', 'Succes!');
        }
      });
    }
  }

  private getProfilulPostului(idPost: number) {
    this.profilPostSub = this.postService.getProfilPost(idPost).subscribe((data) => {
      this.tableSetareProfil = data;
      this.listaProfileSetate = data.map(x => x.setareProfil);
    });
  }

  private resetDataAfterSave() {
    this.tableSetareProfil.forEach(x => x.selected = false);
    this.currentProfil = {} as SetareProfil;
    this.getProfilulPostului(this.idPost);
    this.saveAttempted = false;
    this.showUpdateBtn = false;
  }
}
