import { Injectable } from '@angular/core';
import { ErrorHandingService } from '../error-handing/error-handing.service';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { InputCreateUser, InputUpdateUser, UserResponse } from './user.interface';
import { PagedResult, ResponseObject, ResponseText } from '../config/response';
import { catchError, Observable } from 'rxjs';
import { baseURL } from '@services/config/baseURL';
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

  createUser(input: InputCreateUser): Observable<ResponseText> {
    return this.http.post<ResponseText>(`${apiUrl}`, input).pipe(
      catchError((error: HttpErrorResponse) => this.errorHandingService.getErrorObservable(error))
    )
  }

  updateUser(input: InputUpdateUser, id: number): Observable<ResponseText> {
    return this.http.put<ResponseText>(`${apiUrl}/${id}`, input).pipe(
      catchError((error: HttpErrorResponse) => this.errorHandingService.getErrorObservable(error))
    )
  }

  deleteUser(id: number): Observable<ResponseText> {
    return this.http.delete<ResponseText>(`${apiUrl}/${id}`).pipe(
      catchError((error: HttpErrorResponse) => this.errorHandingService.getErrorObservable(error))
    )
  }
}
