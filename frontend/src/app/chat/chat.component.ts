import { CommonModule, Location } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit, OnDestroy } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { ActivatedRoute, Router, NavigationStart } from '@angular/router';
import { Subject, Subscription } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { ChatService } from '../services/chat.service';
import { ChatMessage } from '../dtos/chat-message.model';
import { ReactionType } from '../types/reaction-type';

@Component({
  selector: 'app-chat',
  imports: [
    CommonModule,
    FormsModule,
    MatButtonModule,
    MatIconModule,
    ReactiveFormsModule,
  ],
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.scss',
})
export class ChatComponent implements OnInit, OnDestroy {
  readonly ReactionType = ReactionType;
  chatId: string | null = null;
  chatForm: FormGroup;
  messages: ChatMessage[] = [];
  isTyping = false;
  shouldSkipFetchingMessages = false;
  private destroy$ = new Subject<void>();
  private messageSubscription: Subscription | null = null;

  constructor(
    private fb: FormBuilder,
    private chatService: ChatService,
    private activatedRoute: ActivatedRoute,
    private location: Location,
    private router: Router,
    private http: HttpClient
  ) {
    this.chatForm = this.fb.group({
      message: ['', [Validators.required, Validators.minLength(1)]],
    });
  }

  ngOnInit(): void {
    this.activatedRoute.paramMap
      .pipe(takeUntil(this.destroy$))
      .subscribe((params) => {
        this.chatId = params.get('id');
        const isNew = this.activatedRoute.snapshot.queryParamMap.get('new');
        // Clear query params
        if (isNew) this.location.replaceState(`/chat/${this.chatId}`);
        // If it's a new chat, skip fetching messages
        if (this.chatId && !isNew) {
          this.http
            .get<ChatMessage[]>(
              `https://localhost:7218/Message/All?chatId=${this.chatId}`
            )
            .pipe(takeUntil(this.destroy$))
            .subscribe({
              next: (messages) => {
                this.messages = messages;
                this.chatService.setMessages(messages);
                this.scrollToBottom();
              },
              error: (err) => {
                console.error('Failed to fetch chat messages', err);
              },
            });
        }
      });

    // Subscribe to messages
    this.chatService.messages$
      .pipe(takeUntil(this.destroy$))
      .subscribe((messages) => {
        this.messages = messages;
        this.scrollToBottom();
      });

    // Subscribe to typing status
    this.chatService.isTyping$
      .pipe(takeUntil(this.destroy$))
      .subscribe((isTyping) => {
        this.isTyping = isTyping;
      });

    this.router.events.subscribe((event) => {
      if (event instanceof NavigationStart) {
        const url = event.url;
        // Match /chat/:id?new=true
        const chatNewRegex = /^\/chat\/[^\/\?]+\?new=true$/;
        if (!chatNewRegex.test(url)) {
          if (this.messageSubscription) {
            this.messageSubscription.unsubscribe();
            this.messageSubscription = null;
          }
        }
      }
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  // Send user message
  submitMessageAfterChatCreated(message: string): void {
    if (this.messageSubscription) {
      this.messageSubscription.unsubscribe();
    }
    this.messageSubscription = this.chatService
      .sendMessage(message, this.chatId!)
      .subscribe({
        next: (_response) => {},
        error: (error) => {
          console.error('Error sending message:', error);
        },
        complete: () => {},
      });

    this.chatForm.reset();
  }

  onSubmit(): void {
    if (this.chatForm.valid && !this.isTyping) {
      const message = this.chatForm.get('message')?.value.trim();
      if (message) {
        // Create a new chat if chatId is null
        if (!this.chatId) {
          this.chatService
            .createChat()
            .pipe(takeUntil(this.destroy$))
            .subscribe({
              next: (response) => {
                this.chatId = response.id;
                this.shouldSkipFetchingMessages = true;
                this.router.navigate(['/chat', this.chatId], {
                  queryParams: { new: true },
                });
                this.submitMessageAfterChatCreated(message);
              },
              error: (error) => {
                console.error('Error creating chat:', error);
              },
              complete: () => {},
            });
        } else {
          this.submitMessageAfterChatCreated(message);
        }
      }
    }
  }

  addReaction(messageId: string, reaction: ReactionType): void {
    this.chatService
      .addReaction(messageId, reaction)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (_response) => {
          this.messages = this.messages.map((msg) =>
            msg.id === messageId ? { ...msg, reaction: reaction } : msg
          );
          this.chatService.setMessages(this.messages);
        },
        error: (error) => {
          console.error('Error adding reaction:', error);
        },
      });
  }

  newChat(): void {
    this.chatService.clearMessages();
    this.messages = [];
    this.chatId = null;
    this.router.navigate(['/']);
  }

  stopResponse(): void {
    if (this.messageSubscription) {
      this.messageSubscription.unsubscribe();
      this.messageSubscription = null;
    }
  }

  onKeyPress(event: KeyboardEvent): void {
    if (event.key === 'Enter' && !event.shiftKey) {
      event.preventDefault();
      this.onSubmit();
    }
  }

  private scrollToBottom(): void {
    setTimeout(() => {
      const chatContainer = document.getElementById('chat-messages');
      if (chatContainer) {
        chatContainer.scrollTop = chatContainer.scrollHeight;
      }
    }, 100);
  }
}
