import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MovieService } from '../../services/movie.service';

@Component({
  selector: 'app-movie-upload',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './movie-upload.component.html',
  styleUrl: './movie-upload.component.css'
})
export class MovieUploadComponent {
  uploadedImageUrl: string | null = null;
  error: string | null = null;
  loading = false;

  constructor(private movieService: MovieService) {}

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (!file) return;

    if (!file.type.startsWith('image/')) {
      this.error = 'Please select an image file';
      return;
    }

    if (file.size > 5 * 1024 * 1024) {
      this.error = 'File size must be less than 5MB';
      return;
    }

    this.loading = true;
    this.error = null;

    this.movieService.uploadImage(file).subscribe({
      next: (response) => {
        this.uploadedImageUrl = response.url;
        this.loading = false;
      },
      error: (err) => {
        this.error = err.error?.message || 'Upload failed';
        this.loading = false;
      }
    });
  }
}
