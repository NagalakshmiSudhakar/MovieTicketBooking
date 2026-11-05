
import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { MovieService } from '../../../services/movie.service';
import { Movie, CreateMovieRequest, UpdateMovieRequest } from '../../../models/movie';

@Component({
  selector: 'app-admin-movies',
  imports: [CommonModule, FormsModule],
  templateUrl: './movies.html',
  styleUrl: './movies.css'
})
export class Movies implements OnInit {

  movies = signal<Movie[]>([]);
  editingMovieId = signal<number | null>(null);

  errorMessage = signal('');
  successMessage = signal('');

  showAddForm = signal(false);
  isLoading = signal(false);
  isSaving = signal(false);

  newMovie: CreateMovieRequest = {
    title: '',
    description: '',
    durationMinutes: 0,
    releaseDate: '',
    language: '',
    genre: ''
  };

  constructor(private movieService: MovieService) { }

  ngOnInit(): void {
    this.loadMovies();
  }

  // Load all movies
  loadMovies(): void {

    this.isLoading.set(true);
    this.errorMessage.set('');

    this.movieService.getAllMovies().subscribe({

      next: (response) => {
        this.movies.set(response);
        this.isLoading.set(false);

        console.log('Admin movies loaded:', response);
      },

      error: (error) => {
        console.error('Error loading movies:', error);

        this.errorMessage.set('Unable to load movies. Please try again.');
        this.isLoading.set(false);
      }

    });
  }

  // Open Add Movie form
  openAddForm(): void {
    this.editingMovieId.set(null);
    this.resetForm();
    this.showAddForm.set(true);
    this.errorMessage.set('');
    this.successMessage.set('');
  }

  // Close Add Movie form
  closeAddForm(): void {
    this.showAddForm.set(false);
    this.editingMovieId.set(null);
    this.resetForm();
    this.errorMessage.set('');
  }
  // Add new movie
  addMovie(): void {

    this.errorMessage.set('');
    this.successMessage.set('');
    this.isSaving.set(true);

    this.movieService.createMovie(this.newMovie).subscribe({

      next: (response) => {

        console.log('Movie created:', response);

        this.successMessage.set('Movie added successfully!');
        this.isSaving.set(false);
        this.showAddForm.set(false);

        this.resetForm();
        this.loadMovies();

      },

      error: (error) => {

        console.error('POST Status:', error.status);
        console.error('POST Error:', error.error);
        console.error('POST Message:', error.message);

        this.errorMessage.set(
          error.error?.detail ||
          error.error?.title ||
          error.message ||
          'Unable to add movie.'
        );

        this.isSaving.set(false);
      }

    });
  }

  // Open form with existing movie details
  editMovie(movie: Movie): void {

    this.editingMovieId.set(movie.movieId);

    this.newMovie = {
      title: movie.title,
      description: movie.description,
      durationMinutes: movie.durationMinutes,
      releaseDate: movie.releaseDate.substring(0, 10),
      language: movie.language,
      genre: movie.genre
    };

    this.showAddForm.set(true);
    this.errorMessage.set('');
    this.successMessage.set('');
  }

  // Update existing movie
  updateMovie(): void {

    const movieId = this.editingMovieId();

    if (movieId === null) {
      return;
    }

    this.isSaving.set(true);
    this.errorMessage.set('');

    const request: UpdateMovieRequest = {
      ...this.newMovie
    };

    this.movieService.updateMovie(movieId, request).subscribe({

      next: () => {
        this.successMessage.set('Movie updated successfully!');
        this.isSaving.set(false);
        this.showAddForm.set(false);
        this.editingMovieId.set(null);
        this.resetForm();
        this.loadMovies();
      },

      error: (error) => {
        console.error('Error updating movie:', error);

        this.errorMessage.set(
          error.error?.detail || 'Unable to update movie.'
        );

        this.isSaving.set(false);
      }

    });
  }

  deleteMovie(movie: Movie): void {

    const confirmDelete = confirm(
      `Are you sure you want to delete "${movie.title}"?`
    );

    if (!confirmDelete) {
      return;
    }

    this.errorMessage.set('');
    this.successMessage.set('');

    this.movieService.deleteMovie(movie.movieId).subscribe({

      next: () => {
        this.successMessage.set(
          'Movie deleted successfully!'
        );

        this.loadMovies();
      },

      error: (error) => {
        console.error('Error deleting movie:', error);

        this.errorMessage.set(
          error.error?.detail ||
          error.error?.message ||
          'Unable to delete movie.'
        );
      }

    });
  }

  // Reset form
  resetForm(): void {

    this.newMovie = {
      title: '',
      description: '',
      durationMinutes: 0,
      releaseDate: '',
      language: '',
      genre: ''
    };

  }
}
