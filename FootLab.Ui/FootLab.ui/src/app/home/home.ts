import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { DailyMatchesListComponent } from '../shared/modules/match/components/dailymatcheslist/dailymatcheslist.component';
import { MatchesListComponent } from '../shared/modules/match/components/matcheslist/matcheslist.component';
import { MatchDetailComponent } from '../shared/modules/match/components/matchdetail/matchdetail.component';
import { PlayerListComponent } from '../shared/modules/player/components/list/playerlist.component';
import { PlayerDetailComponent } from '../shared/modules/player/components/detail/playerdetail.component';
import { PlayerSummaryComponent } from '../shared/modules/player/components/summary/playersummary.component';
import { TeamDetailComponent } from '../shared/modules/team/components/detail/teamdetail.component';
import { StandingListComponent } from '../shared/modules/standing/components/list/standinglist.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule,
    RouterLink,
    DailyMatchesListComponent,
    PlayerSummaryComponent,
  ],
  templateUrl: './home.html'
})
export class HomeComponent {
  // Görseldeki "Günün Maçları" için küçük bir özet data
  quickMatches = [
    { time: '18:00', home: 'Denizli Aslanspor', away: 'Horoz Gençlik FK' },
    { time: '20:00', home: 'Pamukkale Yıldızlar', away: 'Merkezgücü SK' }
  ];
}
