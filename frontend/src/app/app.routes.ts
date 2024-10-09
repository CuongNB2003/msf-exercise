
import { Routes } from '@angular/router';
import { TitleService } from './services/title/title.service';
import { LayoutUserComponent } from './components/layout/layout-user/layout-user.component';
import { AuthGuard } from './guards/auth/auth.guard';
import { AdminGuard } from './guards/admin/admin.guard';
import { HomeComponent } from '@components/client/home/home.component';
import { LayoutAdminComponent } from '@components/layout/layout-admin/layout-admin.component';
import { HomeAdminComponent } from '@components/admin/home-admin/home-admin.component';
import { DashboardComponent } from '@components/admin/dashboard/dashboard.component';
import { RoleListComponent } from '@components/admin/role/role-list/role-list.component';
import { UserListComponent } from '@components/admin/user/user-list/user-list.component';
import { LogListComponent } from '@components/admin/log/log-list/log-list.component';
import { LoginComponent } from '@components/auth/login/login.component';
import { RegisterComponent } from '@components/auth/register/register.component';



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
                component: RoleListComponent,
                data: { title: getTitle(new TitleService(), 'Roles') }
            },
            {
                path: 'users',
                component: UserListComponent,
                data: { title: getTitle(new TitleService(), 'Users') }
            },
            {
                path: 'logs',
                component: LogListComponent,
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