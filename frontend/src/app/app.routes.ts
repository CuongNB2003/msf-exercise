
import { Routes } from '@angular/router';
import { TitleService } from './services/title/title.service';
import { LayoutUserComponent } from './components/layout/layout-user/layout-user.component';
import { authGuard } from './guards/auth/auth.guard';
import { HomeComponent } from '@components/client/home/home.component';
import { LayoutAdminComponent } from '@components/layout/layout-admin/layout-admin.component';
import { HomeAdminComponent } from '@components/admin/home-admin/home-admin.component';
import { DashboardComponent } from '@components/admin/dashboard/dashboard.component';
import { RoleListComponent } from '@components/admin/role/role-list/role-list.component';
import { UserListComponent } from '@components/admin/user/user-list/user-list.component';
import { LogListComponent } from '@components/admin/log/log-list/log-list.component';
import { LoginComponent } from '@components/auth/login/login.component';
import { RegisterComponent } from '@components/auth/register/register.component';
import { MenuListComponent } from '@components/admin/menu/menu-list/menu-list.component';
import { PermissionListComponent } from '@components/admin/permission/permission-list/permission-list.component';
import { menuGuard } from '@guards/menu/menu.guard';
import { NotFoundComponent } from '@ui/not-found/not-found.component';
import { adminGuard } from '@guards/admin/admin.guard';



export function getTitle(titleService: TitleService, suffix: string): string {
    return titleService.getTitle(suffix);
}

export const routes: Routes = [
    {
        path: '',
        component: LayoutUserComponent,
        canActivate: [authGuard],
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
        canActivate: [adminGuard],
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
                data: { title: getTitle(new TitleService(), 'Roles') },
                canActivate: [menuGuard]
            },
            {
                path: 'users',
                component: UserListComponent,
                data: { title: getTitle(new TitleService(), 'Users') },
                canActivate: [menuGuard]
            },
            {
                path: 'logs',
                component: LogListComponent,
                data: { title: getTitle(new TitleService(), 'Logs') },
                canActivate: [menuGuard]
            },
            {
                path: 'menu',
                component: MenuListComponent,
                data: { title: getTitle(new TitleService(), 'Menu') },
                canActivate: [menuGuard]
            },
            {
                path: 'permissions',
                component: PermissionListComponent,
                data: { title: getTitle(new TitleService(), 'Permissions') },
                canActivate: [menuGuard]
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
        path: 'not-found',
        component: NotFoundComponent,
        data: { title: getTitle(new TitleService(), 'Not Found') }
    },
    { path: '**', redirectTo: 'not-found' }
];