import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {

  userId!: string;

  constructor (private readonly router: Router) {}  

  onSubmit() {
    localStorage.setItem("userId", this.userId);
    this.router.navigateByUrl("/otp")
  }

}
