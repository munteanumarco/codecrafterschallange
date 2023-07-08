import { Component } from '@angular/core';
import { OtpService } from 'src/app/services/otp.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-validation',
  templateUrl: './validation.component.html',
  styleUrls: ['./validation.component.css']
})
export class ValidationComponent {
  userId!: string;
  otp!: string;
  status$!: Observable<boolean>;
  
  constructor (private readonly otpService: OtpService) {}

  onValidate() {
    if (this.userId && this.otp) {
      this.status$ = this.otpService.verifyOtp(this.userId, this.otp);
    }
  }
}
