import { Component, Inject, OnInit, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { LocalStorageService } from '../../services/local-storage/local-storage.service';

@Component({
  selector: 'app-voting',
  templateUrl: './voting.component.html',
  styleUrls: ['./voting.component.scss']
})
export class VotingComponent implements OnInit {
  token: string = '';
  candidates: string[] = [];
  selectedCandidate: string = '';
  isLoading: boolean = false;

  constructor(
    private http: HttpClient,
    private localStorageService: LocalStorageService,
    private snackBar: MatSnackBar,
    private router: Router,
    @Inject(PLATFORM_ID) private platformId: Object
  ) { }

  ngOnInit(): void {
    if (isPlatformBrowser(this.platformId)) {
      this.token = this.localStorageService.getItem('token') ?? '';
      this.fetchCandidates();
    }
  }

  fetchCandidates() {
    this.isLoading = true;
    const url = 'https://localhost:44328/api/candidates';
    this.http.get<string[]>(url).subscribe({
      next: (response) => {
        console.log('Candidates fetched successfully:', response);
        this.candidates = response;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error fetching candidates:', error);
        this.snackBar.open('Error fetching candidates.', 'Close', {
          duration: 5000
        });
        this.isLoading = false;
      }
    });
  }

  vote() {
    if (!this.selectedCandidate) {
      console.error('No candidate selected');
      this.snackBar.open('No candidate selected.', 'Close', {
        duration: 5000
      });
      return;
    }

    this.isLoading = true;
    const url = 'https://localhost:44328/api/votes';
    const headers = new HttpHeaders().set('Content-Type', 'application/json').set('Authorization', this.token);
    const payload = `"${this.selectedCandidate}"`;
    this.http.post<any>(url, payload, { headers }).subscribe({
      next: (response) => {
        console.log('Vote submitted successfully:', response);
        this.router.navigate(['/']);
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error submitting vote:', error);
        this.snackBar.open('Error submitting vote.', 'Close', {
          duration: 5000
        });
        this.isLoading = false;
      }
    });
  }
}
