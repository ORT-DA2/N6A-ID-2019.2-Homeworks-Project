import { Component, OnInit } from '@angular/core';
import { Homework } from '../models/Homework';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-homework-detail',
  templateUrl: './homework-detail.component.html',
  styleUrls: ['./homework-detail.component.css']
})
export class HomeworkDetailComponent implements OnInit {
  pageTitle : string = 'Homework Detail';
  homework : Homework;

  constructor(private _currentRoute: ActivatedRoute, private _router : Router) {  }

  ngOnInit() : void {
    // let (es parte de ES2015) y define una variable que vive en este scope
    // usamos el nombre del parámetro que uamos en la configuración de la ruta y lo obtenemos
    let id = this._currentRoute.snapshot.params['id'];
    // definimos el string con interpolacion 
    this.pageTitle +=  `: ${id}`;
  }

  onBack(): void {
    this._router.navigate(['/homeworks']);
  }
}
