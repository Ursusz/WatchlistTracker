import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { MovieUploadComponent } from '../../components/movie-upload/movie-upload.component';
import { MovieImageUploadModalComponent } from '../../components/movie-image-upload-modal/movie-image-upload-modal.component';
import { Movie, MovieService } from '../../services/movie.service';
import { WatchlistService } from '../../services/watchlist.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-movies',
  standalone: true,
  imports: [CommonModule, FormsModule, NavbarComponent, MovieUploadComponent, MovieImageUploadModalComponent],
  templateUrl: './movies.component.html',
  styleUrl: './movies.component.css'
})
export class MoviesComponent implements OnInit {
  movies: Movie[] = [];
  watchlistIds = new Set<number>();
  loading = true;
  error: string | null = null;
  searchTerm = '';
  uploadModalOpen = false;
  selectedMovieForUpload: Movie | null = null;
  isAdmin = false;

  constructor(
    private movieService: MovieService,
    private watchlistService: WatchlistService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
    this.isAdmin = this.authService.isAdmin();
    this.loadData();
  }

  loadData() {
    this.loading = true;

    this.movieService.getMovies().subscribe({
      next: (movies) => {
        this.movies = movies;
        this.loadWatchlistStatus();
      },
      error: () => {
        this.error = 'Failed to load movies';
        this.loading = false;
      }
    });
  }

  loadWatchlistStatus() {
    this.watchlistService.getWatchlist().subscribe({
      next: (items) => {
        this.watchlistIds = new Set(items.map(i => i.movieId));
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  get filteredMovies(): Movie[] {
    if (!this.searchTerm.trim()) return this.movies;
    const term = this.searchTerm.toLowerCase();
    return this.movies.filter(m =>
      m.title.toLowerCase().includes(term) ||
      (m.categoryName?.toLowerCase().includes(term))
    );
  }

  isInWatchlist(movieId: number): boolean {
    return this.watchlistIds.has(movieId);
  }

  toggleWatchlist(movieId: number) {
    if (this.isInWatchlist(movieId)) {
      this.watchlistService.removeFromWatchlist(movieId).subscribe({
        next: () => this.watchlistIds.delete(movieId),
        error: () => this.error = 'Failed to remove from watchlist'
      });
    } else {
      this.watchlistService.addToWatchlist(movieId).subscribe({
        next: () => this.watchlistIds.add(movieId),
        error: (err) => this.error = err.error?.message || 'Failed to add to watchlist'
      });
    }
  }

  writeReview(movieId: number) {
    this.router.navigate(['/create-review'], { queryParams: { movieId } });
  }

  viewMovieReviews(movieId: number) {
    this.router.navigate(['/movies', movieId, 'reviews']);
  }

  openUploadModal(movie: Movie) {
    if (!this.isAdmin) return;
    this.selectedMovieForUpload = movie;
    this.uploadModalOpen = true;
  }

  closeUploadModal() {
    this.uploadModalOpen = false;
    this.selectedMovieForUpload = null;
  }

  onImageUploaded(imageUrl: string | null) {
    if (!imageUrl) return;

    if (this.selectedMovieForUpload) {
      this.selectedMovieForUpload.imagePath = imageUrl;
    }
    this.closeUploadModal();
  }
}
