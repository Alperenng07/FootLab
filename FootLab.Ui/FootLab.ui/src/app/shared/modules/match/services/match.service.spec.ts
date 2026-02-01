import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Match } from '../models/match.model';

@Injectable({ providedIn: 'root' })
export class MatchService {
  private http = inject(HttpClient);
  private apiUrl = 'https://localhost:7000/api/matches'; // .NET API URL

  getDailyMatches(): Observable<Match[]> {
    return this.http.get<Match[]>(`${this.apiUrl}/daily`);
  }

  getAllMatches(): Observable<Match[]> {
    return this.http.get<Match[]>(this.apiUrl);
  }

  getMatchDetail(id: number): Observable<Match> {
    return this.http.get<Match>(`${this.apiUrl}/${id}`);
  }
}
