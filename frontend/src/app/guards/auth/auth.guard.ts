import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { UserLogin } from '../../services/auth/auth.interface';

export const AuthGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);

  if (typeof window !== 'undefined') {
    const userInfo = localStorage.getItem('user');

    if (userInfo) {
      const user: UserLogin = JSON.parse(userInfo) as UserLogin
      if (user.role.name == "user") {
        return true;
      } else {
        router.navigate(['/admin']);
        return false;
      }
    } else {
      router.navigate(['/login']);
      return false;
    }
  } else {
    return false;
  }
};
