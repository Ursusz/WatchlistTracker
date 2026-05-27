import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
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
  private readonly apiBaseUrl = environment.apiUrl.replace(/\/api\/?$/, '');

  constructor(private http: HttpClient) {}

  getMovies(): Observable<Movie[]> {
    return this.http.get<Movie[]>(this.apiUrl).pipe(
      map((movies) => movies.map((movie) => this.normalizeMovieImagePath(movie)))
    );
  }

  getMovie(id: number): Observable<Movie> {
    return this.http.get<Movie>(`${this.apiUrl}/${id}`).pipe(
      map((movie) => this.normalizeMovieImagePath(movie))
    );
  }

  uploadImage(file: File): Observable<{ url: string | null }> {
    const formData = new FormData();
    formData.append('file', file);
      return this.http.post<{ url: string | null }>(`${this.apiUrl}/upload-image`, formData).pipe(
      map((response) => ({ ...response, url: this.normalizeImageUrl(response.url) }))
    );
  }

  uploadImageForMovie(movieId: number, file: File): Observable<{ url: string | null; message: string }> {
    const formData = new FormData();
    formData.append('file', file);
      return this.http.post<{ url: string | null; message: string }>(`${this.apiUrl}/${movieId}/upload-image`, formData).pipe(
      map((response) => ({ ...response, url: this.normalizeImageUrl(response.url) }))
    );
  }

  private normalizeMovieImagePath(movie: Movie): Movie {
    return {
      ...movie,
      imagePath: this.normalizeImageUrl(movie.imagePath)
    };
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
