import { ApplicationConfig, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideClientHydration } from '@angular/platform-browser';
import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { progressInterceptor } from 'ngx-progressbar/http';
import { provideAnimations } from '@angular/platform-browser/animations';
import { authInterceptor } from './services/interceptor/auth.interceptor';


export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideClientHydration(),
    provideHttpClient(withFetch(), withInterceptors([authInterceptor, progressInterceptor])),
    provideAnimations(),
    // provideNgProgressRouter({
    //   startEvents: [GuardsCheckEnd],
    //   completeEvents: [NavigationEnd],
    //   minDuration: 1000,
    // }),
  ]
};
