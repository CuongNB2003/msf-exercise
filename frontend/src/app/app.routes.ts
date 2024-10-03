
import { Routes } from '@angular/router';
import { TitleService } from './services/title/title.service';
import { LayoutUserComponent } from './components/layout/layout-user/layout-user.component';
import { AuthGuard } from './guards/auth/auth.guard';
import { HomeComponent } from './components/client/home/home.component';
import { LayoutAdminComponent } from './components/layout/layout-admin/layout-admin.component';
import { AdminGuard } from './guards/admin/admin.guard';
import { RoleComponent } from './components/admin/role/role.component';
import { UserComponent } from './components/admin/user/user.component';
import { LogComponent } from './components/admin/log/log.component';
import { LoginComponent } from './components/auth/login/login.component';
import { RegisterComponent } from './components/auth/register/register.component';
import { DashboardComponent } from './components/admin/dashboard/dashboard.component';
import { HomeAdminComponent } from './components/admin/home-admin/home-admin.component';

export function getTitle(titleService: TitleService, suffix: string): string {
    return titleService.getTitle(suffix);
}

export const routes: Routes = [
    {
        path: '',
        component: LayoutUserComponent,
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
        component: LayoutAdminComponent,
        canActivate: [AdminGuard],
        children: [
            {
                path: '',
                component: HomeAdminComponent,
                data: { title: getTitle(new TitleService(), 'Admin') }
            },
            {
                path: 'dashboard',
                component: DashboardComponent,
                data: { title: getTitle(new TitleService(), 'Dashboard') }
            },
            {
                path: 'roles',
                component: RoleComponent,
                data: { title: getTitle(new TitleService(), 'Roles') }
            },
            {
                path: 'users',
                component: UserComponent,
                data: { title: getTitle(new TitleService(), 'Users') }
            },
            {
                path: 'logs',
                component: LogComponent,
                data: { title: getTitle(new TitleService(), 'Logs') }
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
    // {
    //     path: '**',
    //     redirectTo: 'admin',
    // }
];