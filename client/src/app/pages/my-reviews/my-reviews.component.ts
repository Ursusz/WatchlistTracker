import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { ReviewCardComponent } from '../../components/review-card/review-card.component';
import { AuthService } from '../../services/auth.service';
import { Review, ReviewService } from '../../services/review.service';

@Component({
  selector: 'app-my-reviews',
  standalone: true,
  imports: [CommonModule, NavbarComponent, ReviewCardComponent],
  templateUrl: './my-reviews.component.html',
  styleUrl: './my-reviews.component.css'
})
export class MyReviewsComponent implements OnInit {
  reviews: Review[] = [];
  loading = true;
  error: string | null = null;

  constructor(
    private reviewService: ReviewService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
    const userId = this.authService.getUserId();
    if (!userId) return;

    this.reviewService.getReviewsByUser(userId).subscribe({
      next: (reviews) => {
        this.reviews = reviews;
        this.loading = false;
      },
      error: () => {
        this.error = 'Failed to load reviews';
        this.loading = false;
      }
    });
  }

  deleteReview(reviewId: number) {
    if (!confirm('Delete this review?')) return;

    this.reviewService.deleteReview(reviewId).subscribe({
      next: () => {
        this.reviews = this.reviews.filter(r => r.id !== reviewId);
      },
      error: () => this.error = 'Failed to delete review'
    });
  }

  isOwnReview(review: Review): boolean {
    return review.authorName === this.authService.getFullName();
  }

  createReview() {
    this.router.navigate(['/create-review']);
  }
}
