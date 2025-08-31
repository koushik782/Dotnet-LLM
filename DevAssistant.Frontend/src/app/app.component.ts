import { Component } from '@angular/core';
import { ChatService } from './services/chat.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ChatComponent } from './components/chat.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule, ChatComponent],
  template: `
    <div class="chat-container">
      <div class="chat-header">
        <h1 class="mb-0">
          <i class="bi bi-robot me-2"></i>
          DevAssistant
        </h1>
        <small class="opacity-75">AI-Powered Developer Helper powered by Ollama</small>
      </div>
      
      <app-chat></app-chat>
    </div>
  `,
  styles: []
})
export class AppComponent {
  constructor(public chatService: ChatService) {}
} 