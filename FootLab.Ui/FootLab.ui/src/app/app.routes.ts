import { Routes } from '@angular/router';
import { HomeComponent } from './home/home';
import { TeamComponent } from './team/team';
import { PlayerComponent } from './player/player';
import { LeagueComponent } from './league/league';
import { MatchComponent } from './match/match';
import { DataImportComponent } from './chief/data/data';
import { DataDetailComponent } from './chief/detail/detail';
import { LoginComponent } from './login/login';

export const routes: Routes = [
  { path: 'home', component: HomeComponent },
  { path: 'player', component: PlayerComponent },
  { path: 'team', component: TeamComponent },
  {path: 'league/:id',component: LeagueComponent, title: 'FootLab - Lig Detayı'},
  // Eğer parametresiz erişmek istersen:
  { path: 'league', component: LeagueComponent },
  { path: 'match', component: MatchComponent },
  { path: 'data-import', component: DataImportComponent },
  { path: 'data-detail', component: DataDetailComponent },
  { path: 'login', component: LoginComponent },

  { path: '', redirectTo: 'home', pathMatch: 'full' }
];
