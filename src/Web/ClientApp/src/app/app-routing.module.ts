import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { loginGuard } from './core/guards/login.guard';

const routes: Routes = [
  { path: '', redirectTo: '/pages', pathMatch: 'full' },
  { path: 'pages', loadChildren: () => import('./pages/pages.module').then((m) => m.PagesModule), canActivate: [authGuard] },
  { path: 'auth', loadChildren: () => import('./auth/auth.module').then((m) => m.AuthModule), canActivate: [loginGuard] },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
