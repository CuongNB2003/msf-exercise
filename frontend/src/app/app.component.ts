import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { filter, map } from 'rxjs';
import { NgProgressbar } from 'ngx-progressbar';
import { NgProgressRouter } from 'ngx-progressbar/router';
import { NgProgressHttp } from 'ngx-progressbar/http';
import { NgProgress } from '@ngx-progressbar/core';
import { RefreshTokenResponse, Token } from './services/auth/auth.interface';
import { ResponseObject } from './services/config/response';
import { HttpClient } from '@angular/common/http';
import { AuthService } from './services/auth/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [NgProgressbar, NgProgressRouter, NgProgressHttp, RouterOutlet,],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'] // Sửa thành styleUrls
})
export class AppComponent implements OnInit {
  private isRefreshing = false;
  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private titleService: Title,
  ) { }

  ngOnInit() {
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
