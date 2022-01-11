import { Member } from './../../models/member';
import { MembersService } from './../../services/members.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  member: Member;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];

  constructor(private memberService : MembersService, private route : ActivatedRoute) { }

  ngOnInit(): void {
    this.loadMember();
    this.galleryOptions = [
      {
        width: '500px',
        height: '500px',
        thumbnailsColumns: 4,
        imagePercent : 100,
        imageAnimation: NgxGalleryAnimation.Fade,
        preview:false
      }
      
    ];

    
  }

  getImages() : NgxGalleryImage[]{

const imageUrls = [];

for(const photo of this.member?.photos){
  console.log(this.member);
  imageUrls.push({
    small : photo?.url,
    medium : photo?.url,
    big : photo?.url,

  });

  
}
return imageUrls;

  }

  loadMember(){
    
    this.memberService.getMember(this.route.snapshot.paramMap.get('username')).subscribe((member)=>{
      this.member = member;
      console.log(member);
      this.galleryImages = this.getImages();
    })
  }

}
