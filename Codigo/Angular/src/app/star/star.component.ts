    
import { Component, OnChanges, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-star',
  templateUrl: './star.component.html',
  styleUrls: ['./star.component.css']
})
export class StarComponent implements OnChanges {
    
    @Input() rating: number;
    @Output() ratingClicked: EventEmitter<string> = new EventEmitter<string>();
    starWidth: number;

    ngOnChanges():void {
        console.log("onchanges!");
        console.log(this.rating);
        this.starWidth = this.rating * 86/5;
    }

    onClick(): void {
        this.ratingClicked.emit(`El puntaje ${this.rating} fue clickeado!`);
    }
}
