import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';


// Kabul edilebilir sekme tiplerini tanımla
type TeamTab = 'kadro' | 'bilgi' | 'takvim';

@Component({
  selector: 'app-team',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './team.html'
})
export class TeamComponent implements OnInit {
  activeTab: TeamTab = 'kadro';

  // HTML'de döngü kuracağımız listeyi bu tiple işaretle
  tabs: TeamTab[] = ['kadro', 'bilgi', 'takvim'];

  setActiveTab(tabName: TeamTab): void {
    this.activeTab = tabName;
  }

  teamInfo: any = {
    name: 'Denizli Aslanspor',
    logo: 'assets/aslanspor-logo.png',
    founded: 2008,
    president: 'Selim Karadağ',
    coach: 'Orhan Yıldız',
    colors: ['Kırmızı', 'Beyaz'],
    stadium: 'Pamukkale Stadı, Denizli',
    // Kadro verileri
    squad: [
      { id: 1, name: 'Mehmet Aksoy', no: 1, pos: 'Kaleci', img: 'assets/p1.jpg' },
      { id: 2, name: 'Serkan Kaya', no: 3, pos: 'Defans', img: 'assets/p2.jpg' },
      { id: 3, name: 'Ahmet Demir', no: 8, pos: 'Orta Saha', img: 'assets/p3.jpg' }
    ],
    // Maç takvimi
    fixtures: [
      { date: '27.4.2024', opponent: 'Horoz Gençlik FK', time: '18:00', pitch: 'Merkezefendi Sahası' }
    ]
  };

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    console.log('Takım ID:', id);
  }


}
