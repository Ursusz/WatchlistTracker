import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface Comment {
  id: number;
  authorId: string;
  authorName: string;
  text: string;
  createdAt: Date;
}

export interface CreateCommentRequest {
  reviewId: number;
  text: string;
}

@Injectable({
  providedIn: 'root'
})
export class CommentService {
  private apiUrl = `${environment.apiUrl}/comments`;

  constructor(private http: HttpClient) {}

  getCommentsByReview(reviewId: number): Observable<Comment[]> {
    return this.http.get<Comment[]>(`${this.apiUrl}/review/${reviewId}`);
  }

  createComment(comment: CreateCommentRequest): Observable<number> {
    return this.http.post<number>(this.apiUrl, comment);
  }

  deleteComment(commentId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${commentId}`);
  }
}
