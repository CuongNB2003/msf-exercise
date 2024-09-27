import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { LoginInput, LoginResponse, RegisterInput, ResponseText, Token } from './auth.interface';

import baseURL from '../baseURL';
import { ErrorHandlingService } from '../error-handing/error-handing.service';
const apiUrl = `${baseURL}api/auth`;

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    constructor(private http: HttpClient, private errorHandlingService: ErrorHandlingService) { }

    login(loginInput: LoginInput): Observable<LoginResponse> {
        return this.http.post<LoginResponse>(`${apiUrl}/login`, loginInput).pipe(
            catchError((error: HttpErrorResponse) => this.errorHandlingService.getErrorObservable(error))
        );

    }

    register(registerInput: RegisterInput): Observable<ResponseText> {
        return this.http.post<ResponseText>(`${apiUrl}/register`, registerInput).pipe(
            catchError((error: HttpErrorResponse) => this.errorHandlingService.getErrorObservable(error))
        );
    }

    isLoggedIn(): boolean {
        if (typeof window !== 'undefined' && typeof localStorage !== 'undefined') {
            return !!localStorage.getItem('accessToken');
        } else {
            return false;
        }
    }

    logout(): Observable<ResponseText> {
        const accessTokenString = localStorage.getItem('accessToken');
        let headers = new HttpHeaders();
        if (accessTokenString) {
            const token: Token = JSON.parse(accessTokenString) as Token;
            headers = headers.set('Authorization', `Beare ${token.token}`);
        }
        return this.http.post<ResponseText>(`${apiUrl}/logout`, {}, { headers }).pipe(
            catchError((error: HttpErrorResponse) => this.errorHandlingService.getErrorObservable(error))
        );
    }
}
