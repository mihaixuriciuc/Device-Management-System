import { Routes } from '@angular/router';
// We import the component we just made
import { DeviceListComponent } from './components/device-list/device-list.component';

export const routes: Routes = [
  // This tells Angular: If the path is empty '', show the DeviceListComponent
  { path: '', component: DeviceListComponent },
  
  // Optional: If the user types /devices, show the same list
  { path: 'devices', component: DeviceListComponent }
];