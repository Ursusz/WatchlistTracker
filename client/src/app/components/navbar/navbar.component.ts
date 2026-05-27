import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { FriendshipService } from '../../services/friendship.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent implements OnInit, OnDestroy {
  fullName: string = '';
  pendingRequestCount = 0;
  showMenu = false;
  showNotifications = false;
  private refreshInterval?: ReturnType<typeof setInterval>;

  constructor(
    private authService: AuthService,
    private friendshipService: FriendshipService,
    private router: Router
  ) {
    this.fullName = this.authService.getFullName() || 'User';
  }

  ngOnInit() {
    this.loadPendingRequests();
    this.refreshInterval = setInterval(() => {
      this.loadPendingRequests();
    }, 30000);
  }

  ngOnDestroy() {
    if (this.refreshInterval) {
      clearInterval(this.refreshInterval);
    }
  }

  loadPendingRequests() {
    this.friendshipService.getPendingRequestCount().subscribe({
      next: (count) => {
        this.pendingRequestCount = count;
      },
      error: () => {}
    });
  }

  toggleMenu() {
    this.showMenu = !this.showMenu;
    this.showNotifications = false;
  }

  toggleNotifications() {
    this.showNotifications = !this.showNotifications;
    this.showMenu = false;
  }

  goToFeed() {
    this.router.navigate(['/feed']);
  }

  goToProfile() {
    const userId = this.authService.getUserId();
    this.router.navigate([`/profile/${userId}`]);
    this.showMenu = false;
  }

  goToMyReviews() {
    this.router.navigate(['/my-reviews']);
    this.showMenu = false;
  }

  goToWatchlist() {
    this.router.navigate(['/watchlist']);
    this.showMenu = false;
  }

  goToMovies() {
    this.router.navigate(['/movies']);
    this.showMenu = false;
  }

  goToFriendRequests() {
    this.router.navigate(['/friend-requests']);
    this.showNotifications = false;
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
