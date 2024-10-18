import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { UserLogin } from '@services/auth/auth.interface';
import { Token } from '@services/config/response';
import { StoreRouter } from '../../store/store.router';

export const AuthGuard: CanActivateFn = (route, state) => {
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
        clearLocalStorage();
        router.navigate(['/login']);
        return false;
      }
    } else {
      clearLocalStorage();
      router.navigate(['/login']);
      return false;
    }

    if (userInfo) {
      const user: UserLogin = JSON.parse(userInfo) as UserLogin;
      const isAdmin = user.roles.some(role => role.name !== "user");
      const isAdminRoute = route.url.some(segment => segment.path === 'admin');

      // Lưu URL trước đó
      // storeRouter.setPreviousUrl(state.url);

      console.log("url: ", state.url);


      // Kiểm tra quyền truy cập
      if (isAdmin && isAdminRoute) {
        return true;
      }

      if (!isAdmin && isAdminRoute) {
        router.navigate(['/']);
        return false;
      }

      return true; // Cho phép người dùng bình thường truy cập vào các route khác
    } else {
      clearLocalStorage();
      router.navigate(['/login']);
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
}

function clearLocalStorage() {
  localStorage.removeItem('accessToken');
  localStorage.removeItem('refreshToken');
  localStorage.removeItem('permissions');
  localStorage.removeItem('menus');
  localStorage.removeItem('user');
}
