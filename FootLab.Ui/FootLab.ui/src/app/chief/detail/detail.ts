import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-data-detail',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './detail.html'
})
export class DataDetailComponent {
  @Input() selectedPlayer: any;
  @Output() close = new EventEmitter<void>();
  @Output() onUpdate = new EventEmitter<any>();

  // Oyuncunun mevcut maç verileri (Başlangıçta sıfır veya mevcut veri gelir)
  playerStats: any = {
    goal: 0,
    assist: 0,
    yellow: 0,
    red: 0,
    in: 0,
    out: 0
  };

  actions = [
    { id: 'goal', label: 'Gol', icon: '⚽' },
    { id: 'assist', label: 'Asist', icon: '👟' },
    { id: 'yellow', label: 'Sarı Kart', icon: '🟨' },
    { id: 'red', label: 'Kırmızı Kart', icon: '🟥' }
  ];

  getActionCount(id: string): number {
    return this.playerStats[id] || 0;
  }

  updateAction(id: string, delta: number) {
    const newValue = (this.playerStats[id] || 0) + delta;

    // Değerin 0'ın altına düşmesini engelle (veya kartlar için 1 sınırı koyulabilir)
    if (newValue >= 0) {
      this.playerStats[id] = newValue;

      // Kırmızı kart mantığı: 1'den fazla olamaz
      if ((id === 'red' || id === 'yellow') && newValue > 2) {
        this.playerStats[id] = 2;
      }
    }
  }

  saveAndClose() {
    this.onUpdate.emit({
      playerId: this.selectedPlayer.id,
      stats: this.playerStats
    });
    this.close.emit();
  }

  closeModal() {
    this.close.emit();
  }
}
