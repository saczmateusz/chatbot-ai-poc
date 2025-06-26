import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { faker } from '@faker-js/faker';
import { ChatMessage } from '../dtos/chat-message.model';
import { ChatResponse } from '../dtos/chat-response.model';
import { SendChatMessage } from '../dtos/send-chat-message.model';
import { ReactionType } from '../types/reaction-type';

@Injectable({
  providedIn: 'root',
})
export class ChatService {
  private readonly baseUrl = 'https://localhost:7218';
  private currentMessages: ChatMessage[] = [];

  private messagesSubject = new BehaviorSubject<ChatMessage[]>([]);
  private isTypingSubject = new BehaviorSubject<boolean>(false);

  public messages$ = this.messagesSubject.asObservable();
  public isTyping$ = this.isTypingSubject.asObservable();

  constructor(private http: HttpClient) {}

  createChat(): Observable<{ id: string }> {
    return this.http.post<{ id: string }>(
      `${this.baseUrl}/Chat/Create`,
      { name: faker.lorem.words({ min: 1, max: 3 }) },
      {
        headers: {
          'Content-Type': 'application/json',
        },
      }
    );
  }

  addReaction(messageId: string, reaction: ReactionType) {
    return this.http.post<{ id: string; reaction: ReactionType }>(
      `${this.baseUrl}/Message/Update`,
      { id: messageId, reaction: reaction },
      {
        headers: {
          'Content-Type': 'application/json',
        },
      }
    );
  }

  sendMessage(message: string, chatId: string): Observable<ChatResponse> {
    // Add user message to chat
    const userMessage: ChatMessage = {
      id: this.generateId(),
      content: message,
      sender: 'user',
      chatId: chatId,
      reaction: ReactionType.None,
    };
    this.addMessage(userMessage);
    this.isTypingSubject.next(true);

    // Create bot response message placeholder
    const botMessage: ChatMessage = {
      id: 'temp',
      content: '',
      sender: 'ai',
      chatId: chatId,
      reaction: ReactionType.None,
    };
    this.addMessage(botMessage);

    // Create observable for Server-Sent Events
    return new Observable<ChatResponse>((observer) => {
      const controller = new AbortController();
      const command: SendChatMessage = {
        message: message,
        sender: 'user',
        chatId: chatId,
      };

      this.sendMessageWithFetch(command, controller.signal)
        .then((response) => {
          if (!response.body) {
            observer.error('No response body');
            return;
          }

          const reader = response.body.getReader();
          const decoder = new TextDecoder();

          const readStream = async () => {
            try {
              while (true) {
                const { done, value } = await reader.read();

                if (done) {
                  this.isTypingSubject.next(false);
                  observer.complete();
                  break;
                }

                const chunk = decoder.decode(value, { stream: true });
                const lines = chunk.split('\n');

                for (const line of lines) {
                  if (line.startsWith('data: ')) {
                    try {
                      const data = JSON.parse(line.substring(6));
                      const chatResponse: ChatResponse = {
                        messageId: data.Id,
                        content: data.Content,
                        isComplete: data.IsComplete,
                      };

                      // Update the bot message with new content
                      this.updateBotMessage(
                        chatResponse.messageId,
                        chatResponse.content,
                        chatResponse.isComplete
                      );
                      observer.next(chatResponse);

                      if (chatResponse.isComplete) {
                        this.isTypingSubject.next(false);
                      }
                    } catch (e) {
                      console.error('Error parsing SSE data:', e);
                    }
                  }
                }
              }
            } catch (error) {
              this.isTypingSubject.next(false);
              observer.error(error);
            }
          };

          readStream();
        })
        .catch((error) => {
          this.isTypingSubject.next(false);
          observer.error(error);
        });

      // Cleanup function
      return () => {
        console.log('Cleaning up SSE connection');
        controller.abort();
        this.isTypingSubject.next(false);
      };
    });
  }

  private async sendMessageWithFetch(
    command: SendChatMessage,
    signal?: AbortSignal
  ): Promise<Response> {
    return fetch(`${this.baseUrl}/Message/AskAI`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Accept: 'text/event-stream',
      },
      body: JSON.stringify(command),
      signal,
    });
  }

  private addMessage(message: ChatMessage): void {
    this.currentMessages = [...this.currentMessages, message];
    this.messagesSubject.next(this.currentMessages);
  }

  setMessages(messages: ChatMessage[]): void {
    this.currentMessages = [...messages];
  }

  private updateBotMessage(
    messageId: string,
    newContent: string,
    isComplete: boolean
  ): void {
    let messageIndex = this.currentMessages.findIndex(
      (m) => m.id === messageId
    );
    if (messageIndex === -1) {
      messageIndex = this.currentMessages.findIndex((m) => m.id === 'temp');
    }
    if (messageIndex !== -1) {
      const updatedMessages = [...this.currentMessages];
      updatedMessages[messageIndex] = {
        ...updatedMessages[messageIndex],
        id: messageId,
        content: updatedMessages[messageIndex].content + newContent,
      };
      this.currentMessages = updatedMessages;
      this.messagesSubject.next(this.currentMessages);
    }
  }

  clearMessages(): void {
    this.currentMessages = [];
    this.messagesSubject.next(this.currentMessages);
  }

  private generateId(): string {
    return Math.random().toString(36).substring(2) + Date.now().toString(36);
  }
}
