import { Routes } from '@angular/router';
// We import the component we just made
import { DeviceListComponent } from './components/device-list/device-list.component';
import { DeviceDetailComponent } from './components/device-detail/device-detail.component';
import { DeviceCreateComponent } from './components/device-create/device-create.component';
import { DeviceEditComponent } from './components/device-edit/device-edit.component';
export const routes: Routes = [
  // This tells Angular: If the path is empty '', show the DeviceListComponent
  { path: '', component: DeviceListComponent },

  // Show only one with details
  { path: 'devices/new', component: DeviceCreateComponent },
  { path: 'devices/:id', component: DeviceDetailComponent },
  { path: 'devices/:id/edit', component: DeviceEditComponent },
];
