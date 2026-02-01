
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Match } from '../models/match.model';
import { Injectable } from '@angular/core';
import { inject } from '@angular/core/primitives/di';

@Injectable({ providedIn: 'root' })
export class MatchService {
  private http = inject(HttpClient);
  private apiUrl = 'https://localhost:7000/api/matches'; // API endpoint'in

  getDailyMatches(): Observable<Match[]> {
    return this.http.get<Match[]>(`${this.apiUrl}/daily`);
  }

  getFixtures(): Observable<Match[]> {
    return this.http.get<Match[]>(`${this.apiUrl}/fixtures`);
  }

  getMatchById(id: number): Observable<Match> {
    return this.http.get<Match>(`${this.apiUrl}/${id}`);
  }
  getAllMatches(): Observable<Match[]> {
    return this.http.get<Match[]>(this.apiUrl);
  }
}
