import { Injectable } from '@angular/core';
import { ErrorHandingService } from '../error-handing/error-handing.service';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { UserResponse } from './user.interface';
import { PagedResult, ResponseObject } from '../config/response';
import { catchError, Observable } from 'rxjs';
import baseURL from '../config/baseURL';
const apiUrl = `${baseURL}api/user`;
@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient, private errorHandingService: ErrorHandingService) { }

  getAll(page: number, limit: number): Observable<ResponseObject<PagedResult<UserResponse>>> {
    return this.http.get<ResponseObject<PagedResult<UserResponse>>>(`${apiUrl}?page=${page}&limit=${limit}`).pipe(
      catchError((error: HttpErrorResponse) => this.errorHandingService.getErrorObservable(error))
    )
  }

  getUserById(id: number): Observable<ResponseObject<UserResponse>> {
    return this.http.get<ResponseObject<UserResponse>>(`${apiUrl}/${id}`).pipe(
      catchError((error: HttpErrorResponse) => this.errorHandingService.getErrorObservable(error))
    )
  }
}
