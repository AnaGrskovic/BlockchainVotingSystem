import { Component } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { LocalStorageService } from '../../services/local-storage/local-storage.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  personalIdNum: string = '';

  constructor(private http: HttpClient, private localStorageService: LocalStorageService, private snackBar: MatSnackBar, private router: Router) {}

  onLogin() {
    if (this.personalIdNum) {
      const url = 'https://localhost:44378/api/authorization/get-token';
      const headers = new HttpHeaders().set('Content-Type', 'application/json');
      const payload = "\"" + this.personalIdNum + "\"";

      this.http.post<number>(url, payload, { headers }).subscribe({
        next: (response) => {
          console.log('Login successful, token received:', response);
          this.localStorageService.setItem('token', response.toString());
          this.router.navigate(['/voting']);
        },
        error: (error) => {
          console.error('Error during login:', error);
          this.snackBar.open('Login failed.', 'Close', { duration: 3000 });
        }
      });
    } else {
      console.error('Personal identification number is required');
      this.snackBar.open('Personal identification number is required', 'Close', { duration: 3000 });
    }
  }
}
