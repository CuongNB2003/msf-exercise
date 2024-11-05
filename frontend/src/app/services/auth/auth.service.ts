import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { LoginInput, LoginResponse, RefreshTokenResponse, RegisterInput, UserLogin } from './auth.interface';
import { ErrorHandingService } from '../error-handing/error-handing.service';
import { ResponseObject, ResponseText } from '../config/response';
import { baseURL } from '@services/config/baseURL';
const apiUrl = `${baseURL}api/auth`;

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private http: HttpClient, private errorHandingService: ErrorHandingService) { }

  login(loginInput: LoginInput): Observable<ResponseObject<LoginResponse>> {
    return this.http.post<ResponseObject<LoginResponse>>(`${apiUrl}/login`, loginInput).pipe(
      catchError((error: HttpErrorResponse) => this.errorHandingService.getErrorObservable(error))
    );

  }

  register(registerInput: RegisterInput): Observable<ResponseText> {
    return this.http.post<ResponseText>(`${apiUrl}/register`, registerInput).pipe(
      catchError((error: HttpErrorResponse) => this.errorHandingService.getErrorObservable(error))
    );
  }

  logout(): Observable<ResponseText> {
    return this.http.post<ResponseText>(`${apiUrl}/logout`, {}).pipe(
      catchError((error: HttpErrorResponse) => this.errorHandingService.getErrorObservable(error))
    );
  }

  refreshToken(refreshToken: string): Observable<ResponseObject<RefreshTokenResponse>> {
    return this.http.post<ResponseObject<RefreshTokenResponse>>(`${apiUrl}/refresh-token?refreshToken=${refreshToken}`, {}).pipe(
      catchError((error: HttpErrorResponse) => this.errorHandingService.getErrorObservable(error))
    );
  }

  me(): Observable<ResponseObject<UserLogin>> {
    return this.http.get<ResponseObject<UserLogin>>(`${apiUrl}/me`).pipe(
      catchError((error: HttpErrorResponse) => this.errorHandingService.getErrorObservable(error))
    )
  }

}
