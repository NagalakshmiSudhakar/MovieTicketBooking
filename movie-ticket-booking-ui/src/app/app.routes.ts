import { Routes } from '@angular/router';

import { Register } from './components/auth/register/register';
import { Login } from './components/auth/login/login';
import { Dashboard } from './components/admin/dashboard/dashboard';
import { Movies } from './components/admin/movies/movies';
import { authGuard } from './guards/auth-guard';
import { roleGuard } from './guards/role-guard';
import { Home } from './components/home/home';
import { MovieDetails } from './components/movie-details/movie-details';
import { SeatSelection } from './components/seat-selection/seat-selection';

export const routes: Routes = [

  { path: 'register', component: Register },
  { path: 'login', component: Login },
  {
    path: 'admin',
    component: Dashboard,
    canActivate: [authGuard, roleGuard],
    data: { role: 'Admin' }
  },
  {
    path: 'admin/movies',
    component: Movies,
    canActivate: [authGuard, roleGuard],
    data: { role: 'Admin' }
  },
  { path: '', component: Home },

  { path: 'movies/:id', component: MovieDetails },

  { path: 'shows/:id/seats', component: SeatSelection },

  { path: '**', redirectTo: '' }

];
