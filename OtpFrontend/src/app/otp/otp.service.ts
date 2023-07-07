import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class OtpService {

  baseUrl: string = "/api/otp";

  constructor(private readonly http: HttpClient) { }

  testUp() {
    return this.http.get(`${this.baseUrl}/up`);
  }
  
  getOtp(userId: string): Observable<string> {
    let params = new HttpParams()
      .set('userId', userId);

    return this.http.get<string>(this.baseUrl, { params });
  }

  verifyOtp(userId: string, otp: string): Observable<boolean> {
    const requestBody = {
      userId: userId,
      otp: otp
    };

    return this.http.post<boolean>(this.baseUrl, requestBody).pipe(
      catchError(error => {
        if (error.status === 401) {
          // If the status code is 401 Unauthorized, return an observable of `false`
          return of(false);
        } else {
          // If the status code is not 401, re-throw the error
          throw error;
        }
      })
    );;
  }
}
