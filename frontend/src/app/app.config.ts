import { ApplicationConfig, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideClientHydration } from '@angular/platform-browser';
import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { progressInterceptor } from 'ngx-progressbar/http';
import { provideAnimations } from '@angular/platform-browser/animations';
import { authInterceptor } from './services/interceptor/auth.interceptor';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatTabsModule } from '@angular/material/tabs';
import { MatCheckboxModule } from '@angular/material/checkbox';


export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideClientHydration(),
    provideHttpClient(withFetch(), withInterceptors([authInterceptor, progressInterceptor])),
    provideAnimations(), provideAnimationsAsync(),
    // provideNgProgressRouter({
    //   startEvents: [GuardsCheckEnd],
    //   completeEvents: [NavigationEnd],
    //   minDuration: 1000,
    // }),
    importProvidersFrom(
      MatDialogModule,
      MatButtonModule,
      MatIconModule,
      MatFormFieldModule,
      MatInputModule,
      MatTabsModule,
      MatCheckboxModule
    )
  ]
};
