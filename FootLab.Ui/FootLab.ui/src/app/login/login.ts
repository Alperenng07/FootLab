import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class LoginComponent {
  // Görünüm modları: 'login', 'register', 'forgot'
  viewMode: string = 'login';

  loginData = {
    email: '',
    password: ''
  };

  onLogin() {
    console.log('Stark Operasyonu Başlatılıyor...', this.loginData);
    // Burada .NET Core Auth servis çağrısı yapılacak
  }
}
