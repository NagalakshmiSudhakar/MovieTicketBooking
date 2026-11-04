import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Screen } from '../models/screen';

@Injectable({
  providedIn: 'root'
})
export class ScreenService {

  private apiUrl = 'https://localhost:7286/api/screens';

  constructor(private http: HttpClient) { }

  getAllScreens(): Observable<Screen[]> {
    return this.http.get<Screen[]>(this.apiUrl);
  }
}
