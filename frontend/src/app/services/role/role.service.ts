import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable, Input } from '@angular/core';
import { ErrorHandingService } from '../error-handing/error-handing.service';
import baseURL from '../config/baseURL';
import { catchError, Observable } from 'rxjs';
import { PagedResult, ResponseObject, ResponseText } from '../config/response';
import { RoleInput, RoleResponse } from './role.interface';
const apiUrl = `${baseURL}api/role`;
@Injectable({
  providedIn: 'root'
})
export class RoleService {

  constructor(private http: HttpClient, private errorHandingService: ErrorHandingService) { }

  getRoleAll(page: number, limit: number): Observable<ResponseObject<PagedResult<RoleResponse>>> {
    return this.http.get<ResponseObject<PagedResult<RoleResponse>>>(`${apiUrl}?page=${page}&limit=${limit}`).pipe(
      catchError((error: HttpErrorResponse) => this.errorHandingService.getErrorObservable(error))
    )
  }

  getRoleById(id: number): Observable<ResponseObject<RoleResponse>> {
    return this.http.get<ResponseObject<RoleResponse>>(`${apiUrl}/${id}`).pipe(
      catchError((error: HttpErrorResponse) => this.errorHandingService.getErrorObservable(error))
    )
  }

  createRole(input: RoleInput): Observable<ResponseText> {
    return this.http.post<ResponseText>(`${apiUrl}`, input).pipe(
      catchError((error: HttpErrorResponse) => this.errorHandingService.getErrorObservable(error))
    )
  }

  updateRole(input: RoleInput, id: number): Observable<ResponseText> {
    return this.http.put<ResponseText>(`${apiUrl}/${id}`, input).pipe(
      catchError((error: HttpErrorResponse) => this.errorHandingService.getErrorObservable(error))
    )
  }

  deleteRole(id: number): Observable<ResponseText> {
    return this.http.delete<ResponseText>(`${apiUrl}/${id}`).pipe(
      catchError((error: HttpErrorResponse) => this.errorHandingService.getErrorObservable(error))
    )
  }

}
