import { Routes } from '@angular/router';
// We import the component we just made
import { DeviceListComponent } from './components/device-list/device-list.component';
import { DeviceDetailComponent } from './components/device-detail/device-detail.component';

export const routes: Routes = [
  // This tells Angular: If the path is empty '', show the DeviceListComponent
  { path: '', component: DeviceListComponent },
  
  // Show only one with details
  { path: 'devices/:id', component: DeviceDetailComponent }
];