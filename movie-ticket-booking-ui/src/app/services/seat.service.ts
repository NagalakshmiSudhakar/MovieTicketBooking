
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Seat } from '../models/seat';

@Injectable({
  providedIn: 'root'
})
export class SeatService {

  private apiUrl = 'https://localhost:7286/api/shows';

  constructor(private http: HttpClient) { }

  getSeatsByShowId(showId: number): Observable<Seat[]> {
    return this.http.get<Seat[]>(
      `${this.apiUrl}/${showId}/seats`
    );
  }
}
