import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    loadComponent: () =>
      import('./chat/chat.component').then((m) => m.ChatComponent),
  },
  {
    path: 'chat/:id',
    loadComponent: () =>
      import('./chat/chat.component').then((m) => m.ChatComponent),
  },
];
