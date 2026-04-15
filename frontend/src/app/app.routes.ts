import { Routes } from '@angular/router';

// Your existing Device components
import { DeviceListComponent } from './components/device-list/device-list.component';
import { DeviceDetailComponent } from './components/device-detail/device-detail.component';
import { DeviceCreateComponent } from './components/device-create/device-create.component';
import { DeviceEditComponent } from './components/device-edit/device-edit.component';

// The new Home component (make sure the path matches where you generated it)
import { HomePageComponent } from './components/home-page/home-page.component';
import { RegisterComponent } from './components/register/register.component';
import { LoginComponent } from './components/login/login.component';

// We will create these next!
// import { LoginComponent } from './components/login/login.component';
// import { RegisterComponent } from './components/register/register.component';

export const routes: Routes = [
  // 1. THE FRONT DOOR: Show the Home Page by default
  { path: '', component: HomePageComponent },

  // 2. AUTH ROUTES (Uncomment these once we generate the components)
  // { path: 'login', component: LoginComponent },
  // { path: 'register', component: RegisterComponent },

  // 3. THE SECURE ZONE: Move the device list to '/devices'
  { path: 'devices', component: DeviceListComponent },
  { path: 'devices/new', component: DeviceCreateComponent },
  { path: 'devices/:id', component: DeviceDetailComponent },
  { path: 'devices/:id/edit', component: DeviceEditComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'login', component: LoginComponent },

  // 4. THE CATCH-ALL: If a user types a weird URL, send them back to Home
  { path: '**', redirectTo: '' },
];
