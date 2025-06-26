export interface SendChatMessage {
  message: string;
  sender: 'user' | 'ai';
  chatId: string;
}
