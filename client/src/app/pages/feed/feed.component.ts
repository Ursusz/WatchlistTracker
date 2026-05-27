import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { ReviewService, Review } from '../../services/review.service';
import { ReviewCardComponent } from '../../components/review-card/review-card.component';
import { NavbarComponent } from '../../components/navbar/navbar.component';

@Component({
  selector: 'app-feed',
  standalone: true,
  imports: [CommonModule, ReviewCardComponent, NavbarComponent],
  templateUrl: './feed.component.html',
  styleUrl: './feed.component.css'
})
export class FeedComponent implements OnInit {
  reviews: Review[] = [];
  loading = true;
  error: string | null = null;
  activeTab = 'global';

  constructor(
    private reviewService: ReviewService,
    private router: Router
  ) {}

  ngOnInit() {
    this.loadFeed();
  }

  loadFeed() {
    this.loading = true;
    this.error = null;

    const feedObservable = this.activeTab === 'global'
      ? this.reviewService.getGlobalFeed()
      : this.reviewService.getFriendsFeed();

    feedObservable.subscribe({
      next: (reviews) => {
        this.reviews = reviews;
        this.loading = false;
      },
      error: () => {
        this.error = 'Failed to load feed. Please try logging in again.';
        this.loading = false;
      }
    });
  }

  switchTab(tab: string) {
    this.activeTab = tab;
    this.loadFeed();
  }

  createReview() {
    this.router.navigate(['/create-review']);
  }
}
