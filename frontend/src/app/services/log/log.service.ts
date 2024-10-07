import { Injectable } from '@angular/core';
import { ErrorHandingService } from '../error-handing/error-handing.service';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, Observable } from 'rxjs';
import { PagedResult, ResponseObject } from '../config/response';
import { Log } from './log.interface';
import baseURL from '../config/baseURL';
const apiUrl = `${baseURL}api/log`;

@Injectable({
  providedIn: 'root'
})
export class LogService {

  constructor(private http: HttpClient, private errorHandingService: ErrorHandingService) { }

  getAll(page: number, limit: number): Observable<ResponseObject<PagedResult<Log>>> {
    return this.http.get<ResponseObject<PagedResult<Log>>>(`${apiUrl}?page=${page}&limit=${limit}`).pipe(
      catchError((error: HttpErrorResponse) => this.errorHandingService.getErrorObservable(error))
    )
  }

  getLogById(id: number): Observable<ResponseObject<Log>> {
    return this.http.get<ResponseObject<Log>>(`${apiUrl}/${id}`).pipe(
      catchError((error: HttpErrorResponse) => this.errorHandingService.getErrorObservable(error))
    )
  }
}
