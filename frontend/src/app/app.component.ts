import { Component, CUSTOM_ELEMENTS_SCHEMA, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { filter, forkJoin, map } from 'rxjs';
import { NgProgressbar } from 'ngx-progressbar';
import { NgProgressRouter } from 'ngx-progressbar/router';
import { NgProgressHttp } from 'ngx-progressbar/http';
import { ToastModule } from 'primeng/toast';
import { RoleResponse } from '@services/role/role.interface';
import { RoleService } from '@services/role/role.service';
import { MessageService } from 'primeng/api';
import { UserLogin } from '@services/auth/auth.interface';
import { MenuRoleResponse } from '@services/menu/menu.interface';
import { PermissionRoleResponse } from '@services/permission/permission.interface';
import { PrimeModule } from '@modules/prime/prime.module';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    NgProgressbar,
    NgProgressRouter,
    NgProgressHttp,
    RouterOutlet,
    PrimeModule,
  ],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class AppComponent implements OnInit {

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private titleService: Title,
  ) { }

  ngOnInit() {
    this.setupTitle();
  }

  private setupTitle() {
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd),
      map(() => {
        let route = this.activatedRoute;
        while (route.firstChild) {
          route = route.firstChild;
        }
        return route.snapshot.data['title'] || 'Default Title';
      })
    ).subscribe(title => {
      this.titleService.setTitle(title);
    });
  }

  // Triển khai phương thức title
  title(newTitle: string) {
    this.titleService.setTitle(newTitle);
  }
}
