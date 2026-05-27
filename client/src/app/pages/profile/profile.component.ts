import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { ReviewCardComponent } from '../../components/review-card/review-card.component';
import { AuthService } from '../../services/auth.service';
import { UserProfile, UserService } from '../../services/user.service';
import { FriendshipService } from '../../services/friendship.service';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, NavbarComponent, ReviewCardComponent],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit {
  userProfile: UserProfile | null = null;
  loading = true;
  error: string | null = null;
  userId: string = '';
  isOwnProfile = false;
  friendRequestSent = false;
  sendingRequest = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private userService: UserService,
    private authService: AuthService,
    private friendshipService: FriendshipService
  ) {}

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.userId = params.get('userId') || '';
      this.isOwnProfile = this.userId === this.authService.getUserId();
      if (this.userId) {
        this.loadProfile();
      }
    });
  }

  loadProfile() {
    this.loading = true;
    this.error = null;

    this.userService.getUserProfile(this.userId).subscribe({
      next: (profile) => {
        this.userProfile = profile;
        this.loading = false;
      },
      error: () => {
        this.error = 'Failed to load profile';
        this.loading = false;
      }
    });
  }

  sendFriendRequest() {
    if (this.isOwnProfile || this.sendingRequest) return;

    this.sendingRequest = true;
    this.friendshipService.sendFriendRequest(this.userId).subscribe({
      next: () => {
        this.friendRequestSent = true;
        this.sendingRequest = false;
      },
      error: (err) => {
        this.error = err.error?.message || 'Failed to send friend request';
        this.sendingRequest = false;
      }
    });
  }

  goToWatchlist() {
    if (this.isOwnProfile) {
      this.router.navigate(['/watchlist']);
    }
  }

  goBack() {
    this.router.navigate(['/feed']);
  }
}
