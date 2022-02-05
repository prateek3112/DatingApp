import { ToastrService } from 'ngx-toastr';
import { AccountService } from './../services/account.service';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AbstractControl, ControlContainer, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MembersService } from '../services/members.service';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
model:any = {}
registerForm : FormGroup;
maxDate: Date;
@Output() cancelRegister = new EventEmitter();
validationErrors : string[];
  constructor(private accountService : AccountService, private toastr : ToastrService,private fb: FormBuilder, private router : Router,private service : MembersService) { }

  ngOnInit(): void {
    this.initializeForm();
    this.maxDate = new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }


initializeForm(){
this.registerForm = this.fb.group({
  username : ['',Validators.required],
  gender : ['male'],
  name : ['',Validators.required],
  dateOfBirth : ['',Validators.required],
  city : ['',Validators.required],
  country : ['',Validators.required],
  password : ['',[Validators.required,Validators.minLength(4)]],
  confirmPassword : ['',[Validators.required, this.matchValues('password')]]
})
this.registerForm.controls.password.valueChanges.subscribe(()=>{
  this.registerForm.controls.confirmPassword.updateValueAndValidity();
})
}

matchValues(matchTo : string) : ValidatorFn
{
return (control : AbstractControl) => {
 return control?.value === control?.parent?.controls[matchTo].value ? null : {isMatching : true}
}
}

  async register(){
    console.log(this.registerForm.value);
    
    await (await this.accountService.register(this.registerForm.value)).subscribe(response =>{
      this.router.navigateByUrl('/members').then(()=>{
        this.service.getMembers();
      })
    },
    error=>{
      this.validationErrors = error;
    })
  }
cancel(){
  this.cancelRegister.emit(false);
}


}
