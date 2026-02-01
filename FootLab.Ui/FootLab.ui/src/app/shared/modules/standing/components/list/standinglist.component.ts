import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StandingTeam } from '../../models/standingteam.model';

@Component({
  selector: 'app-standing-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './standinglist.component.html',
  styleUrl: './standinglist.component.scss'
})
export class StandingListComponent implements OnInit {
  // Taslak Veri: Görselle birebir uyumlu
  standings: StandingTeam[] = [
    { rank: 1, teamId: 101, teamName: 'Denizli Aslanspor', teamLogo: 'assets/aslanspor.png', played: 22, won: 19, draw: 3, lost: 0, goalsFor: 78, goalsAgainst: 22, average: 56, points: 60, status: 'champion' },
    { rank: 2, teamId: 102, teamName: 'Çamlık Spor', teamLogo: 'assets/camlik.png', played: 22, won: 19, draw: 6, lost: 3, goalsFor: 78, goalsAgainst: 17, average: 61, points: 57, status: 'relegation' },
    { rank: 3, teamId: 103, teamName: 'Pamukkale Yıldızlar', teamLogo: 'assets/pamukkale.png', played: 22, won: 17, draw: 3, lost: 1, goalsFor: 78, goalsAgainst: 15, average: 63, points: 48, status: 'relegation' },
    { rank: 4, teamId: 104, teamName: 'Kartal FK', teamLogo: 'assets/kartal.png', played: 22, won: 16, draw: 2, lost: 8, goalsFor: 75, goalsAgainst: 12, average: 63, points: 40 }
  ];

  ngOnInit() { }
}
