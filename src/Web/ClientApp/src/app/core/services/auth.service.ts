import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserRegisterDto } from '../data/dtos/auth/user-register-dto';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { UserLoginDto } from '../data/dtos/auth/user-login-dto';
import { environment } from 'src/environments/environment';
import { TokenResponse } from '../data/dtos/auth/token-response-dto';
import { LOCAL_STORAGE_TOKEN_NAME } from '../constants';
import { UserInfo } from '../data/dtos/auth/user-info-dto';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AuthenticationClient, AuthenticationResponse, LoginCommand } from 'src/app/web-api-client';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  api = environment.API_ENDPOINT + '/Authentication';
  private currentUserSubject: BehaviorSubject<UserInfo>;
  public currentUser: Observable<UserInfo>;
  private decodedToken: any;

  constructor(private http: HttpClient, private jwtHelper: JwtHelperService, private authClient: AuthenticationClient) {
    this.currentUserSubject = new BehaviorSubject<UserInfo>(this.getUserFromToken());
    this.currentUser = this.currentUserSubject.asObservable();
  }

  register(registerDto: UserRegisterDto): Observable<any> {
    return this.http.post(this.api + '/register', registerDto);
  }

  login(loginCommand: LoginCommand): Observable<AuthenticationResponse> {
    return this.authClient.authenticate(loginCommand).pipe(
      tap((tokenResponse: AuthenticationResponse) => {
        this.saveToken(tokenResponse.token);
        this.updateUser();
      })
    );
  }

  logout(): Observable<void> {
    localStorage.removeItem(LOCAL_STORAGE_TOKEN_NAME);
    return new Observable((observer) => {
      observer.next();
      observer.complete();
    });
  }

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
      return this.decodedToken;
    }
    return null;
  }

  public updateUser(): void {
    const user = this.getUserFromToken();
    if (user) {
      this.currentUserSubject.next(user);
    }
  }
}
