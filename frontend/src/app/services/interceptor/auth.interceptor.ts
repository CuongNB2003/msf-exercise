import { HttpInterceptorFn } from '@angular/common/http';
import { RefreshTokenResponse } from '../auth/auth.interface';
import { ResponseObject, Token } from '../config/response';
import { inject } from '@angular/core';
import { switchMap, catchError, filter, take } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { AuthService } from '../auth/auth.service';

let isRefreshing = false;
const refreshTokenSubject: Subject<string> = new Subject<string>();

export const authInterceptor: HttpInterceptorFn = (request, next) => {
  const authService = inject(AuthService);
  const accessTokenString = localStorage.getItem('accessToken');
  const refreshTokenString = localStorage.getItem('refreshToken');
  // loại bỏ những api không cần thêm token vào header
  const excludedEndpoints = ['/login', '/register', '/refresh-token'];
  const isExcludedEndpoint = excludedEndpoints.some(endpoint => request.url.includes(endpoint));

  if (!accessTokenString || isExcludedEndpoint || !refreshTokenString) {
    return next(request);
  }

  const accessToken: Token = JSON.parse(accessTokenString) as Token;
  const refreshToken: Token = JSON.parse(refreshTokenString) as Token;

  if (isTokenExpiringSoon(accessToken.expires)) {
    if (!isRefreshing) {
      isRefreshing = true;
      return authService.refreshToken(refreshToken.token).pipe(
        switchMap((response: ResponseObject<RefreshTokenResponse>) => {
          isRefreshing = false;
          localStorage.setItem('accessToken', JSON.stringify(response.data.accessToken));
          localStorage.setItem('refreshToken', JSON.stringify(response.data.refreshToken));
          console.log("Token hết hạn rồi đang call nhá");
          refreshTokenSubject.next(response.data.accessToken.token);
          return next(handleClonedRequest(request, response.data.accessToken.token));
        }),
        catchError((error) => {
          isRefreshing = false;
          refreshTokenSubject.error(error);
          return next(request);
        })
      );
    }

    return refreshTokenSubject.pipe(
      filter(token => token != null),
      take(1),
      switchMap((token) => next(handleClonedRequest(request, token)))
    );
  }

  return next(handleClonedRequest(request, accessToken.token));
};

function handleClonedRequest(request: any, token: string) {
  const clonedRequest = request.clone({
    setHeaders: {
      Authorization: `${token}`
    }
  });
  return clonedRequest;
}

function isTokenExpiringSoon(expiration: string, bufferTime: number = 30000): boolean {
  const expiryTime = new Date(expiration).getTime();
  const currentTime = new Date().getTime();
  return (expiryTime - currentTime) < bufferTime;
}
