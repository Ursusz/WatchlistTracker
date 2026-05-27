import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
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

  constructor(private http: HttpClient) {}

  getWatchlist(): Observable<WatchlistItem[]> {
    return this.http.get<WatchlistItem[]>(this.apiUrl);
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
}
