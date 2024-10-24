import { ApplicationConfig, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideClientHydration } from '@angular/platform-browser';
import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { progressInterceptor } from 'ngx-progressbar/http';
import { BrowserAnimationsModule, provideAnimations } from '@angular/platform-browser/animations';
import { authInterceptor } from './services/interceptor/auth.interceptor';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { MessageService } from 'primeng/api';
import { provideCharts, withDefaultRegisterables } from 'ng2-charts';


export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideClientHydration(),
    provideHttpClient(withFetch(), withInterceptors([authInterceptor, progressInterceptor])),
    provideAnimations(),
    provideAnimationsAsync(),
    MessageService,
    BrowserAnimationsModule,
    provideCharts(withDefaultRegisterables())
  ]
};
