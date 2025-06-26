import { ReactionType } from '../types/reaction-type';

export interface ChatMessage {
  id?: string;
  content: string;
  sender: 'user' | 'ai';
  chatId: string;
  reaction: ReactionType;
}
