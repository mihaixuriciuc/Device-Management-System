import { Routes } from '@angular/router';

import { DeviceListComponent } from './components/device-list/device-list.component';
import { DeviceDetailComponent } from './components/device-detail/device-detail.component';
import { DeviceCreateComponent } from './components/device-create/device-create.component';
import { DeviceEditComponent } from './components/device-edit/device-edit.component';
import { HomePageComponent } from './components/home-page/home-page.component';
import { RegisterComponent } from './components/register/register.component';
import { LoginComponent } from './components/login/login.component';
import { authGuard } from './guards/auth-guard';
import { ProfileComponent } from './components/profile-page/profile-page.component';

export const routes: Routes = [
  // Public routes
  { path: '', component: HomePageComponent },
  { path: 'login', component: LoginComponent }, // ← No guard
  { path: 'register', component: RegisterComponent }, // ← No guard

  // Protected routes
  { path: 'devices', component: DeviceListComponent, canActivate: [authGuard] },
  {
    path: 'devices/new',
    component: DeviceCreateComponent,
    canActivate: [authGuard],
  },
  {
    path: 'devices/:id',
    component: DeviceDetailComponent,
    canActivate: [authGuard],
  },
  {
    path: 'devices/:id/edit',
    component: DeviceEditComponent,
    canActivate: [authGuard],
  },

  {
    path: 'profile',
    component: ProfileComponent,
    canActivate: [authGuard],
  },

  // Catch-all
  { path: '**', redirectTo: '' },
];
