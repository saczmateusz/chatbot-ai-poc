import { Component, OnInit } from '@angular/core';
import { ChatListService } from '../services/chat-list.service';
import { CommonModule } from '@angular/common';
import { RouterModule, Router, NavigationEnd } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { filter } from 'rxjs/operators';
import { ChatListItem } from '../dtos/chat-list-item.model';

@Component({
  selector: 'app-side-menu',
  imports: [CommonModule, MatButtonModule, RouterModule],
  templateUrl: './side-menu.component.html',
  styleUrl: './side-menu.component.scss',
})
export class SideMenuComponent implements OnInit {
  chats: ChatListItem[] = [];

  constructor(
    private chatListService: ChatListService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.fetchChats();
    this.router.events
      .pipe(filter((event) => event instanceof NavigationEnd))
      .subscribe((event: NavigationEnd) => {
        const url = event.urlAfterRedirects || event.url;
        // Re-fetch chat list if navigating to a new chat or root
        if (/^\/chat\/.+\?new=true$/.test(url) || url === '/') {
          this.fetchChats();
        }
      });
  }

  private fetchChats(): void {
    this.chatListService.getAllChats().subscribe({
      next: (chats) => {
        this.chats = chats;
      },
      error: (err) => {
        console.error('Failed to fetch chats', err);
      },
    });
  }
}
