import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TitleService {
  private baseTitle = 'MSF';

  getTitle(suffix: string): string {
    return `${this.baseTitle} | ${suffix}`;
  }
}
