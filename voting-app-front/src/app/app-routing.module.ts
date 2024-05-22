import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { VotingComponent } from './components/voting/voting.component';
import { ResultsComponent } from './components/results/results.component';
import { MainLayoutComponent } from './layouts/main-layout/main-layout.component';

const routes: Routes = [
	{
		path: '',
		component: MainLayoutComponent,
		children: [
			{ path: '', component: ResultsComponent, },
			{ path: 'login', component: LoginComponent },
			{ path: 'voting', component: VotingComponent },
		],
	},
	{
		path: '**',
		redirectTo: '',
	},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

