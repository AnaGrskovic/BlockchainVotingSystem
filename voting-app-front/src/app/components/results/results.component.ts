import { Component, Inject, OnInit, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Chart } from 'chart.js';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-results',
  templateUrl: './results.component.html',
  styleUrls: ['./results.component.scss']
})
export class ResultsComponent implements OnInit {
  isLoading: boolean = false;
  public chart: any;

  constructor(private http: HttpClient, private snackBar: MatSnackBar, @Inject(PLATFORM_ID) private platformId: Object) {}

  ngOnInit(): void {
    if (isPlatformBrowser(this.platformId)) {
      this.fetchResults();
      this.createChart();
    }
  }

  fetchResults(): void {
    this.isLoading = true;
    const url = 'https://localhost:44328/api/results';
    this.http.get<any>(url).subscribe({
      next: (response) => {
        console.log('Results fetched successfully:', response);
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error fetching results:', error);
        this.snackBar.open('Fetching results failed.', 'Close', { duration: 3000 });
        this.isLoading = false;
      }
    });
  }

  createChart(): void {
    this.chart = new Chart("MyChart", {
      type: 'bar',
      data: {
        labels: ['2022-05-10', '2022-05-11', '2022-05-12','2022-05-13',
								 '2022-05-14', '2022-05-15', '2022-05-16','2022-05-17', ], 
	       datasets: [
          {
            label: "Sales",
            data: ['467','576', '572', '79', '92',
								 '574', '573', '576'],
            backgroundColor: 'blue'
          },
          {
            label: "Profit",
            data: ['542', '542', '536', '327', '17',
									 '0.00', '538', '541'],
            backgroundColor: 'limegreen'
          }  
        ]
      },
      options: {
        aspectRatio:2.5
      }
    });
  }

}
