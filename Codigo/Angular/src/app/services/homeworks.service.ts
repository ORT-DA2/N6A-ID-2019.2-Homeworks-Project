import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable, throwError } from "rxjs"; 
import { map, tap, catchError } from 'rxjs/operators';
import { Homework } from '../models/Homework';

@Injectable()
export class HomeworksService {

  private WEB_API_URL : string = 'http://localhost:5000/api/homeworks';

  constructor(private _httpService: HttpClient) {  }
  
  getHomeworks():Observable<Array<Homework>> {
    const myHeaders = new HttpHeaders();
    myHeaders.append('Accept', 'application/json');    
    const httpOptions = {
        headers: myHeaders
    };
          
    return this._httpService.get<Array<Homework>>(this.WEB_API_URL, httpOptions)
        .pipe(
            //map((response : Response) => <Array<Homework>> response.json()),
            tap(data => console.log('Los datos que obtuvimos fueron: ' + JSON.stringify(data))),
            catchError(this.handleError)
        );
  }

  private handleError(error: any) {
    console.error(error);
    return throwError(error.error || error.message);
  }
}