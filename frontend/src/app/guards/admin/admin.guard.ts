import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { UserLogin } from '../../services/auth/auth.interface';

export const AdminGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);

  if (typeof window !== 'undefined') {
    const userInfo = localStorage.getItem('user');

    if (userInfo) {
      const user: UserLogin = JSON.parse(userInfo) as UserLogin
      if (user.role.name == "admin") {
        return true;
      } else {
        router.navigate(['/']);
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
