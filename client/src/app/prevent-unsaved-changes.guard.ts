import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { Injectable } from '@angular/core';
import { CanDeactivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PreventUnsavedChangesGuard implements CanDeactivate<unknown> {
  canDeactivate(
    component: MemberEditComponent): boolean{
      if(component.editForm.dirty){
        return confirm("Are you sure ? You will lose the changes you made! XO");
      }
    return true;
  }
  
}
