
import { Routes } from '@angular/router';
import { LayoutComponent } from './components/layout/layout.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { AuthGuard } from './guards/auth/auth.guard';


export const routes: Routes = [
    {
        path: '',
        component: LayoutComponent,
        canActivate: [AuthGuard],
        children: [
            {
                path: '',
                component: HomeComponent,
                data: { title: 'CuongNB | Trang Chủ' }
            },

        ]
    },
    {
        path: 'login',
        component: LoginComponent,
        data: { title: 'CuongNB | Đăng nhập' }
    },
    {
        path: 'register',
        component: RegisterComponent,
        data: { title: 'CuongNB | Đăng ký' }
    },
];