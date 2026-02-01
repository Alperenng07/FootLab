import { Component, inject, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatchService } from '../../services/match.service';
import { Match } from '../../models/match.model';

@Component({
  selector: 'app-match-detail',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './matchdetail.component.html',
  styleUrl: './matchdetail.component.scss',
})
export class MatchDetailComponent implements OnInit {
  @Input() id!: number;
  private matchService = inject(MatchService);
  matchData?: Match;

  // HATA VEREN DEĞİŞKENLERİ BURAYA EKLE:
  displayDate: string = '31 Ocak 2026'; // HTML'deki {{displayDate}} için
  matches: Match[] = [];                // HTML'deki *ngFor="let m of matches" için

  ngOnInit() {
    // Eğer 'id' gelmezse hata almamak için kontrol ekleyelim
    if (this.id) {
      this.matchService.getMatchById(this.id).subscribe(res => this.matchData = res);
    }
  }
}

