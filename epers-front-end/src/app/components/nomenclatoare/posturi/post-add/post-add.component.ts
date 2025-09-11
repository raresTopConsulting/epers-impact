import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UntilDestroy } from '@ngneat/until-destroy';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { DropdownSelection } from 'src/app/models/common/Dorpdown';
import { NPosturi } from 'src/app/models/nomenclatoare/NPosturi';
import { FirmeService } from 'src/app/services/nomenclatoare/firme.service';
import { PosturiService } from 'src/app/services/nomenclatoare/posturi.service';
import { DropdownStateService } from 'src/app/states/dropdown/dropdown-state.service';

@UntilDestroy({checkProperties: true})
@Component({
    selector: 'post-add',
    templateUrl: './post-add-edit.component.html',
    standalone: false
})
export class PostAddComponent implements OnInit {
  post: NPosturi;
  isEditMode: boolean = false;
  ddFirmeSelectionList: DropdownSelection[];
  loggedInUserHasFirma: boolean = false;
  firmeSub: Subscription;
  firmaLoggedInUserSub: Subscription;

  constructor(private firmeService: FirmeService,
    private posturiService: PosturiService,
    private ddStateService: DropdownStateService,
    private toastr: ToastrService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.post = {
      id: 0,
      nume: '',
      dataIn: null,
      dataSf: null,
      profilCompetente: '',
      cor: '',
      denFunctie: '',
      nivelPost: '',
      punctaj: 0,
      activ: false,
      idFirma: null,
      firma: ''
    };
  }

  ngOnInit() {
    this.firmeSub = this.firmeService.firmeSelections$.subscribe((data) => {
      this.ddFirmeSelectionList = data;
    });

    this.firmaLoggedInUserSub = this.firmeService.firmaLoggedInUser$.subscribe((data) => {
      if (data) {
        this.loggedInUserHasFirma = true;
        this.post.firma = data.Text;
        this.post.idFirma = data.IdFirma;
      }
    });
  }

  onSubmit() {
    this.posturiService.add(this.post).subscribe({
      next: () => {
        this.ddStateService.upsertDropdowns();
        this.toastr.success('Postul a fost adaugăt cu succes!', 'Succes!');
        setTimeout(() => {
          this.goToPosturi();
        }, 500);
      },
      error: (error) => {
        this.toastr.error(
          `Postul nu a putut fi adaugăt!, ${error.message}`,
          'Eroare!'
        );
      }});
  }

  onCancel() {
    this.goToPosturi();
  }

  private goToPosturi() {
    this.router.navigate(['/posturi'], { relativeTo: this.route });
  }

}
