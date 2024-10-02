import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { LoginInput, LoginResponse, MeResponse, RefreshTokenResponse, RegisterInput, Token } from './auth.interface';
import baseURL from '../config/baseURL';
import { ErrorHandingService } from '../error-handing/error-handing.service';
import { ResponseObject, ResponseText } from '../config/response';
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

  me(): Observable<ResponseObject<MeResponse>> {
    return this.http.get<ResponseObject<MeResponse>>(`${apiUrl}/me`).pipe(
      catchError((error: HttpErrorResponse) => this.errorHandingService.getErrorObservable(error))
    )
  }

}
