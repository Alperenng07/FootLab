import { Component, OnInit } from '@angular/core'; // OnInit ekledik
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { DailyMatchesListComponent } from '../shared/modules/match/components/dailymatcheslist/dailymatcheslist.component';
import { MatchesListComponent } from '../shared/modules/match/components/matcheslist/matcheslist.component';
import { MatchDetailComponent } from '../shared/modules/match/components/matchdetail/matchdetail.component';
import { PlayerListComponent } from '../shared/modules/player/components/list/playerlist.component';
import { PlayerDetailComponent } from '../shared/modules/player/components/detail/playerdetail.component';
import { PlayerSummaryComponent } from '../shared/modules/player/components/summary/playersummary.component';
import { TeamDetailComponent } from '../shared/modules/team/components/detail/teamdetail.component';
import { StandingListComponent } from '../shared/modules/standing/components/list/standinglist.component';

// 1. Tip tanımını export class'ın hemen üzerinde, bir kez yapıyoruz
type LeagueTab = 'puan' | 'fikstur' | 'gol' | 'kart';

@Component({
  selector: 'app-league',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    DailyMatchesListComponent,
    MatchesListComponent,
    MatchDetailComponent,
    PlayerListComponent,
    PlayerDetailComponent,
    PlayerSummaryComponent,
    TeamDetailComponent,
    StandingListComponent
  ],
  templateUrl: './league.html'
})
export class LeagueComponent implements OnInit {
  // 2. Değişkeni SADECE BİR KEZ tanımlıyoruz
  activeTab: LeagueTab = 'puan';

  // HTML'de döngü için dizi
  tabs: LeagueTab[] = ['puan', 'fikstur', 'gol'];

  leagueId: string | null = null;
  leagueInfo: any = {
    name: 'Denizli 1. Amatör Lig A Grubu',
    category: 'Denizli • Amatör Lig',
    logo: 'assets/league-logo.png',
    bgImage: 'assets/league-bg.jpg'
  };

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {
    // URL'den League ID'yi alma
    this.leagueId = this.route.snapshot.paramMap.get('id');
    console.log('Yüklenen Lig ID:', this.leagueId);
  }

  // Sekme değiştirme fonksiyonu
  setActiveTab(tabName: LeagueTab): void {
    this.activeTab = tabName;
  }
}
