import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UntilDestroy } from '@ngneat/until-destroy';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { Notite } from 'src/app/models/evaluare/Mentiuni';
import { UserCreateModel } from 'src/app/models/useri/User';
import { EvaluareService } from 'src/app/services/evaluare/evaluare.service';
import { UserStateService } from 'src/app/services/user/user-state.service';

@UntilDestroy({ checkProperties: true })
@Component({
    selector: 'mentiuni',
    templateUrl: './mentiuni.component.html',
    standalone: false
})
export class MentiuniComponent implements OnInit {
  idAngajat: number;
  mentiuni: Notite[];
  currentDate: Date;
  mentiuneNoua: Notite;
  years = Array.from({length: 50}, (_, i) => 2022 + i);
  selectedYear: number;

  evalSub: Subscription;
  userSub: Subscription;

  constructor(private route: ActivatedRoute,
    private router: Router,
    private evalService: EvaluareService,
    private userService: UserStateService,
    private toastr: ToastrService) {

    this.currentDate = new Date;
    this.route.params.subscribe(params => {
      this.idAngajat = +params['id'];
      this.userSub = this.userService.get(this.idAngajat).subscribe((user) => {
        const loggedInUserId = JSON.parse(localStorage.getItem('user')).id.toString()
        this.initMentiuneObject(user, loggedInUserId);
      });
    });
  }

  ngOnInit() {
    this.getMentiuni(this.idAngajat, this.currentDate.getFullYear());
  }

  submitForm() {
    this.evalSub = this.evalService.addMentiune(this.mentiuneNoua).subscribe(() => {
      this.toastr.success('Mențiunea a fost salvată cu succes!');
      setTimeout(() => {
        this.router.navigate(['../../listaSubalterni'], { relativeTo: this.route });
      }, 500);
    })
  }

  getMentiuniInYear() {
    this.getMentiuni(this.idAngajat, this.selectedYear);
  }

  private getMentiuni(idAngajat: number, anul: number) {
    this.evalSub = this.evalService.getMentiuni(idAngajat, anul).subscribe((data) => {
      this.mentiuni = data;
    });
  }

  private initMentiuneObject(userData: UserCreateModel, loggedInUserId: string) {
    this.mentiuneNoua = {
      data: this.currentDate,
      idAngajat: this.idAngajat.toString(),
      id: 0,
      idSuperior: userData.idSuperior?.toString(),
      updateId: loggedInUserId,
      matricolaAngajat: userData?.matricola,
      matricolaSuperior: userData?.matricolaSuperior,
      nota: ""
    };
  }
}
