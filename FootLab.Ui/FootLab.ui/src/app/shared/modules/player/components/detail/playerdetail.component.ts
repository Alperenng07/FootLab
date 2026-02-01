import { Component, inject, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PlayerService } from '../../services/player.service';
import { Player } from '../../models/player.model';

@Component({
  selector: 'app-player-detail',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './playerdetail.component.html',
  styleUrl: './playerdetail.component.scss'
})
export class PlayerDetailComponent implements OnInit {
  @Input() id!: number;
  // Dışarıdan gelecek veriyi kabul eden kapı burası
  @Input() playerData: any;

  private playerService = inject(PlayerService);

  ngOnInit() {
    this.playerService.getPlayerById(this.id).subscribe({
      next: (res) => {
        console.log("Gelen Oyuncu Verisi:", res);
        this.playerData = res;
      },
      error: (err) => console.error("Oyuncu verisi çekilemedi:", err)
    });
  }
}
