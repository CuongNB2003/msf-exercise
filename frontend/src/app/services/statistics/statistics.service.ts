import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ResponseObject } from '@services/config/response';
import { ErrorHandingService } from '@services/error-handing/error-handing.service';
import { catchError, Observable } from 'rxjs';
import LogMethodByYear from './statistics.interface';
import LogMethodByMonth from './statistics.interface';
import RoleCountUser from './statistics.interface';
import { baseURL } from '@services/config/baseURL';
const apiUrl = `${baseURL}api/statistics`;

@Injectable({
  providedIn: 'root'
})
export class StatisticsService {
  constructor(private http: HttpClient, private errorHandingService: ErrorHandingService) { }

  getLogMethodByYear(startDate: Date, endDate: Date): Observable<ResponseObject<LogMethodByYear[]>> {
    const start = startDate.toLocaleDateString('en-CA'); // Định dạng YYYY-MM-DD
    const end = endDate.toLocaleDateString('en-CA'); // Định dạng YYYY-MM-DD
    return this.http.get<ResponseObject<LogMethodByYear[]>>(
      `${apiUrl}/log-method-by-year?startDate=${start}&endDate=${end}`
    ).pipe(
      catchError((error: HttpErrorResponse) => this.errorHandingService.getErrorObservable(error))
    );
  }

  getLogMethodByMonth(searchDate: Date): Observable<ResponseObject<LogMethodByMonth[]>> {
    const start = searchDate.toLocaleDateString('en-CA'); // Định dạng YYYY-MM-DD
    return this.http.get<ResponseObject<LogMethodByMonth[]>>(
      `${apiUrl}/log-method-by-month?searchDate=${start}`
    ).pipe(
      catchError((error: HttpErrorResponse) => this.errorHandingService.getErrorObservable(error))
    );
  }

  getRoleCountUser(): Observable<ResponseObject<RoleCountUser[]>> {
    return this.http.get<ResponseObject<RoleCountUser[]>>(
      `${apiUrl}/role-count-user`
    ).pipe(
      catchError((error: HttpErrorResponse) => this.errorHandingService.getErrorObservable(error))
    );
  }

}
