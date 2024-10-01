
import { Routes } from '@angular/router';
import { LayoutComponent } from './components/layout/layout.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { AuthGuard } from './guards/auth/auth.guard';
import { AdminComponent } from './components/admin/admin.component';
import { AdminGuard } from './guards/admin/admin.guard';
import { TitleService } from './services/title/title.service';
import { RoleComponent } from './components/role/role.component';
import { UserComponent } from './components/user/user.component';

export function getTitle(titleService: TitleService, suffix: string): string {
    return titleService.getTitle(suffix);
}

export const routes: Routes = [
    {
        path: '',
        component: LayoutComponent,
        canActivate: [AuthGuard],
        children: [
            {
                path: '',
                component: HomeComponent,
                data: { title: getTitle(new TitleService(), 'Trang Chủ') }
            },

        ]
    },
    {
        path: 'admin',
        component: AdminComponent,
        canActivate: [AdminGuard],
        children: [
            {
                path: 'role',
                component: RoleComponent,
                data: { title: getTitle(new TitleService(), 'Role') }
            },
            {
                path: 'user',
                component: UserComponent,
                data: { title: getTitle(new TitleService(), 'User') }
            },

        ]
    },
    {
        path: 'login',
        component: LoginComponent,
        data: { title: getTitle(new TitleService(), 'Đăng nhập') }

    },
    {
        path: 'register',
        component: RegisterComponent,
        data: { title: getTitle(new TitleService(), 'Đăng ký') }

    },
    {
        path: '**',
        redirectTo: 'login',
    }
];