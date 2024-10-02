import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { User } from '../../services/auth/auth.interface';

export const AuthGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);

  if (typeof window !== 'undefined') {
    const userInfo = localStorage.getItem('user');

    if (userInfo) {
      const user: User = JSON.parse(userInfo) as User
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
