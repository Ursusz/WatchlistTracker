import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { FriendshipService, FriendRequest } from '../../services/friendship.service';

@Component({
  selector: 'app-friend-requests',
  standalone: true,
  imports: [CommonModule, NavbarComponent],
  templateUrl: './friend-requests.component.html',
  styleUrl: './friend-requests.component.css'
})
export class FriendRequestsComponent implements OnInit {
  requests: FriendRequest[] = [];
  loading = true;

  constructor(private friendshipService: FriendshipService) {}

  ngOnInit() {
    this.loadRequests();
  }

  loadRequests() {
    this.loading = true;
    this.friendshipService.getPendingRequests().subscribe({
      next: (requests) => {
        this.requests = requests;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading requests:', err);
        this.loading = false;
      }
    });
  }

  acceptRequest(friendshipId: number) {
    this.friendshipService.acceptFriendRequest(friendshipId).subscribe({
      next: () => {
        this.loadRequests();
      },
      error: (err) => {
        console.error('Error accepting request:', err);
      }
    });
  }

  rejectRequest(friendshipId: number) {
    this.friendshipService.rejectFriendRequest(friendshipId).subscribe({
      next: () => {
        this.loadRequests();
      },
      error: (err) => {
        console.error('Error rejecting request:', err);
      }
    });
  }
}
