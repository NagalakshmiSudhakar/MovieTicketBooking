import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { forkJoin } from 'rxjs';

import { ShowService } from '../../services/show.service';
import { MovieService } from '../../services/movie.service';
import { TheatreService } from '../../services/theatre.service';
import { ScreenService } from '../../services/screen.service';

import { Show } from '../../models/show';
import { Movie } from '../../models/movie';
import { Theatre } from '../../models/theatre';
import { Screen } from '../../models/screen';

interface TheatreShowGroup {
  theatre: Theatre;
  shows: Show[];
}

@Component({
  selector: 'app-movie-details',
  imports: [CommonModule, RouterLink],
  templateUrl: './movie-details.html',
  styleUrl: './movie-details.css',
})
export class MovieDetails implements OnInit {

  movie = signal<Movie | null>(null);
  errorMessage = signal('');

  shows = signal<Show[]>([]);
  theatreGroups = signal<TheatreShowGroup[]>([]);

  constructor(
    private route: ActivatedRoute,
    private movieService: MovieService,
    private showService: ShowService,
    private theatreService: TheatreService,
    private screenService: ScreenService
  ) { }

  ngOnInit(): void {

    const id = Number(this.route.snapshot.paramMap.get('id'));

    if (!id || id <= 0) {
      this.errorMessage.set('Invalid movie ID.');
      return;
    }

    this.movieService.getMovieById(id).subscribe({
      next: data => {
        this.movie.set(data);
      },
      error: error => {
        console.error('Error loading movie details:', error);
        this.errorMessage.set('Movie not found.');
      }
    });

    this.loadShows(id);
  }

  loadShows(movieId: number): void {

    this.showService.getShowsByMovieId(movieId).subscribe({

      next: showData => {

        this.shows.set(showData);

        forkJoin({
          theatres: this.theatreService.getAllTheatres(),
          screens: this.screenService.getAllScreens()
        }).subscribe({

          next: result => {

            const groups: TheatreShowGroup[] = [];

            result.theatres.forEach(theatre => {

              const theatreScreens = result.screens.filter(
                screen => screen.theatreId === theatre.theatreId
              );

              const theatreShows = showData.filter(show =>
                theatreScreens.some(
                  screen => screen.screenId === show.screenId
                )
              );

              if (theatreShows.length > 0) {
                groups.push({
                  theatre: theatre,
                  shows: theatreShows
                });
              }
            });

            this.theatreGroups.set(groups);
          },

          error: error => {
            console.error('Error loading theatres and screens:', error);
            this.errorMessage.set('Unable to load theatre details.');
          }

        });
      },

      error: error => {
        console.error('Error loading shows:', error);
        this.errorMessage.set('Unable to load shows.');
      }

    });
  }
}
