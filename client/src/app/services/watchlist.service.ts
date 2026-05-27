import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../environments/environment';

export interface WatchlistItem {
  movieId: number;
  movieTitle: string;
  movieImage: string | null;
  dateAdded: string;
}

@Injectable({
  providedIn: 'root'
})
export class WatchlistService {
  private apiUrl = `${environment.apiUrl}/watchlist`;
  private readonly apiBaseUrl = environment.apiUrl.replace(/\/api\/?$/, '');

  constructor(private http: HttpClient) {}

  getWatchlist(): Observable<WatchlistItem[]> {
    return this.http.get<WatchlistItem[]>(this.apiUrl).pipe(
      map((items) =>
        items.map((item) => ({
          ...item,
          movieImage: this.normalizeImageUrl(item.movieImage)
        }))
      )
    );
  }

  addToWatchlist(movieId: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${movieId}`, {});
  }

  removeFromWatchlist(movieId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${movieId}`);
  }

  isInWatchlist(movieId: number): Observable<boolean> {
    return this.http.get<boolean>(`${this.apiUrl}/check/${movieId}`);
  }

  private normalizeImageUrl(imagePath: string | null): string | null {
    if (!imagePath) return null;
    if (/^https?:\/\//i.test(imagePath)) return imagePath;

    if (/^\/?api\/movies\/image\//i.test(imagePath)) {
      const normalizedPath = imagePath.startsWith('/') ? imagePath : `/${imagePath}`;
      return `${this.apiBaseUrl}${normalizedPath}`;
    }

    if (/^\/?images\//i.test(imagePath)) {
      const fileName = imagePath.split('/').pop();
      if (!fileName) return null;
      return `${this.apiBaseUrl}/api/movies/image/${encodeURIComponent(fileName)}`;
    }

    return `${this.apiBaseUrl}/api/movies/image/${encodeURIComponent(imagePath)}`;
  }
}
