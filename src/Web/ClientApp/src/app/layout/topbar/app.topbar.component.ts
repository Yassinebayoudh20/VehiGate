import { Component, ElementRef, OnDestroy, OnInit, ViewChild, inject } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { LayoutService } from '../service/app.layout.service';
import { AuthService } from 'src/app/core/services/auth.service';
import { UserInfo } from 'src/app/core/data/dtos/auth/user-info-dto';
import { Observable, Subscription } from 'rxjs';

@Component({
  selector: 'app-topbar',
  templateUrl: './app.topbar.component.html',
})
export class AppTopBarComponent implements OnInit, OnDestroy {
  settingsMenuItems!: MenuItem[];

  @ViewChild('menubutton') menuButton!: ElementRef;

  @ViewChild('topbarmenubutton') topbarMenuButton!: ElementRef;

  @ViewChild('topbarmenu') menu!: ElementRef;

  currentUser$: Observable<UserInfo>;

  logoutSubscription: Subscription;

  authService = inject(AuthService);

  constructor(public layoutService: LayoutService) {}

  ngOnInit(): void {
    this.currentUser$ = this.authService.currentUser;

    this.settingsMenuItems = [
      {
        label: 'Logout',
        icon: 'pi pi-sign-out',
        command: () => {
          this.logoutSubscription = this.authService.logout().subscribe();
        },
      },
    ];
  }

  isLoggedIn() {
    return this.authService.isLoggedIn();
  }

  ngOnDestroy(): void {
    if (this.logoutSubscription) {
      this.logoutSubscription.unsubscribe();
    }
  }
}
