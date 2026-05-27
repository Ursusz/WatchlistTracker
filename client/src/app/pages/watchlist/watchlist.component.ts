import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { WatchlistItem, WatchlistService } from '../../services/watchlist.service';

@Component({
  selector: 'app-watchlist',
  standalone: true,
  imports: [CommonModule, NavbarComponent],
  templateUrl: './watchlist.component.html',
  styleUrl: './watchlist.component.css'
})
export class WatchlistComponent implements OnInit {
  items: WatchlistItem[] = [];
  loading = true;
  error: string | null = null;

  constructor(
    private watchlistService: WatchlistService,
    private router: Router
  ) {}

  ngOnInit() {
    this.loadWatchlist();
  }

  loadWatchlist() {
    this.loading = true;
    this.watchlistService.getWatchlist().subscribe({
      next: (items) => {
        this.items = items;
        this.loading = false;
      },
      error: () => {
        this.error = 'Failed to load watchlist';
        this.loading = false;
      }
    });
  }

  removeFromWatchlist(movieId: number) {
    this.watchlistService.removeFromWatchlist(movieId).subscribe({
      next: () => this.loadWatchlist(),
      error: () => this.error = 'Failed to remove movie'
    });
  }

  writeReview(movieId: number) {
    this.router.navigate(['/create-review'], { queryParams: { movieId } });
  }

  browseMovies() {
    this.router.navigate(['/movies']);
  }
}
