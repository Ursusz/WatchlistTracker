import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface Review {
  id: number;
  authorId: string;
  authorName: string;
  movieTitle: string;
  rating: number;
  content: string;
  watchedAt: Date;
  commentCount: number;
}

export interface CreateReviewRequest {
  movieId: number;
  rating: number;
  content: string;
  visibility: number;
}

export interface UpdateReviewRequest {
  rating: number;
  content: string;
  visibility: number;
}

@Injectable({
  providedIn: 'root'
})
export class ReviewService {
  private apiUrl = `${environment.apiUrl}/reviews`;

  constructor(private http: HttpClient) {}

  getGlobalFeed(): Observable<Review[]> {
    return this.http.get<Review[]>(`${this.apiUrl}/feed/global`);
  }

  getFriendsFeed(): Observable<Review[]> {
    return this.http.get<Review[]>(`${this.apiUrl}/feed/friends`);
  }

  createReview(review: CreateReviewRequest): Observable<number> {
    return this.http.post<number>(this.apiUrl, review);
  }

  getReviewsByUser(userId: string): Observable<Review[]> {
    return this.http.get<Review[]>(`${this.apiUrl}/user/${userId}`);
  }

  getReviewsByMovie(movieId: number): Observable<Review[]> {
    return this.http.get<Review[]>(`${this.apiUrl}/movie/${movieId}`);
  }

  updateReview(id: number, review: UpdateReviewRequest): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, review);
  }

  deleteReview(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
