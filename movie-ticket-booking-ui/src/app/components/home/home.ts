
import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { MovieService } from '../../services/movie.service';
import { Movie } from '../../models/movie';

@Component({
  selector: 'app-home',
  imports: [CommonModule, RouterLink],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class Home implements OnInit {

  movies = signal<Movie[]>([]);
  errorMessage = signal('');

  constructor(private movieService: MovieService) { }

  ngOnInit(): void {
    this.loadMovies();
  }

  // Load all movies
  loadMovies(): void {
    this.movieService.getAllMovies().subscribe({
      next: (data) => {
        this.movies.set(data);
        this.errorMessage.set('');
        console.log('Movies loaded:', data);
      },
      error: (error) => {
        console.error('Error loading movies:', error);
        this.errorMessage.set('Unable to load movies.');
      }
    });
  }

  // Search movies by title
  searchMovies(title: string): void {

    if (!title.trim()) {
      this.loadMovies();
      return;
    }

    this.movieService.searchMovies(title).subscribe({
      next: (data) => {
        this.movies.set(data);
        this.errorMessage.set('');
        console.log('Search results:', data);
      },
      error: (error) => {
        console.error('Error searching movies:', error);
        this.movies.set([]);
        this.errorMessage.set('No movies found.');
      }
    });
  }
}
