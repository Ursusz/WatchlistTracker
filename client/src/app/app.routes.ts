import { Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { FeedComponent } from './pages/feed/feed.component';
import { ProfileComponent } from './pages/profile/profile.component';
import { FriendRequestsComponent } from './pages/friend-requests/friend-requests.component';
import { CreateReviewComponent } from './pages/create-review/create-review.component';
import { WatchlistComponent } from './pages/watchlist/watchlist.component';
import { MyReviewsComponent } from './pages/my-reviews/my-reviews.component';
import { MoviesComponent } from './pages/movies/movies.component';
import { MovieReviewsComponent } from './pages/movie-reviews/movie-reviews.component';
import { AuthGuard } from './guards/auth.guard';
import { GuestGuard } from './guards/guest.guard';

export const routes: Routes = [
  { path: '', redirectTo: '/feed', pathMatch: 'full' },
  { path: 'login', component: LoginComponent, canActivate: [GuestGuard] },
  { path: 'register', component: RegisterComponent, canActivate: [GuestGuard] },
  { path: 'feed', component: FeedComponent, canActivate: [AuthGuard] },
  { path: 'profile/:userId', component: ProfileComponent, canActivate: [AuthGuard] },
  { path: 'friend-requests', component: FriendRequestsComponent, canActivate: [AuthGuard] },
  { path: 'create-review', component: CreateReviewComponent, canActivate: [AuthGuard] },
  { path: 'watchlist', component: WatchlistComponent, canActivate: [AuthGuard] },
  { path: 'my-reviews', component: MyReviewsComponent, canActivate: [AuthGuard] },
  { path: 'movies', component: MoviesComponent, canActivate: [AuthGuard] },
  { path: 'movies/:movieId/reviews', component: MovieReviewsComponent, canActivate: [AuthGuard] },
  { path: '**', redirectTo: '/feed' }
];
