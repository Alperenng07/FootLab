import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { MatchDetailComponent } from '../shared/modules/match/components/matchdetail/matchdetail.component';


@Component({
  selector: 'app-match',
  standalone: true,
  imports: [CommonModule,MatchDetailComponent],
  templateUrl: './match.html'
})
export class MatchComponent implements OnInit {

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {

  }


}
