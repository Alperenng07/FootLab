import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatchService } from '../../services/match.service';
import { Match } from '../../models/match.model';

@Component({
  selector: 'app-dailymatcheslist',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dailymatcheslist.component.html',
  styleUrl: './dailymatcheslist.component.scss'
})
export class DailyMatchesListComponent implements OnInit {
  private matchService = inject(MatchService);
  matches: Match[] = [];
  currentDate: string = '29 Mayıs 2024';

  ngOnInit(): void {
    this.matchService.getDailyMatches().subscribe(data => {
      this.matches = data;
    });
  }
}
