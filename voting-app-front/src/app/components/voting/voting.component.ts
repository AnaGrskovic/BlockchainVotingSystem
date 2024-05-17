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
    const url = 'https://localhost:44328/api/candidates';
    this.http.get<string[]>(url).subscribe({
      next: (response) => {
        this.candidates = response;
      },
      error: (error) => {
        console.error('Error fetching candidates:', error);
        this.snackBar.open('Error fetching candidates.', 'Close', {
          duration: 5000
        });
      }
    });
  }

  vote() {
    const url = 'https://localhost:44328/api/votes';
    const headers = new HttpHeaders().set('Content-Type', 'application/json').set('Authorization', 'token');
    const payload = "\"" + this.selectedCandidate + "\"";
    if (this.selectedCandidate) {
      this.http.post<any>(url, payload, { headers }).subscribe({
        next: (response) => {
          console.log('Vote submitted successfully:', response);
        },
        error: (error) => {
          console.error('Error submitting vote:', error);
          this.snackBar.open('Error submitting vote.', 'Close', {
            duration: 5000
          });
        }
      });
    } else {
      console.error('No candidate selected');
      this.snackBar.open('No candidate selected.', 'Close', {
        duration: 5000
      });
    }
  }
}