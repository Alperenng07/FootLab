import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
// Reusable bileşenlerini eklemeyi unutma
import { PlayerSummaryComponent } from '../shared/modules/player/components/summary/playersummary.component';
import { PlayerDetailComponent } from '../shared/modules/player/components/detail/playerdetail.component';
import { DailyMatchesListComponent } from '../shared/modules/match/components/dailymatcheslist/dailymatcheslist.component';

@Component({
  selector: 'app-player',
  standalone: true,
  imports: [CommonModule, PlayerSummaryComponent, PlayerDetailComponent,
    DailyMatchesListComponent],
  templateUrl: './player.html'
})
export class PlayerComponent implements OnInit {



  // Seçili olan ana oyuncu verisi
  playerInfo: any = {
    fullName: 'Emre Yılmaz',
    position: 'Forvet',
    age: 25,
    weight: '74kg',
    height: '1.78m',
    foot: 'Sağ',
    teamName: 'Denizli Aslanspor',
    teamLogo: 'assets/aslanspor-logo.png',
    imageUrl: 'assets/player1.jpg',
    careerStats: [
      { season: '2023/24', team: 'Denizli Aslanspor', match: 20, goal: 14 },
      { season: '2022/23', team: 'Denizli Yıldızspor', match: 18, goal: 7 }
    ],
    lastMatches: [
      { opponent: 'Merkezefendi SK', score: '4-1', goal: 2, rating: 8.5 },
      { opponent: 'Kartal FK', score: '3-1', goal: 2, rating: 8.0 }
    ]
  };

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {
    // Dinamik geçişler için ID takibi
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      console.log('Oyuncu Detayları Yükleniyor:', id);
      // Burada .NET servisinle gerçek veriyi çekebilirsin
    }
  }
}
