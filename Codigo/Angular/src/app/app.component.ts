import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
    title:string = 'Homeworks Angular';
    name:string = "Santiago Mnedez";
    email : string = "santi17mendez@hotmail.com";
    address = {
        street: "la dirección del profe",
        city: "Montevideo",
        number: "1234"
    }
}