import { Component, OnInit } from '@angular/core';
import { OtpService } from '../services/otp.service';
import { Observable, interval, startWith, switchMap, tap, timer } from 'rxjs';
import { OTP } from './models/otp';

@Component({
  selector: 'app-otp',
  templateUrl: './otp.component.html',
  styleUrls: ['./otp.component.css']
})
export class OtpComponent implements OnInit {
  otp$!: Observable<OTP>;
  timer$!: Observable<any>;
  secondsLeft!: number;
  userId!: string;

  constructor (private readonly otpService: OtpService) {}

  ngOnInit(): void {
    this.fetchOtp();
    this.startTimer();
  }

  fetchOtp() {
    this.userId = localStorage.getItem("userId") ?? "default";
    this.otp$ = this.otpService.getOtp(this.userId);
  }

  private startTimer(): void {
    this.timer$ = timer(0, 1000).pipe(
      tap(() => {
        const currentTime = new Date();
        const seconds = currentTime.getSeconds();
        const secondsPassedInCurrentInterval = seconds % 30;
        this.secondsLeft = 30 - secondsPassedInCurrentInterval;
  
        if (this.secondsLeft === 30 || this.secondsLeft === 0) {
          this.fetchOtp();
        }
      })
    );
  
    this.timer$.subscribe();
  }

}
