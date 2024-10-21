import { StoreRouter } from './../../store/store.router';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-not-found',
  standalone: true,
  imports: [],
  templateUrl: './not-found.component.html',
  styleUrls: ['./not-found.component.scss']
})
export class NotFoundComponent {
  constructor(private router: Router, private storeRouter: StoreRouter) { }

  goBack(): void {
    const previousUrl = this.storeRouter.getPreviousUrl();
    if (previousUrl === '/admin') {
      previousUrl.split('/')[0]
    }

    console.log("hihi: ", previousUrl);

    if (previousUrl) {
      this.router.navigateByUrl(previousUrl);
      this.storeRouter.clearPreviousUrl();
    } else {
      this.router.navigate(['..']);
      this.storeRouter.clearPreviousUrl();
    }
  }
}
