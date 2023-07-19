import { Component } from '@angular/core';
import { User } from './models/user';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'angular-frontend';
  user = new User();

  constructor(private authService: AuthService) { }

  register(user: User) {
    this.authService.register(user).subscribe();
  }

  login(user: User) {
    this.authService.login(user).subscribe((token: string) => {
      localStorage.setItem('authToken', token);
    });
  }

  getMe() {
    this.authService.getMe().subscribe((name: string) => {
      console.log(name);
    });
  }
}
