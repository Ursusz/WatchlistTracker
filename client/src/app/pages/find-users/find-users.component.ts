import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { UserService } from '../../services/user.service';

export interface User {
  id: string;
  fullName: string;
  email: string;
  reviewCount: number;
}

@Component({
  selector: 'app-find-users',
  standalone: true,
  imports: [CommonModule, FormsModule, NavbarComponent],
  templateUrl: './find-users.component.html',
  styleUrl: './find-users.component.css'
})
export class FindUsersComponent implements OnInit {
  users: User[] = [];
  loading = false;
  error: string | null = null;
  searchTerm = '';
  noResults = false;

  constructor(
    private userService: UserService,
    private router: Router
  ) {}

  ngOnInit() {
    // Optionally load all users on init
    this.searchUsers();
  }

  searchUsers() {
    if (!this.searchTerm.trim()) {
      this.users = [];
      this.noResults = false;
      return;
    }

    this.loading = true;
    this.error = null;
    this.noResults = false;

    // This would need a new endpoint - for now show a message
    this.error = 'Search feature coming soon. Click on users from the Feed page to view their profiles and add them as friends!';
    this.loading = false;
  }

  viewProfile(userId: string) {
    this.router.navigate(['/profile', userId]);
  }
}
