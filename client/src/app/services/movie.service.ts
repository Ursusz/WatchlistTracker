import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface Movie {
  id: number;
  title: string;
  description: string;
  publishedAt: string;
  categoryName: string | null;
  imagePath: string | null;
  averageRating: number;
  reviewCount: number;
}

@Injectable({
  providedIn: 'root'
})
export class MovieService {
  private apiUrl = `${environment.apiUrl}/movies`;

  constructor(private http: HttpClient) {}

  getMovies(): Observable<Movie[]> {
    return this.http.get<Movie[]>(this.apiUrl);
  }

  getMovie(id: number): Observable<Movie> {
    return this.http.get<Movie>(`${this.apiUrl}/${id}`);
  }

  uploadImage(file: File): Observable<{ url: string }> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<{ url: string }>(`${this.apiUrl}/upload-image`, formData);
  }

  uploadImageForMovie(movieId: number, file: File): Observable<{ url: string; message: string }> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<{ url: string; message: string }>(`${this.apiUrl}/${movieId}/upload-image`, formData);
  }
}
