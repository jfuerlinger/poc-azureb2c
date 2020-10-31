
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './navbar/navbar.component';
import { HttpClientModule } from '@angular/common/http';
import { MsalModule } from '@azure/msal-angular'

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    MsalModule.forRoot({
      auth: {
        clientId: "4cd7f364-20eb-44d1-925f-9828dfa32933", // Application Id of Application registered in B2C
        authority: "https://jfuerlingerb2cdemo.b2clogin.com/jfuerlingerb2cdemo.onmicrosoft.com/B2C_1_SignUpAndIn", //signup-signin userflow
        validateAuthority: false,
        redirectUri: "http://localhost:4200/"
      },
      cache: {
        cacheLocation: "sessionStorage",
        storeAuthStateInCookie: false
      }
    }, {
      consentScopes: [
        "user.read", "openid", "profile"
      ]
    })
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

