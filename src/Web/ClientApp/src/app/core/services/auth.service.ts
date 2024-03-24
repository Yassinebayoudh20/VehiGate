import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, shareReplay, switchMap, tap } from 'rxjs';
import { environment } from 'src/environments/environment';
import { LOCAL_STORAGE_TOKEN_NAME } from '../constants';
import { UserInfo } from '../data/dtos/auth/user-info-dto';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AuthenticationClient, AuthenticationResponse, LoginCommand, LogoutCommand, RegisterCommand, Result } from 'src/app/web-api-client';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  api = environment.API_ENDPOINT + '/Authentication';
  public currentUserSubject: BehaviorSubject<UserInfo>;
  public currentUser: Observable<UserInfo>;
  private decodedToken: any;

  constructor(private jwtHelper: JwtHelperService, private authClient: AuthenticationClient, private router: Router) {
    this.currentUserSubject = new BehaviorSubject<UserInfo>(this.getUserFromToken());
    this.currentUser = this.currentUserSubject.asObservable();
  }

  register(registerCommand: RegisterCommand): Observable<void> {
    return this.authClient.register(registerCommand);
  }

  login(loginCommand: LoginCommand): Observable<AuthenticationResponse> {
    return this.authClient.authenticate(loginCommand).pipe(
      tap((tokenResponse: AuthenticationResponse) => {
        this.saveToken(tokenResponse.token);
        //this.updateUser();
      }),
      shareReplay()
    );
  }

  logout(): Observable<unknown> {
    return this.authClient.signOut(new LogoutCommand()).pipe(
      tap((result) => {
        localStorage.removeItem(LOCAL_STORAGE_TOKEN_NAME);
        this.router.navigate(['auth/login']);
      }),
      switchMap((result) => {
        return new Observable((observer) => {
          observer.next();
          observer.complete();
        });
      })
    );
  }

  getToken = (): string | null => localStorage.getItem(LOCAL_STORAGE_TOKEN_NAME);

  saveToken(token) {
    localStorage.setItem(LOCAL_STORAGE_TOKEN_NAME, token);
  }

  public get currentUserValue(): UserInfo {
    return this.currentUserSubject.value;
  }

  private getUserFromToken(): UserInfo {
    const token = localStorage.getItem(LOCAL_STORAGE_TOKEN_NAME);
    if (token) {
      if (!this.decodedToken || this.decodedToken.token !== token) {
        this.decodedToken = this.jwtHelper.decodeToken(token);
      }
      const isTokenExpired = this.jwtHelper.isTokenExpired(token);

      if (!isTokenExpired) {
        return this.decodedToken;
      } else {
        this.logout();
      }
    }
    return null;
  }

  public getUserDetails(): UserInfo {
    const user = this.getUserFromToken();
    if (user) {
      return user;
    }
    return null;
  }

  isLoggedIn(): boolean {
    const token = localStorage.getItem(LOCAL_STORAGE_TOKEN_NAME);
    return token && !this.jwtHelper.isTokenExpired(token);
  }
}
