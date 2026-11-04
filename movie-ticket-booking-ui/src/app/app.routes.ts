
import { Routes } from '@angular/router';

import { Home } from './components/home/home';
import { MovieDetails } from './components/movie-details/movie-details';
import { SeatSelection } from './components/seat-selection/seat-selection';

export const routes: Routes = [
  { path: '', component: Home },
  { path: 'movies/:id', component: MovieDetails },
  { path: 'shows/:id/seats', component: SeatSelection },
  { path: '**', redirectTo: '' }
];
