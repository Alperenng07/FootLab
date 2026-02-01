// match.model.ts
export interface Match {
  id: number;
  time: string;
  date: string;
  homeTeamName: string;
  awayTeamName: string;
  homeTeamLogoUrl: string; // "Url" ekledik
  awayTeamLogoUrl: string; // "Url" ekledik
  homeScore?: number;
  awayScore?: number;
  week?: number; // Eksik olan 'week' alanını buraya ekle
  homeTeamLogo: string;
  awayTeamLogo: string;
  stadiumName: string;
  location: string;
  events: MatchEvent[]; // HTML'deki *ngFor döngüsü için kritik
}

export interface MatchEvent {
  minute: number;
  playerName: string;
  type: 'goal' | 'yellow-card' | 'red-card' | 'substitution';
  teamName: string;
}
