// Currently OBSOLETE -> CHECK IN THE FUTURE IF KEEP OR NOT
// import { Injectable, OnDestroy } from '@angular/core';
// import { fromEvent, Subject, Subscription, timer } from 'rxjs';
// import { takeUntil } from 'rxjs/operators';

// @Injectable({
//     providedIn: 'root'
// })
// export class InactivityService implements OnDestroy {
//     private readonly INACTIVITY_TIMEOUT = 60 * 30 * 1000; // 30 minutes
//     private readonly CHECK_INTERVAL = 60 * 10 * 1000; // 10 minutes

//     private destroy = new Subject<void>();
//     private activityChanged = new Subject<boolean>();
//     private inactivityTimer: any;

//     private resetMouseMoveSub: Subscription;
//     private resetKeyDownSub: Subscription;

//     constructor() {
//         this.inactivityTimer = this.checkUserIsActive();
//     }

//     private checkUserIsActive() {
//         let lastActivityTime = Date.now();

//         this.resetMouseMoveSub = fromEvent(document, 'mousemove')
//             .pipe(takeUntil(this.destroy))
//             .subscribe(() => {
//                 lastActivityTime = Date.now();
//             });

//         this.resetKeyDownSub = fromEvent(document, 'keydown')
//             .pipe(takeUntil(this.destroy))
//             .subscribe(() => {
//                 lastActivityTime = Date.now();
//             });

//         return timer(0, this.CHECK_INTERVAL)
//             .pipe(takeUntil(this.destroy))
//             .subscribe(() => {
//                 const currentTime = Date.now();
//                 const elapsedTime = currentTime - lastActivityTime;

//                 if (elapsedTime < this.INACTIVITY_TIMEOUT) {
//                     this.activityChanged.next(true);
//                 } else {
//                     this.activityChanged.next(false);
//                 }
//             });
//     }

//     getActivityChanged(): Subject<boolean> {
//         return this.activityChanged;
//     }

//     ngOnDestroy(): void {
//         this.resetMouseMoveSub.unsubscribe();
//         this.resetKeyDownSub.unsubscribe();
//         this.destroy.next();
//         this.destroy.complete();
//         if (this.inactivityTimer && typeof this.inactivityTimer.unsubscribe === 'function') {
//             this.inactivityTimer.unsubscribe();
//         }
//     }
// }
