import { Photo } from "./Photo";

export interface Member {
  id: number;
  userName: string;
  photoUrl: string;
  age: number;
  name: string;
  createdOn: Date;
  lastActive: Date;
  gender: string;
  intro: string;
  lookingFor: string;
  interests: string;
  city: string;
  country: string;
  photos: Photo[];
}
