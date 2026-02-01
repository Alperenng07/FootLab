import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PlayerService } from '../../services/player.service';
import { PlayerSummary } from '../../models/player.model';

@Component({
  selector: 'app-player-summary',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './playersummary.component.html',
  styleUrl: './playersummary.component.scss'
})
export class PlayerSummaryComponent implements OnInit {
  private playerService = inject(PlayerService);

  // Taslak Veri: API hazır olana kadar görseldeki gibi görünsün
  featuredPlayers: PlayerSummary[] = [
    { id: 1, fullName: 'Emre Yılmaz', position: 'Forvet', statCount: 8, statLabel: 'Gol', photoUrl: 'assets/emre.jpg', borderColorClass: 'border-green-500' },
    { id: 2, fullName: 'Ahmet Demir', position: 'Orta Saha', statCount: 6, statLabel: 'Gol', photoUrl: 'assets/ahmet.jpg', borderColorClass: 'border-red-500' },
    { id: 3, fullName: 'Serkan Kaya', position: 'Defans', statCount: 4, statLabel: 'Gol', photoUrl: 'assets/serkan.jpg', borderColorClass: 'border-green-800' },
    { id: 4, fullName: 'Mehmet Aksoy', position: 'Kaleci', statCount: 3, statLabel: 'Maç Kurtarışı', photoUrl: 'assets/mehmet.jpg', borderColorClass: 'border-blue-500' }
  ];

  ngOnInit() {
    // API bağlantısı hazır olduğunda burayı açabilirsin
    // this.playerService.getFeaturedPlayers().subscribe(res => this.featuredPlayers = res);
  }
}
