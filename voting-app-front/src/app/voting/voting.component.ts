import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

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
    const headers = new HttpHeaders().set('Content-Type', 'application/json');
    const payload = "\"" + this.selectedCandidate + "\"";
    if (this.selectedCandidate) {
      this.http.post<any>('https://localhost:44328/api/votes', payload, { headers }).subscribe({
        next: (response) => {
          console.log('Vote submitted successfully:', response);
          // You can add any additional logic here, such as displaying a confirmation message
        },
        error: (error) => {
          console.error('Error submitting vote:', error);
          // You can handle errors here, such as displaying an error message to the user
        }
      });
    } else {
      console.error('No candidate selected');
      // You can handle this case, such as displaying a message to the user to select a candidate before voting
    }
  }
}