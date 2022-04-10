export interface Message{
    id : number;
    senderId : number;
    senderUsername : string;
    senderPhotoUrl : string;
    recipientId : number;
    recipientUsername : string;
    recipientPhotoUrl : string;
    content : string;
    readAt : Date;
    sentAt : Date;
}