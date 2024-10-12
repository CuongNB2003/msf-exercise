import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import baseURL from '@services/config/baseURL';
import { PagedResult, ResponseObject, ResponseText } from '@services/config/response';
import { ErrorHandingService } from '@services/error-handing/error-handing.service';
import { catchError, Observable } from 'rxjs';
import { MenuCreateInput, MenuResponse, MenuUpdateInput } from './menu.interface';
const apiUrl = `${baseURL}api/menu`;

@Injectable({
  providedIn: 'root'
})
export class MenuService {
  constructor(private http: HttpClient, private errorHandingService: ErrorHandingService) { }

  getMenuAll(page: number, limit: number): Observable<ResponseObject<PagedResult<MenuResponse>>> {
    return this.http.get<ResponseObject<PagedResult<MenuResponse>>>(`${apiUrl}?page=${page}&limit=${limit}`).pipe(
      catchError((error: HttpErrorResponse) => this.errorHandingService.getErrorObservable(error))
    )
  }

  getMenuById(id: number): Observable<ResponseObject<MenuResponse>> {
    return this.http.get<ResponseObject<MenuResponse>>(`${apiUrl}/${id}`).pipe(
      catchError((error: HttpErrorResponse) => this.errorHandingService.getErrorObservable(error))
    )
  }

  createMenu(input: MenuCreateInput): Observable<ResponseText> {
    return this.http.post<ResponseText>(`${apiUrl}`, input).pipe(
      catchError((error: HttpErrorResponse) => this.errorHandingService.getErrorObservable(error))
    )
  }

  updateMenu(input: MenuUpdateInput, id: number): Observable<ResponseText> {
    return this.http.put<ResponseText>(`${apiUrl}/${id}`, input).pipe(
      catchError((error: HttpErrorResponse) => this.errorHandingService.getErrorObservable(error))
    )
  }

  deleteMenu(id: number): Observable<ResponseText> {
    return this.http.delete<ResponseText>(`${apiUrl}/${id}`).pipe(
      catchError((error: HttpErrorResponse) => this.errorHandingService.getErrorObservable(error))
    )
  }
}
