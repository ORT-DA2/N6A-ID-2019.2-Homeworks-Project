import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router } from '@angular/router';

@Injectable()
export class HomeworkDetailGuard implements CanActivate {

  constructor(private _router: Router) { }

  canActivate(route: ActivatedRouteSnapshot): boolean {
    let id = route.url[1].path;
    if (!isNaN(+id)) {
        alert('La id de la tarea no es valida');
        // redirigimos (a traves de una navegacion), a /homeworks
        this._router.navigate(['/homeworks']);
        // abortamos la navegacion actual
        return false;
    };
    return true;
  }
}