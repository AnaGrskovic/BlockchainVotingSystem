import { Component } from '@angular/core';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  personalIdNum: string = '';

  onLogin() {
    if (this.personalIdNum) {
      console.log('Login successful with ID:', this.personalIdNum);
    } else {
      console.error('Personal identification number is required');
    }
  }
}
