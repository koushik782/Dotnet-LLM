import { Component, OnInit, OnDestroy, ViewChild, ElementRef, AfterViewChecked } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ChatService } from '../services/chat.service';
import { ChatMessage, PromptTemplate, FeedbackType } from '../models/chat.models';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="chat-messages" #chatMessages>
      <!-- Status Indicator -->
      <div class="status-indicator" [class.error]="!(isConnected$ | async)">
        <i class="bi" [class.bi-wifi]="isConnected$ | async" [class.bi-wifi-off]="!(isConnected$ | async)"></i>
        <span>{{ (isConnected$ | async) ? 'Connected to Ollama' : 'Ollama not available' }}</span>
      </div>

      <!-- Messages -->
      <div *ngFor="let message of messages$ | async" class="message" [class.user]="message.role === 'user'" [class.assistant]="message.role === 'assistant'">
        <div class="message-avatar">
          <i class="bi" [class.bi-person]="message.role === 'user'" [class.bi-robot]="message.role === 'assistant'"></i>
        </div>
        
        <div class="message-content">
          <!-- User message content -->
          <div *ngIf="message.role === 'user'">
            {{ message.content }}
            <small *ngIf="message.template" class="d-block mt-1 opacity-75">
              Template: {{ getTemplateName(message.template) }}
            </small>
          </div>
          
          <!-- Assistant message content -->
          <div *ngIf="message.role === 'assistant'">
            <!-- Streaming content with markdown support -->
            <div [innerHTML]="formatMessage(message.content)"></div>
            
            <!-- Typing indicator -->
            <div *ngIf="message.isStreaming" class="typing-indicator">
              <span>AI is thinking</span>
              <div class="typing-dots">
                <div class="typing-dot"></div>
                <div class="typing-dot"></div>
                <div class="typing-dot"></div>
              </div>
            </div>
            
            <!-- Feedback buttons (only for completed assistant messages) -->
            <div *ngIf="!message.isStreaming && message.content" class="feedback-buttons">
              <button 
                class="feedback-btn" 
                [class.active]="message.feedback === 'thumbs_up'"
                (click)="submitFeedback(message.id, 'thumbs_up')"
                title="This response was helpful">
                👍
              </button>
              <button 
                class="feedback-btn" 
                [class.active]="message.feedback === 'thumbs_down'"
                [class.thumbs-down]="message.feedback === 'thumbs_down'"
                (click)="submitFeedback(message.id, 'thumbs_down')"
                title="This response was not helpful">
                👎
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Input Area -->
    <div class="chat-input-area">
      <div class="input-container">
        <!-- Template Selector -->
        <div class="template-selector">
          <select 
            class="form-select" 
            [(ngModel)]="selectedTemplate"
            [disabled]="isStreaming$ | async">
            <option value="">General Assistant</option>
            <option *ngFor="let template of templates" [value]="template.key">
              {{ template.name }}
            </option>
          </select>
        </div>

        <!-- Message Input -->
        <div class="message-input-group">
          <textarea
            class="message-input"
            [(ngModel)]="userInput"
            placeholder="Ask me anything about .NET, C#, ASP.NET Core, Entity Framework, SQL Server..."
            [disabled]="isStreaming$ | async"
            (keydown.enter)="onEnterKey($event)"
            rows="1"
            #messageInput>
          </textarea>
          
          <button 
            class="send-button"
            (click)="sendMessage()"
            [disabled]="!userInput.trim() || (isStreaming$ | async)">
            <i class="bi bi-send"></i>
          </button>
        </div>
      </div>
      
      <!-- Clear Chat Button -->
      <div class="text-center mt-2">
        <button 
          class="btn btn-outline-secondary btn-sm"
          (click)="clearChat()"
          [disabled]="(messages$ | async)?.length === 0">
          <i class="bi bi-trash me-1"></i>
          Clear Chat
        </button>
      </div>
    </div>
  `,
  styles: []
})
export class ChatComponent implements OnInit, OnDestroy, AfterViewChecked {
  @ViewChild('chatMessages') chatMessages!: ElementRef;
  @ViewChild('messageInput') messageInput!: ElementRef;

  userInput = '';
  selectedTemplate = '';
  templates: PromptTemplate[] = [];
  
  messages$ = this.chatService.messages$;
  isConnected$ = this.chatService.isConnected$;
  isStreaming$ = this.chatService.isStreaming$;
  
  private subscriptions: Subscription[] = [];

  constructor(private chatService: ChatService) {}

  ngOnInit(): void {
    this.loadTemplates();
    
    // Auto-resize textarea
    this.subscriptions.push(
      this.messages$.subscribe(() => {
        setTimeout(() => this.scrollToBottom(), 100);
      })
    );
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  ngAfterViewChecked(): void {
    this.autoResizeTextarea();
  }

  async sendMessage(): Promise<void> {
    if (!this.userInput.trim() || this.isStreaming$.value) return;

    const request = {
      userInput: this.userInput.trim(),
      template: this.selectedTemplate || undefined,
      model: 'mistral'
    };

    this.userInput = '';
    this.selectedTemplate = '';
    
    await this.chatService.sendMessage(request);
  }

  onEnterKey(event: KeyboardEvent): void {
    if (event.shiftKey) return; // Allow new lines with Shift+Enter
    
    event.preventDefault();
    this.sendMessage();
  }

  submitFeedback(messageId: string, feedbackType: FeedbackType): void {
    this.chatService.submitFeedback(messageId, feedbackType);
  }

  clearChat(): void {
    this.chatService.clearMessages();
  }

  getTemplateName(templateKey: string): string {
    const template = this.templates.find(t => t.key === templateKey);
    return template?.name || templateKey;
  }

  formatMessage(content: string): string {
    // Simple markdown-like formatting for code blocks
    return content
      .replace(/```(\w+)?\n([\s\S]*?)```/g, '<div class="code-block"><pre><code>$2</code><button class="copy-button" onclick="navigator.clipboard.writeText(`$2`)">Copy</button></pre></div>')
      .replace(/`([^`]+)`/g, '<code>$1</code>')
      .replace(/\*\*([^*]+)\*\*/g, '<strong>$1</strong>')
      .replace(/\*([^*]+)\*/g, '<em>$1</em>')
      .replace(/\n/g, '<br>');
  }

  private loadTemplates(): void {
    this.chatService.getTemplates().subscribe({
      next: (templates) => {
        this.templates = templates;
      },
      error: (error) => {
        console.error('Failed to load templates:', error);
        // Fallback templates
        this.templates = [
          { key: 'error-explain', name: 'Error Explainer', description: 'Analyze and explain errors' },
          { key: 'refactor', name: 'Code Refactoring', description: 'Improve code quality' },
          { key: 'sql-helper', name: 'SQL Helper', description: 'Database and SQL assistance' }
        ];
      }
    });
  }

  private scrollToBottom(): void {
    try {
      const element = this.chatMessages.nativeElement;
      element.scrollTop = element.scrollHeight;
    } catch (err) {
      console.warn('Failed to scroll to bottom:', err);
    }
  }

  private autoResizeTextarea(): void {
    if (this.messageInput?.nativeElement) {
      const textarea = this.messageInput.nativeElement;
      textarea.style.height = 'auto';
      textarea.style.height = Math.min(textarea.scrollHeight, 150) + 'px';
    }
  }
} 