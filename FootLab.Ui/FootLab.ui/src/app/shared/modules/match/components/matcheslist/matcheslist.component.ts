import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatchService } from '../../services/match.service';
import { Match } from '../../models/match.model';

@Component({
  selector: 'app-matcheslist',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './matcheslist.component.html',
  styleUrl: './matcheslist.component.scss'
})
export class MatchesListComponent implements OnInit {
  private matchService = inject(MatchService);
  allMatches: Match[] = [];

  ngOnInit() {
    this.matchService.getAllMatches().subscribe(res => this.allMatches = res);
  }
}
