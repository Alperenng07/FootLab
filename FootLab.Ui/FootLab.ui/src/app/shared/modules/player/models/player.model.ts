export interface Player {
  id: number;
  fullName: string;
  position: string;     // Mevki
  age: number;          // Yaş
  teamName: string;     // Takım
  teamLogoUrl: string;  // Takım Logosu
  photoUrl: string;     // Oyuncu Fotoğrafı
  overall: number;      // Genel Reyting (Örn: 85)
  marketValue?: string; // Piyasa Değeri
  currentTeam: string;
  currentTeamLogo: string;
  heightWeight: string;
  preferredFoot: string;
  totalMatches: number;
  totalGoals: number;
  totalAssists: number;
  career: careerHistory[];
  performance: PlayerStats;
}
export interface careerHistory {
  season: string;
  teamName: string;
  teamLogo: string;
  matches: number;
  goals: number;
}

export interface PlayerStats {
  lastFiveMatches: {
    opponentTeam: string;
    opponentLogo: string;
    score: string;
    goals: number;
    assists: number;
  }[];
}

export interface PlayerSummary {
  id: number;
  fullName: string;
  position: string;
  photoUrl: string;
  statCount: number;      // Gol veya Kurtarış sayısı
  statLabel: string;      // "Gol" veya "Maç Kurtarışı" yazısı
  borderColorClass: string; // Yeşil, Kırmızı veya Mavi alt çizgi için
}
