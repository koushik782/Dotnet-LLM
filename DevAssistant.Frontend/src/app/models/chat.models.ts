export interface ChatMessage {
  id: string;
  content: string;
  role: 'user' | 'assistant';
  timestamp: Date;
  template?: string;
  isStreaming?: boolean;
  feedback?: FeedbackType;
}

export interface ChatRequest {
  userInput: string;
  template?: string;
  model?: string;
}

export interface PromptTemplate {
  key: string;
  name: string;
  description: string;
}

export interface SseEvent {
  type: 'start' | 'chunk' | 'complete' | 'error' | 'cancelled';
  data: string;
  timestamp: Date;
}

export interface HealthStatus {
  isHealthy: boolean;
  ollamaStatus: string;
  timestamp: Date;
}

export type FeedbackType = 'thumbs_up' | 'thumbs_down' | null;

export interface ConversationHistory {
  conversations: ChatMessage[][];
  currentConversation: ChatMessage[];
}

export interface AppSettings {
  apiBaseUrl: string;
  defaultModel: string;
  enableAutoScroll: boolean;
  enableTypingIndicator: boolean;
} 