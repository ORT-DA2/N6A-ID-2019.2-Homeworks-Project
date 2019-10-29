import { Component, OnInit } from '@angular/core';
import { Homework } from '../models/Homework';
import { HomeworksService } from '../services/homeworks.service';

@Component({
  selector: 'app-homeworks-list',
  templateUrl: './homeworks-list.component.html',
  styleUrls: ['./homeworks-list.component.css']
})
export class HomeworksListComponent implements OnInit {
  pageTitle:string = 'HomeworksList';
  homeworks:Array<Homework>;
  showExercises:boolean = false;
  listFilter:string = "";

  constructor(private _serviceHomeworks:HomeworksService) { 
    
  }

  ngOnInit() {
    this._serviceHomeworks.getHomeworks().subscribe(
        ((data : Array<Homework>) => this.result(data)),
        ((error : any) => console.log(error))
    )
  }
  
  private result(data: Array<Homework>):void {
    this.homeworks = data;
    console.log(this.homeworks);
  }

  toogleExercises() {
    this.showExercises = !this.showExercises;
  }

  onRatingClicked(message:string):void {
    this.pageTitle = 'HomeworksList ' + message;
  }
}