
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Show } from '../models/show';

@Injectable({
  providedIn: 'root'
})
export class ShowService {

  private apiUrl = 'https://localhost:7286/api/shows';

  constructor(private http: HttpClient) { }

  getShowsByMovieId(movieId: number): Observable<Show[]> {
    return this.http.get<Show[]>(
      `${this.apiUrl}/movie/${movieId}`
    );
  }
}
