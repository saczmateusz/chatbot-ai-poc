import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ChatListItem } from '../dtos/chat-list-item.model';

@Injectable({
  providedIn: 'root',
})
export class ChatListService {
  constructor(private http: HttpClient) {}

  getAllChats(): Observable<ChatListItem[]> {
    return this.http.get<ChatListItem[]>(`https://localhost:7218/Chat/GetAll`);
  }
}
