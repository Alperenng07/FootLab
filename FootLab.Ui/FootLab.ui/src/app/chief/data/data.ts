import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {
  CdkDragDrop,
  DragDropModule,
  moveItemInArray,
  transferArrayItem
} from '@angular/cdk/drag-drop';
import { DataDetailComponent } from '../detail/detail';

@Component({
  selector: 'app-data-import',
  standalone: true,
  imports: [DragDropModule, CommonModule, FormsModule, DataDetailComponent],
  templateUrl: './data.html'
})
export class DataImportComponent {
  showPopup = false;
  activePlayer: any = null;

  matchEntry = {
    homeTeam: 'Denizli Aslanspor',
    awayTeam: 'Horoz Gençlik FK',
    homeScore: 0,
    awayScore: 0,
    homePlayers: [
      { id: 1, name: 'Mehmet', no: 1, stats: { goal: 0, assist: 0, yellow: 0, red: 0 } },
      { id: 4, name: 'Emre Yılmaz', no: 4, stats: { goal: 0, assist: 0, yellow: 0, red: 0 } },
      { id: 5, name: 'Hakan Kaya', no: 5, stats: { goal: 0, assist: 0, yellow: 0, red: 0 } }
    ],
    awayPlayers: [
      { id: 21, name: 'Volkan Aras', no: 21, stats: { goal: 0, assist: 0, yellow: 0, red: 0 } },
      { id: 22, name: 'Mustafa Can', no: 22, stats: { goal: 0, assist: 0, yellow: 0, red: 0 } }
    ]
  };

  homeSubstitutes = [{ id: 12, name: 'Ahmet Ateş', no: 12, stats: { goal: 0 } }];
  awaySubstitutes = [{ id: 30, name: 'Caner Şen', no: 30, stats: { goal: 0 } }];

  drop(event: CdkDragDrop<any[]>) {
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      transferArrayItem(
        event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex
      );
      console.log('Stark Operasyonu: Oyuncu değişikliği yapıldı.');
    }
  }

  // Popup'ı açarken oyuncuyu referans alıyoruz
  openDetail(player: any) {
    this.activePlayer = player;
    this.showPopup = true;
  }

  // POPUP'TAN GELEN VERİYİ İŞLEYEN YENİ FONKSİYON
  handleActionUpdate(event: any) {
    if (this.activePlayer) {
      // Oyuncunun istatistiklerini güncelle
      this.activePlayer.stats = { ...event.stats };

      // Eğer gol atıldıysa ana skoru otomatik güncelle!
      this.calculateLiveScore();

      console.log(`${this.activePlayer.name} verileri güncellendi:`, event.stats);
    }
  }

  // Canlı skor hesaplama mantığı
  calculateLiveScore() {
    this.matchEntry.homeScore = this.matchEntry.homePlayers.reduce((total, p) => total + (p.stats?.goal || 0), 0);
    this.matchEntry.awayScore = this.matchEntry.awayPlayers.reduce((total, p) => total + (p.stats?.goal || 0), 0);
  }

  saveAllData() {
    console.log('FootLab Veritabanına Gönderiliyor:', this.matchEntry);
  }
}
