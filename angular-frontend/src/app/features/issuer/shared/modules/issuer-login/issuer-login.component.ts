import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-issuer-login',
  templateUrl: './issuer-login.component.html',
  styleUrls: ['./issuer-login.component.scss']
})
export class IssuerLoginComponent {
  public title: string;

  public constructor(
    private route: ActivatedRoute,
    private router: Router,
  ) {
    this.title = route.snapshot.data.title;
  }

  public onLogin(): void {

  }
}
