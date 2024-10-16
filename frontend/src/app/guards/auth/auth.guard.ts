import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { Token } from '@services/config/response';
import { UserLogin } from '@services/auth/auth.interface';

export const AuthGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);

  if (typeof window !== 'undefined') {
    const userInfo = localStorage.getItem('user');
    const refreshTokenString = localStorage.getItem('refreshToken');

    if (refreshTokenString) {
      const refreshToken: Token = JSON.parse(refreshTokenString) as Token;
      if (isTokenExpiringSoon(refreshToken.expires)) {
        console.log("refreshToken hết hạn rồi bye bye");

        localStorage.removeItem('accessToken');
        localStorage.removeItem('refreshToken');
        localStorage.removeItem('user');
        router.navigate(['/login']);
        return false;
      }
    } else {
      localStorage.removeItem('accessToken');
      localStorage.removeItem('refreshToken');
      localStorage.removeItem('user');
      router.navigate(['/login']);
      return false;
    }
    if (userInfo) {
      //const user: UserLogin = JSON.parse(userInfo) as UserLogin
      // if (user.role.name == "user") {
      //   return true;
      // } else {
      //   router.navigate(['/admin']);
      //   return false;
      // }
      return true
    } else {
      localStorage.removeItem('accessToken');
      localStorage.removeItem('refreshToken');
      localStorage.removeItem('user');
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