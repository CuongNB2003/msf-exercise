import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { User } from '../../services/auth/auth.interface';

export const AdminGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);

  if (typeof window !== 'undefined') {
    const userInfo = localStorage.getItem('user');

    if (userInfo) {
      const user: User = JSON.parse(userInfo) as User
      if (user.role.name == "admin") {
        return true;
      } else {
        localStorage.removeItem('accessToken');
        localStorage.removeItem('refreshToken');
        localStorage.removeItem('user');
        router.navigate(['/login']);
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
