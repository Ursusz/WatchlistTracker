import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { Review, ReviewService } from '../../services/review.service';
import { ReviewCardComponent } from '../../components/review-card/review-card.component';
import { Movie, MovieService } from '../../services/movie.service';

@Component({
  selector: 'app-movie-reviews',
  standalone: true,
  imports: [CommonModule, NavbarComponent, ReviewCardComponent],
  templateUrl: './movie-reviews.component.html',
  styleUrl: './movie-reviews.component.css'
})
export class MovieReviewsComponent implements OnInit {
  movieId: number | null = null;
  movie: Movie | null = null;
  reviews: Review[] = [];
  loading = true;
  error: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private reviewService: ReviewService,
    private movieService: MovieService
  ) {}

  ngOnInit(): void {
    const movieIdParam = this.route.snapshot.paramMap.get('movieId');
    const movieId = movieIdParam ? Number(movieIdParam) : NaN;

    if (!Number.isInteger(movieId) || movieId <= 0) {
      this.error = 'Invalid movie';
      this.loading = false;
      return;
    }

    this.movieId = movieId;
    this.loadMovieContext(movieId);
    this.loadReviews(movieId);
  }

  private loadMovieContext(movieId: number): void {
    this.movieService.getMovie(movieId).subscribe({
      next: (movie) => (this.movie = movie),
      error: () => {
        if (!this.error) this.error = 'Failed to load movie details';
      }
    });
  }

  private loadReviews(movieId: number): void {
    this.loading = true;
    this.error = null;

    this.reviewService.getReviewsByMovie(movieId).subscribe({
      next: (reviews) => {
        this.reviews = reviews;
        this.loading = false;
      },
      error: () => {
        this.error = 'Failed to load movie reviews';
        this.loading = false;
      }
    });
  }

  onReviewDeleted(reviewId: number): void {
    this.reviews = this.reviews.filter((review) => review.id !== reviewId);
  }

  backToMovies(): void {
    this.router.navigate(['/movies']);
  }
}
