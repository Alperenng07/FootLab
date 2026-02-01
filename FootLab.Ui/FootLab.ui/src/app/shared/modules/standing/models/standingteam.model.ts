export interface StandingTeam {
  rank: number;
  teamId: number;
  teamName: string;
  teamLogo: string;
  played: number;    // O (Oynanan)
  won: number;       // G (Galibiyet)
  draw: number;      // B (Beraberlik)
  lost: number;      // M (Mağlubiyet)
  goalsFor: number;  // A (Atılan)
  goalsAgainst: number; // Y (Yenilen)
  average: number;   // AV (Averaj)
  points: number;    // P (Puan)
  status?: 'champion' | 'relegation' | 'none'; // S ve D harfleri için
}
