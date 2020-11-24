import { Component } from "@angular/core";
import { MsalService } from "@azure/msal-angular";
import { HttpClient, HttpHeaders } from "@angular/common/http";

const API_ENDPOINT = "https://localhost:5001/weatherforecast/city/";
const API_SCOPE = "https://jfuerlingerb2cdemo.onmicrosoft.com/api/read_data";

@Component({
  selector: "app-root",
  templateUrl: "app.component.html",
  styles: [],
})
export class AppComponent {
  title = "JitBoxNg";
  city = "";
  temperature = "";
  humidity = "";
  summary = "";

  constructor(private authService: MsalService, private http: HttpClient) { }

  ngOnInit(): void { }

  async onApiCall() {
    console.log("Perform api call");

    if (!this.authService.getAccount()) {
      console.log("Perform login");
      try {
        let result = await this.authService.loginPopup();
        console.log("Login success", result);
      } catch (error) {
        console.log("Login failed : ", error);
      }
    }

    if (this.authService.getAccount()) {

      this.city = this.authService.getAccount().idTokenClaims.city;
      console.log(`city of current user: ${this.city}`);

      let tokenResult = await this.authService.acquireTokenSilent({ scopes: [API_SCOPE] });

      this.http
        .get(API_ENDPOINT + this.city, {
          responseType: "text",
          headers: new HttpHeaders({
            Authorization: "Bearer " + tokenResult.accessToken,
          }),
        })
        .subscribe((result) => {
          console.log(result);
          var res = JSON.parse(result);
          this.temperature = res.temperature;
          this.humidity = res.humidity;
          this.summary = res.summary;
        },
          (error) => console.log(error)
        );
    }
  }
}
