import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ErrorHandingService } from '../error-handing/error-handing.service';
import baseURL from '../config/baseURL';
import { catchError, Observable } from 'rxjs';
import { PagedResult, ResponseObject } from '../config/response';
import { Role } from './role.interface';
const apiUrl = `${baseURL}api/role`;
@Injectable({
  providedIn: 'root'
})
export class RoleService {

  constructor(private http: HttpClient, private errorHandingService: ErrorHandingService) { }

  getAll(page: number, limit: number): Observable<ResponseObject<PagedResult<Role>>> {
    return this.http.get<ResponseObject<PagedResult<Role>>>(`${apiUrl}?page=${page}&limit=${limit}`).pipe(
      catchError((error: HttpErrorResponse) => this.errorHandingService.getErrorObservable(error))
    )
  }


}
