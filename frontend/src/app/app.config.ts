import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core'; // Add provideZoneChangeDetection here
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    // This is the line that fixes the NG0908 error:
    provideZoneChangeDetection({ eventCoalescing: true }), 
    provideRouter(routes),
    provideHttpClient()
  ]
};