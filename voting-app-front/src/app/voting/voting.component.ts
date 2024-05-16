import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-voting',
  templateUrl: './voting.component.html',
  styleUrls: ['./voting.component.scss']
})
export class VotingComponent implements OnInit {
  candidates: string[] = ['Candidate 1', 'Candidate 2', 'Candidate 3', 'Candidate 4', 'Candidate 5', 'Candidate 6', 'Candidate 7', 'Candidate 8', 'Candidate 9', 'Candidate 10'];
  selectedCandidate: string = '';

  constructor(private http: HttpClient) { }

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
    if (this.selectedCandidate) {
      // Perform action upon voting, like sending data to a server
      console.log('Voted for:', this.selectedCandidate);
    } else {
      console.log('Please select a candidate to vote');
    }
  }
}