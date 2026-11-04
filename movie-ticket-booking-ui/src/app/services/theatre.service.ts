import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Theatre } from '../models/theatre';

@Injectable({
  providedIn: 'root'
})
export class TheatreService {

  private apiUrl = 'https://localhost:7286/api/theatres';

  constructor(private http: HttpClient) { }

  getAllTheatres(): Observable<Theatre[]> {
    return this.http.get<Theatre[]>(this.apiUrl);
  }
}
