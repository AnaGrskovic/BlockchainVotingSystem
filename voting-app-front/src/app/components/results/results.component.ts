import { Component, Inject, OnInit, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-results',
  templateUrl: './results.component.html',
  styleUrls: ['./results.component.scss']
})
export class ResultsComponent implements OnInit {
  voteData: any;
  isLoading: boolean = false;

  constructor(private http: HttpClient, private snackBar: MatSnackBar, @Inject(PLATFORM_ID) private platformId: Object) {}

  ngOnInit(): void {
    if (isPlatformBrowser(this.platformId)) {
      this.fetchResults();
    }
  }

  fetchResults(): void {
    this.isLoading = true;
    const url = 'https://localhost:44328/api/results';
    this.http.get<any>(url).subscribe({
      next: (response) => {
        console.log('Results fetched successfully:', response);
        this.voteData = response;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error fetching results:', error);
        this.snackBar.open('Fetching results failed.', 'Close', { duration: 3000 });
        this.isLoading = false;
      }
    });
  }

}
