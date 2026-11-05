
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  Movie,
  CreateMovieRequest,
  UpdateMovieRequest
} from '../models/movie';

@Injectable({
  providedIn: 'root'
})
export class MovieService {

  private apiUrl = 'https://localhost:7286/api/movies';

  constructor(private http: HttpClient) { }

  // Get all movies
  getAllMovies(): Observable<Movie[]> {
    return this.http.get<Movie[]>(this.apiUrl);
  }

  // Get movie by ID
  getMovieById(id: number): Observable<Movie> {
    return this.http.get<Movie>(`${this.apiUrl}/${id}`);
  }

  // Search movies by title
  searchMovies(title: string): Observable<Movie[]> {
    return this.http.get<Movie[]>(
      `${this.apiUrl}/search?title=${encodeURIComponent(title)}`
    );
  }

  // Create a new movie (Admin only)
  createMovie(request: CreateMovieRequest): Observable<Movie> {
    return this.http.post<Movie>(this.apiUrl, request);
  }

  // Update an existing movie (Admin only)
  updateMovie(
    id: number,
    request: UpdateMovieRequest
  ): Observable<Movie> {
    return this.http.put<Movie>(`${this.apiUrl}/${id}`, request);
  }

  // Delete a movie (Admin only)
  deleteMovie(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
