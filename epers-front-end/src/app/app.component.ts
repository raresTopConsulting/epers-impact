import { Component, OnInit } from '@angular/core';
import { AutentificareService } from './services/autentificare/autentificare.service';
import { DropdownStateService } from './states/dropdown/dropdown-state.service';
import { SelectionBoxStateService } from './states/selectionBox/selectionBox.service';
import { FirmeService } from './services/nomenclatoare/firme.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  standalone: false
})
export class AppComponent implements OnInit {

  constructor(private authService: AutentificareService,
    private ddStateService: DropdownStateService,
    private selectionBoxStateService: SelectionBoxStateService,
    private firmeService: FirmeService) { }

  ngOnInit() {
    this.authService.refreshToken().subscribe({
      next: () => {
      },
      error: () => {
        this.authService.logout();
      }
    });
    if (this.authService.isAuthenticated()) {
      this.ddStateService.upsertDropdowns();
      this.selectionBoxStateService.upsertSelections();
      this.firmeService.upsertDDFirme();
    }
  }
}
