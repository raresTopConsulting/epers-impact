import { CommonModule } from '@angular/common';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { ModalDeleteComponent } from '../modals/modal-delete/modal-delete.component';
import { UserdataHeaderComponent } from './userdata-header/userdata-header.component';
import { SaveButtonComponent } from './save-button/save-button.component';
import { CancelButtonComponent } from './cancel-button/cancel-button.component';
import { RouterModule } from '@angular/router';
import { TableEditButtonComponent } from './table-edit-button/table-edit-button.component';
import { TableDeleteButtonComponent } from './table-delete-button/table-delete-button.component';
import { ModalUserDetailsComponent } from '../modals/modal-user-details/modal-user-details.component';
import { BackButtonComponent } from './back-button/back-button.component';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDialogModule } from '@angular/material/dialog';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatInputModule } from '@angular/material/input';
import { MatRadioModule } from '@angular/material/radio';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatTableModule } from '@angular/material/table';
import { SemnificatieNoteEvaluareComponent } from './semnificatie-note-evaluare/semnificatie-note-evaluare.component';
import { IdToValuePipe } from '../pipes/id-to-value.pipe';
import { MatStepperModule } from '@angular/material/stepper';
import { MatChipsModule } from '@angular/material/chips';
import { MatListModule } from '@angular/material/list';
import { PercentageNrInputComponent } from './percentage-nr-input/percentage-nr-input.component.component';

@NgModule({ declarations: [
        ModalDeleteComponent,
        UserdataHeaderComponent,
        SaveButtonComponent,
        CancelButtonComponent,
        TableEditButtonComponent,
        TableDeleteButtonComponent,
        ModalUserDetailsComponent,
        BackButtonComponent,
        SemnificatieNoteEvaluareComponent,
        PercentageNrInputComponent,
        IdToValuePipe
    ],
    exports: [
        CommonModule,
        FormsModule,
        MatIconModule,
        MatButtonModule,
        MatMenuModule,
        MatFormFieldModule,
        MatProgressSpinnerModule,
        MatInputModule,
        MatDialogModule,
        MatCardModule,
        MatRadioModule,
        MatCheckboxModule,
        MatTooltipModule,
        MatTableModule,
        MatStepperModule,
        ModalDeleteComponent,
        UserdataHeaderComponent,
        SaveButtonComponent,
        CancelButtonComponent,
        TableEditButtonComponent,
        TableDeleteButtonComponent,
        ModalUserDetailsComponent,
        BackButtonComponent,
        SemnificatieNoteEvaluareComponent,
        PercentageNrInputComponent,
        IdToValuePipe,
        MatChipsModule,
        MatListModule
    ], imports: [CommonModule,
        FormsModule,
        MatIconModule,
        MatButtonModule,
        MatMenuModule,
        MatFormFieldModule,
        MatProgressSpinnerModule,
        MatInputModule,
        MatDialogModule,
        MatCardModule,
        MatRadioModule,
        MatCheckboxModule,
        MatTableModule,
        MatStepperModule,
        MatChipsModule,
        MatListModule,
        RouterModule], providers: [provideHttpClient(withInterceptorsFromDi())] })
export class SharedModule { }