import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-voting',
  templateUrl: './voting.component.html',
  styleUrls: ['./voting.component.scss']
})
export class VotingComponent implements OnInit {
  candidates: string[] = ['Candidate 1', 'Candidate 2', 'Candidate 3', 'Candidate 4', 'Candidate 5', 'Candidate 6', 'Candidate 7', 'Candidate 8', 'Candidate 9', 'Candidate 10'];
  selectedCandidate: string = '';

  constructor(private http: HttpClient, private snackBar: MatSnackBar) { }

  ngOnInit(): void {
    this.fetchCandidates();
  }

  fetchCandidates() {
    this.http.get<string[]>('https://localhost:44328/api/candidates').subscribe({
      next: (response) => {
        this.candidates = response;
      },
      error: (error) => {
        console.error('Error fetching candidates:', error);
      }
    });
  }

  vote() {
    const headers = new HttpHeaders().set('Content-Type', 'application/json').set('Authorization', 'token');
    const payload = "\"" + this.selectedCandidate + "\"";
    if (this.selectedCandidate) {
      this.http.post<any>('https://localhost:44328/api/votes', payload, { headers }).subscribe({
        next: (response) => {
          console.log('Vote submitted successfully:', response);
        },
        error: (error) => {
          console.error('Error submitting vote:', error);
          this.snackBar.open('Error submitting vote. Please try again.', 'Close', {
            duration: 5000
          });
        }
      });
    } else {
      console.error('No candidate selected');
      this.snackBar.open('No candidate selected. Please try again.', 'Close', {
        duration: 5000
      });
    }
  }
}