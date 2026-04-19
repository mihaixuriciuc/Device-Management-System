import { Component } from '@angular/core';
import { RouterModule } from '@angular/router'; // <-- CRITICAL

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterModule], // <-- CRITICAL
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.scss',
})
export class HomePageComponent {}
