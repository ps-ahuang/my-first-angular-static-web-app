import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  template: `<a href="/.auth/login/aad">Alan Login</a><div>{{message}}</div><a href="/.auth/logout/aad">Alan Logout</a>`,
})
export class AppComponent {
  message = '';

  constructor(private http: HttpClient) {
    this.http.get('/api/message')
      .subscribe((resp: any) => this.message = resp.text);
  }
}