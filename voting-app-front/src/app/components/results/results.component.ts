import { Component, Inject, OnInit, Input, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { MatSnackBar } from '@angular/material/snack-bar';
import { GraphModel } from "../../models/graph.model";

@Component({
  selector: 'app-results',
  templateUrl: './results.component.html',
  styleUrls: ['./results.component.scss']
})
export class ResultsComponent implements OnInit {
  voteData: any;
  graphData: Array<GraphModel> = [];
  isLoading: boolean = false;

  total = 0;
  maxHeight = 160;

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
        this.setGraphData(this.voteData.numberOfVotesPerCandidate);
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error fetching results:', error);
        this.snackBar.open('Fetching results failed.', 'Close', { duration: 3000 });
        this.isLoading = false;
      }
    });
  }

  setGraphData(voteData: any): void {
    this.graphData = [];
    this.total = 0;

    for (const candidate in voteData) {
      if (voteData.hasOwnProperty(candidate)) {
        this.graphData.push({
          Value: voteData[candidate],
          Color: this.getRandomColor(),
          Size: '',
          Legend: candidate
        });
        this.total += voteData[candidate];
      }
    }

    this.graphData.forEach(element => {
      element.Size = Math.round((element.Value * this.maxHeight) / this.total) + '%';
    });

    this.graphData.forEach(element => {
      this.total += element.Value;
    });
    this.graphData.forEach(element => {
      element.Size = Math.round((element.Value * this.maxHeight) / this.total) + '%';
    });
  }

  getRandomColor(): string {
    const letters = '0123456789ABCDEF';
    let color = '#';
    for (let i = 0; i < 6; i++) {
      color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
  }
}
