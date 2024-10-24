import { HttpInterceptorFn } from '@angular/common/http';
import { RefreshTokenResponse } from '../auth/auth.interface';
import { ResponseObject, Token } from '../config/response';
import { inject } from '@angular/core';
import { switchMap, catchError, filter, take } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { AuthService } from '../auth/auth.service';
import { Router } from '@angular/router';

let isRefreshing = false;
const refreshTokenSubject: Subject<string> = new Subject<string>();

export const authInterceptor: HttpInterceptorFn = (request, next) => {
  const authService = inject(AuthService);
  const router = inject(Router)
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
        switchMap((res: ResponseObject<RefreshTokenResponse>) => {
          isRefreshing = false;
          localStorage.setItem('accessToken', JSON.stringify(res.data.accessToken));
          localStorage.setItem('refreshToken', JSON.stringify(res.data.refreshToken));
          console.log("Token cũ", refreshToken.expires);
          console.log("Token cũ", res.data.refreshToken.expires);
          console.log("Token hết hạn rồi đang call nhá");
          refreshTokenSubject.next(res.data.accessToken.token);
          return next(handleClonedRequest(request, res.data.accessToken.token));
        }),
        catchError((error) => {
          isRefreshing = false;
          clearLocalStorage(router)
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

function isTokenExpiringSoon(expiration: Date): boolean {
  const expiryTime = new Date(expiration).getTime();
  const currentTime = new Date().getTime();
  return expiryTime < currentTime;
}

function clearLocalStorage(router: Router) {
  localStorage.removeItem('accessToken');
  localStorage.removeItem('refreshToken');
  localStorage.removeItem('permissions');
  localStorage.removeItem('menus');
  localStorage.removeItem('user');
  router.navigate(['/login']);
};
