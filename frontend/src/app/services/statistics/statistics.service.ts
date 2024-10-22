import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import baseURL from '@services/config/baseURL';
import { ResponseObject } from '@services/config/response';
import { ErrorHandingService } from '@services/error-handing/error-handing.service';
import { catchError, Observable } from 'rxjs';
import LogMethodStatistics from './statistics.interface';
import RoleCountUserStatistics from './statistics.interface';

const apiUrl = `${baseURL}api/statistics`;

@Injectable({
  providedIn: 'root'
})
export class StatisticsService {
  constructor(private http: HttpClient, private errorHandingService: ErrorHandingService) { }

  statisticLogMethod(startDate: Date, endDate: Date): Observable<ResponseObject<LogMethodStatistics[]>> {
    const start = startDate.toLocaleDateString('en-CA'); // Định dạng YYYY-MM-DD
    const end = endDate.toLocaleDateString('en-CA'); // Định dạng YYYY-MM-DD

    return this.http.get<ResponseObject<LogMethodStatistics[]>>(
      `${apiUrl}/log-method?startDate=${start}&endDate=${end}`
    ).pipe(
      catchError((error: HttpErrorResponse) => this.errorHandingService.getErrorObservable(error))
    );
  }

  statisticRoleCountUser(): Observable<ResponseObject<RoleCountUserStatistics[]>> {
    return this.http.get<ResponseObject<RoleCountUserStatistics[]>>(
      `${apiUrl}/role-count-user`
    ).pipe(
      catchError((error: HttpErrorResponse) => this.errorHandingService.getErrorObservable(error))
    );
  }

}
