import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { UserLogin } from '@services/auth/auth.interface';
import { Token } from '@services/config/response';
import { StoreRouter } from '../../store/store.router';

export const adminGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const storeRouter = inject(StoreRouter); // Inject service

  storeRouter.setPreviousUrl(state.url);

  if (typeof window !== 'undefined') {
    const userInfo = localStorage.getItem('user');
    const refreshTokenString = localStorage.getItem('refreshToken');

    if (refreshTokenString) {
      const refreshToken: Token = JSON.parse(refreshTokenString) as Token;
      if (isTokenExpiringSoon(refreshToken.expires)) {
        console.log("refreshToken hết hạn rồi bye bye");
        clearLocalStorage(router);
        return false;
      }
    } else {
      clearLocalStorage(router);
      return false;
    }
    if (userInfo) {
      const user: UserLogin = JSON.parse(userInfo) as UserLogin
      if (user.roles.some(role => role.name != "user")) {
        return true;
      } else {
        router.navigate(['/']);
        return false;
      }
    }
    else {
      clearLocalStorage(router);
      return false;
    }
  } else {
    return false;
  }
};

function isTokenExpiringSoon(expiration: Date): boolean {
  const expiryTime = new Date(expiration).getTime();
  const currentTime = new Date().getTime();
  return expiryTime < currentTime;
};

function clearLocalStorage(router: Router) {
  localStorage.removeItem('accessToken');
  localStorage.removeItem('refreshToken');
  localStorage.removeItem('permissions');
  localStorage.removeItem('menus');
  localStorage.removeItem('user');
  router.navigate(['/login']);
};
