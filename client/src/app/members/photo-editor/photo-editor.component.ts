import { MembersService } from './../../services/members.service';
import { Photo } from './../../models/Photo';
import { AccountService } from './../../services/account.service';
import { environment } from 'src/environments/environment';

import { Member } from './../../models/member';
import { Component, OnInit, Input } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { User } from 'src/app/models/user';
import { take } from 'rxjs/operators';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
@Input() member : Member;
uploader : FileUploader;
hasBaseDropZoneOver = false;
baseUrl = environment.apiUrl;
user : User;

  constructor(private service : AccountService,private spinner : NgxSpinnerService, private memberService : MembersService) { 
    this.service.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
  }

  ngOnInit(): void {
    this.InitializeUploader();
  }

  InitializeUploader(){
this.uploader = new FileUploader({
  url : this.baseUrl + "users/add-photo",
  authToken : "Bearer " + this.user.token,
  isHTML5 : true,
  allowedFileType : ['image'],
  removeAfterUpload : true,
  autoUpload : false,
  maxFileSize : 10 * 1024 * 1024

});

this.uploader.onAfterAddingFile = (file) =>{
  file.withCredentials = false;
}
this.uploader.onProgressItem = ()=>{
  this.spinner.show();
}
this.uploader.onSuccessItem = (item,response,status,headers) =>{
  this.spinner.hide();
  if(response){
    const photo = JSON.parse(response);
    this.member.photos.push(photo);
  }
  
}

  }

  fileOverBase(e : any){
this.hasBaseDropZoneOver = e;
  }

  setMain(photo : Photo){
    this.memberService.setMainPhoto(photo.id).subscribe(()=>{
this.user.photoUrl = photo.url;
this.service.setCurrentUser(this.user);
this.member.photoUrl = photo.url;
this.member.photos.forEach(p=>{
  if(p.isMain) p.isMain = false;
  if(p.id === photo.id) p.isMain = true;
})
    })
  }

}
