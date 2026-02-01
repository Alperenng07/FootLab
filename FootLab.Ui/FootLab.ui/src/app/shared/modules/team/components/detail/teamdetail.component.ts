import { Component, inject, OnInit, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
// Model ve Servis yollarını projene göre kontrol et
// import { TeamService } from '../../services/team.service'; 

@Component({
  selector: 'app-team-detail',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './teamdetail.component.html',
  styleUrl: './teamdetail.component.scss'
})
export class TeamDetailComponent implements OnInit {
  @Input() id!: number; // Rota veya Parent'tan gelen takım ID'si

  // Taslak Veri: Görsel 1'deki Denizli Aslanspor verileriyle birebir uyumlu
  teamData: any = {
    name: 'Denizli Aslanspor',
    logo: 'assets/logos/denizli-aslan.png',
    city: 'Denizli',

    // Kulüp Bilgileri Paneli
    clubInfo: {
      foundedYear: 2008,
      president: 'Selim Karadağ',
      coach: 'Orhan Yıldız',
      colors: 'Kırmızı - Beyaz',
      stadium: 'Pamukkale Stadı, Denizli'
    },

    // Kadro Listesi (Mevkilere göre ayrılmış)
    squad: {
      goalkeepers: [
        { no: 1, name: 'Mehmet Aksoy', photo: 'assets/p1.jpg' },
        { no: 12, name: 'Ahmet Ateş', photo: 'assets/p2.jpg' }
      ],
      defenders: [
        { no: 3, name: 'Serkan Kaya', photo: 'assets/p3.jpg' },
        { no: 5, name: 'Ali Güler', photo: 'assets/p4.jpg' },
        { no: 15, name: 'Kadir Demir', photo: 'assets/p5.jpg' }
      ],
      midfielders: [
        { no: 8, name: 'Ahmet Demir', photo: 'assets/p6.jpg' },
        { no: 6, name: 'Hasan Ceylan', photo: 'assets/p7.jpg' },
        { no: 21, name: 'Faruk Yıldız', photo: 'assets/p8.jpg' },
        { no: 11, name: 'Halil Yıldırım', photo: 'assets/p9.jpg' }
      ]
    },

    // Maç Takvimi Paneli
    schedule: [
      { opponent: 'Horoz Gençlik FK', time: '18:00', date: '27.04.2024', stadium: 'Merkezefendi Sahası' },
      { opponent: 'Pamukkale Yıldızlar', time: '17:00', date: '27.05.2024', stadium: 'Akvadi Sahası' },
      { opponent: 'Kartal FK', time: '20:00', date: '18.05.2024', stadium: 'Doğan Demircioğlu Sahası' }
    ]
  };

  ngOnInit() {
    // API hazır olduğunda buradan çekebilirsin
    /*
    this.teamService.getTeamDetail(this.id).subscribe(res => {
      this.teamData = res;
    });
    */
  }
}
