import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { catchError, Observable, throwError } from "rxjs";
import baseURL from "../baseURL";
import { LoginResponse, User } from "./auth.interface";
import { ErrorHandlerService } from "../error-handler.service";
import { Router } from "@angular/router";

const httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'Application/json' })

}
const apiUrl = `${baseURL}api/auth`;
const apiCaptcha = `${baseURL}api/captcha`

@Injectable({
    providedIn: 'root'
})

export class AuthService {
    constructor(private http: HttpClient, private errorHandler: ErrorHandlerService, private router: Router) { }

    login(email: string, password: string, reCaptchaToken: string): Observable<LoginResponse> {
        return this.http.post<LoginResponse>(`${apiUrl}/login`, { email, password, reCaptchaToken },).pipe(
            catchError(this.errorHandler.handlerError)
        );
    }

    register(user: User): Observable<LoginResponse> {
        return this.http.post<LoginResponse>(`${apiUrl}/register`, user, { headers: httpOptions.headers }).pipe(
            catchError(this.errorHandler.handlerError)
        );
    }

    isLoggedIn(): boolean {

        if (typeof window !== 'undefined' && typeof localStorage !== 'undefined') {
            return !!localStorage.getItem('accessToken');
        } else {
            return false;
        }
    }

    logout() {
        localStorage.removeItem('accessToken');
        localStorage.removeItem('userData');
        this.router.navigate(['/login']);
    }
}