import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, Subject, BehaviorSubject } from 'rxjs';
import { ChatMessage, ChatRequest, PromptTemplate, SseEvent, HealthStatus, FeedbackType } from '../models/chat.models';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  private readonly apiBaseUrl = 'https://localhost:7001/api';
  
  private messagesSubject = new BehaviorSubject<ChatMessage[]>([]);
  public messages$ = this.messagesSubject.asObservable();
  
  private isConnectedSubject = new BehaviorSubject<boolean>(false);
  public isConnected$ = this.isConnectedSubject.asObservable();
  
  private isStreamingSubject = new BehaviorSubject<boolean>(false);
  public isStreaming$ = this.isStreamingSubject.asObservable();

  constructor(private http: HttpClient) {
    this.checkHealth();
  }

  public async sendMessage(request: ChatRequest): Promise<void> {
    // Add user message to chat
    const userMessage: ChatMessage = {
      id: this.generateId(),
      content: request.userInput,
      role: 'user',
      timestamp: new Date(),
      template: request.template
    };
    
    this.addMessage(userMessage);
    
    // Create assistant message placeholder
    const assistantMessage: ChatMessage = {
      id: this.generateId(),
      content: '',
      role: 'assistant',
      timestamp: new Date(),
      isStreaming: true
    };
    
    this.addMessage(assistantMessage);
    this.isStreamingSubject.next(true);

    try {
      await this.streamChatResponse(request, assistantMessage);
    } catch (error) {
      console.error('Error sending message:', error);
      this.updateMessage(assistantMessage.id, {
        content: 'Sorry, there was an error processing your request. Please make sure Ollama is running and try again.',
        isStreaming: false
      });
    } finally {
      this.isStreamingSubject.next(false);
    }
  }

  private async streamChatResponse(request: ChatRequest, assistantMessage: ChatMessage): Promise<void> {
    return new Promise((resolve, reject) => {
      const eventSource = new EventSource(`${this.apiBaseUrl}/chat/stream`, {
        // Note: We need to send the request body via POST, but EventSource only supports GET
        // We'll need to modify this approach
      });

      // Since EventSource doesn't support POST with body, we'll use fetch with streaming
      this.streamWithFetch(request, assistantMessage)
        .then(resolve)
        .catch(reject);
    });
  }

  private async streamWithFetch(request: ChatRequest, assistantMessage: ChatMessage): Promise<void> {
    const response = await fetch(`${this.apiBaseUrl}/chat/stream`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Accept': 'text/event-stream',
      },
      body: JSON.stringify(request)
    });

    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }

    if (!response.body) {
      throw new Error('No response body available');
    }

    const reader = response.body.getReader();
    const decoder = new TextDecoder();
    let accumulatedContent = '';

    try {
      while (true) {
        const { done, value } = await reader.read();
        
        if (done) break;

        const chunk = decoder.decode(value, { stream: true });
        const lines = chunk.split('\n');

        for (const line of lines) {
          if (line.startsWith('data: ')) {
            const eventData = line.slice(6);
            
            if (eventData.trim() === '') continue;

            try {
              const sseEvent: SseEvent = JSON.parse(eventData);
              
              switch (sseEvent.type) {
                case 'start':
                  // Response starting
                  break;
                  
                case 'chunk':
                  accumulatedContent += sseEvent.data;
                  this.updateMessage(assistantMessage.id, {
                    content: accumulatedContent,
                    isStreaming: true
                  });
                  break;
                  
                case 'complete':
                  this.updateMessage(assistantMessage.id, {
                    content: accumulatedContent,
                    isStreaming: false
                  });
                  return;
                  
                case 'error':
                  this.updateMessage(assistantMessage.id, {
                    content: `Error: ${sseEvent.data}`,
                    isStreaming: false
                  });
                  throw new Error(sseEvent.data);
                  
                case 'cancelled':
                  this.updateMessage(assistantMessage.id, {
                    content: accumulatedContent + '\n\n[Response was cancelled]',
                    isStreaming: false
                  });
                  return;
              }
            } catch (parseError) {
              console.warn('Failed to parse SSE event:', eventData, parseError);
            }
          }
        }
      }
    } finally {
      reader.releaseLock();
    }
  }

  public getTemplates(): Observable<PromptTemplate[]> {
    return this.http.get<PromptTemplate[]>(`${this.apiBaseUrl}/chat/templates`);
  }

  public checkHealth(): void {
    this.http.get<HealthStatus>(`${this.apiBaseUrl}/chat/health`)
      .subscribe({
        next: (health) => {
          this.isConnectedSubject.next(health.isHealthy);
        },
        error: () => {
          this.isConnectedSubject.next(false);
        }
      });
  }

  public submitFeedback(messageId: string, feedbackType: FeedbackType): void {
    this.updateMessage(messageId, { feedback: feedbackType });
    
    // Here you could also send feedback to the backend
    // this.http.post(`${this.apiBaseUrl}/feedback`, { messageId, feedbackType }).subscribe();
  }

  public clearMessages(): void {
    this.messagesSubject.next([]);
  }

  public getMessages(): ChatMessage[] {
    return this.messagesSubject.value;
  }

  private addMessage(message: ChatMessage): void {
    const currentMessages = this.messagesSubject.value;
    this.messagesSubject.next([...currentMessages, message]);
  }

  private updateMessage(messageId: string, updates: Partial<ChatMessage>): void {
    const currentMessages = this.messagesSubject.value;
    const updatedMessages = currentMessages.map(msg => 
      msg.id === messageId ? { ...msg, ...updates } : msg
    );
    this.messagesSubject.next(updatedMessages);
  }

  private generateId(): string {
    return Date.now().toString(36) + Math.random().toString(36).substr(2);
  }
} 