# OTP Generator

## Quick Demo
[![Quick Demo](https://img.youtube.com/vi/BfJjJYZ_BSU/0.jpg)](https://www.youtube.com/watch?v=BfJjJYZ_BSU)

## Link to the app

I deployed the frontend on Netlify and the API on Azure so you can easily access it. I'll leave the link to the app bellow so you can play with it :)<br>
It may take some time to load as I used free tiers across the deployments.<br>
I added a small angular app just for the purpose of the demo cause it's easier to interract with it this way.<br>
This is the angular web app: https://codecrafterschallange.netlify.app<br>
Link to the exposed endpoints (swagger): https://webapp-230707184837.azurewebsites.net/swagger/index.html


## Background Information
I want to give you some background about my thought process and choices.

When I saw the challange the TOTP algorithm (Time-based One-time Password) came to my mind.
I made a more indepth research about it and basically it's an algorithm that computes a one-time password from a shared secret key and the current time. I heard that it has been commonly adopted for two-factor authentication. So this seemed like a good match for the requirement having in mind that most likely this could be a valid use case for this project: A user wants to log in and after they provided valid credentials a second layer of security is introduced, a temporary password that is valid for a short period of time. So the goal is to design this generator of OTPs.

Also here are two paths: a stateless aproach or a stateful one. Of course each one has its advantages and disadvantages and I read a little more about each one before making a decision for my implementaion. I leave bellow some information about this.

Stateless TOTP: <br>
    Advantages:<br>
        + scalability:  no information is stored, no excessive I/O on the DB<br>
        + simplicity: no need to keep track of any state or manage a DB of past OTPs.<br>
        + performance: validation also  can be done quickly, doesn't require any DB operations<br>
    Disadvantages:<br>
        - replay attacks: potentially vulnerable to replay attacks within the same time window as   the server has no memory of previously validated OTPs.<br>
        - time sync: if there's a significant time difference between the server and client, it could cause valid OTPs to be rejected or expired OTPs to be accepted. This can be mitigated by allowing a certain amount of time skew, but it's still a potential issue.<br>

Stateful TOTP:<br>
    Advantages:<br>
        + replay protection: can prevent replay attacks because the server keeps track of      previously used OTPs and can reject any that are used again.<br>
        + more control: it provides more control over OTP usage, for example, by allowing you to enforce stricter rules about OTP reuse or expiry.<br>
    Disadvantages:<br>
        - complexity: complex to implement and maintain, as you need to reliably store and manage state for each OTP.<br>
        - scalability & performance: can require significant storage and database resources, especially for a large number of users.<br>

After getting a little familiar with these concepts I thought that the approach I would take for this project will be the stateless one, as for this small project the reduced complexity and simplicity are key factors.

## Implementation

In our program that shared secret will be obtained from the hasing of the userID. This ensures that each user has a unique shared secret. The hashing operation is done using a secure hash algorithm like SHA256.(I know that using a user's ID as a base for the shared secret in a real-world application may not be the best idea, especially for sensitive applications, as it can create potential vulnerabilities if an attacker can guess or know the user IDs. But for this tiny project I think this will do it =)).

Then the OTP will be generatated using this shared secret and a time step. Now the time step:
Let's say ValidityPeriod is 30 seconds. So, from Unix timestamp 0 to 29 (the first 30 seconds after Unix epoch which is 1 January 1970), the value of timeStep would be 0. From timestamp 30 to 59 (the next 30 seconds), the value of timeStep would be 1. From 60 to 89, the value of timeStep would be 2, and so on.
So, by using this timeStep value as part of the OTP generation, I ensure that the OTP changes every 30 seconds. The OTP generated in the first 30 seconds after Unix epoch would be different than the OTP generated in the next 30 seconds, and so on.

Next we use these two, the shared secret and the time step and generate the OTP which is then returned.

I also added a little angular app to be able to demo the use case of the project. I had in mind the Microsoft Authenticator app, when you open it you see there all the codes for your different apps with a timer going down for each OTP. So in the angular app on the Home page you can put the userId and then you ll start seeing codes for this user. Also added a validation page where you can check this codes in real time.


## Testing

Regarding the testing part, I added some unit tests for both the service and the controller.<br>
For this I used the NUnit unit testing framework and Moq for the mocking part.


    
