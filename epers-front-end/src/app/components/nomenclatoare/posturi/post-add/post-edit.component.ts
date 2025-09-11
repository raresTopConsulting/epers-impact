import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { UntilDestroy } from '@ngneat/until-destroy';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { DropdownSelection } from 'src/app/models/common/Dorpdown';
import { NPosturi } from 'src/app/models/nomenclatoare/NPosturi';
import { FirmeService } from 'src/app/services/nomenclatoare/firme.service';
import { PosturiService } from 'src/app/services/nomenclatoare/posturi.service';
import { DropdownStateService } from 'src/app/states/dropdown/dropdown-state.service';

@UntilDestroy({ checkProperties: true })
@Component({
    selector: 'post-edit',
    templateUrl: './post-add-edit.component.html',
    standalone: false
})
export class PostEditComponent implements OnInit {
  post: NPosturi;
  isEditMode: boolean = true;
  private id: number;
  loggedInUserHasFirma: boolean = false;
  ddFirmeSelectionList: DropdownSelection[];
  loggedInUserIdFirma: number | null;
  firmeSub: Subscription;
  postSub: Subscription;
  firmaLoggedInUserSub: Subscription;

  constructor(private firmeService: FirmeService,
    private posturiService: PosturiService,
    private toastr: ToastrService,
    private ddService: DropdownStateService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.loggedInUserIdFirma = JSON.parse(localStorage.getItem('user'))?.idFirma;

    this.firmeSub = this.firmeService.firmeSelections$.subscribe((data) => {
      this.ddFirmeSelectionList = data;;
    });

    this.firmaLoggedInUserSub = this.firmeService.firmaLoggedInUser$.subscribe((data) => {
      if (data) {
        this.loggedInUserHasFirma = true;
      }
    });
  }

  ngOnInit() {
    this.route.params.subscribe((params) => {
      this.id = +params['id'];
      this.postSub = this.posturiService.get(this.id).subscribe((post) => {
        this.post = post;
      });
    });
  }

  onSubmit() {
    this.posturiService.update(this.post).subscribe({
      next: () => {
        this.ddService.upsertDropdowns();
        this.toastr.success('Postul a fost modificat cu succes!', 'Succes!');
        setTimeout(() => {
          this.goToPosturi();
        }, 1000);
      },
      error: (err) => {
        this.toastr.error(
          `Postul nu a putut fi modificat!, ${err.message}`,
          'Eroare!'
        );
      }
    });
  }

  onCancel() {
    this.goToPosturi();
  }

  private goToPosturi() {
    this.router.navigate(['/posturi']);
  }

}
