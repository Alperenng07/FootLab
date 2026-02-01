import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PlayerService } from '../../services/player.service';
import { Player } from '../../models/player.model';

@Component({
  selector: 'app-player-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './playerlist.component.html',
  styleUrl: './playerlist.component.scss'

})
export class PlayerListComponent implements OnInit {
  private playerService = inject(PlayerService);
  players: Player[] = [];

  ngOnInit() {
    this.playerService.getPlayers().subscribe(res => this.players = res);
  }
}
