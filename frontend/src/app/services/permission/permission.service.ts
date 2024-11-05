import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PagedResult, ResponseObject, ResponseText } from '@services/config/response';
import { ErrorHandingService } from '@services/error-handing/error-handing.service';
import { catchError, Observable } from 'rxjs';
import { PermissionInput, PermissionResponse } from './permission.interface';
import { baseURL } from '@services/config/baseURL';
const apiUrl = `${baseURL}api/permission`;

@Injectable({
  providedIn: 'root'
})
export class PermissionService {
  constructor(private http: HttpClient, private errorHandingService: ErrorHandingService) { }

  getPermissionAll(page: number, limit: number): Observable<ResponseObject<PagedResult<PermissionResponse>>> {
    return this.http.get<ResponseObject<PagedResult<PermissionResponse>>>(`${apiUrl}?page=${page}&limit=${limit}`).pipe(
      catchError((error: HttpErrorResponse) => this.errorHandingService.getErrorObservable(error))
    )
  }

  getPermissionById(id: number): Observable<ResponseObject<PermissionResponse>> {
    return this.http.get<ResponseObject<PermissionResponse>>(`${apiUrl}/${id}`).pipe(
      catchError((error: HttpErrorResponse) => this.errorHandingService.getErrorObservable(error))
    )
  }

  createPermission(input: PermissionInput): Observable<ResponseText> {
    return this.http.post<ResponseText>(`${apiUrl}`, input).pipe(
      catchError((error: HttpErrorResponse) => this.errorHandingService.getErrorObservable(error))
    )
  }

  updatePermission(input: PermissionInput, id: number): Observable<ResponseText> {
    return this.http.put<ResponseText>(`${apiUrl}/${id}`, input).pipe(
      catchError((error: HttpErrorResponse) => this.errorHandingService.getErrorObservable(error))
    )
  }

  deletePermission(id: number): Observable<ResponseText> {
    return this.http.delete<ResponseText>(`${apiUrl}/${id}`).pipe(
      catchError((error: HttpErrorResponse) => this.errorHandingService.getErrorObservable(error))
    )
  }
}
