import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { StandingTeam } from '../models/standingteam.model';

@Injectable({
  providedIn: 'root'
})
export class StandingService {
  private http = inject(HttpClient);
  // Backend URL'ini kendi ortamına göre güncelle
  private apiUrl = 'https://localhost:7000/api/standings';

  /**
   * Belirli bir haftaya ait puan durumunu getirir
   * @param weekNumber Çekilmek istenen hafta (Örn: 22)
   */
  getStandingsByWeek(weekNumber: number): Observable<StandingTeam[]> {
    return this.http.get<StandingTeam[]>(`${this.apiUrl}?week=${weekNumber}`);
  }

  /**
   * Mevcut (aktif) haftanın puan durumunu getirir
   */
  getCurrentStandings(): Observable<StandingTeam[]> {
    return this.http.get<StandingTeam[]>(`${this.apiUrl}/current`);
  }
}
