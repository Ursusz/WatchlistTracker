import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MovieService } from '../../services/movie.service';

@Component({
  selector: 'app-movie-image-upload-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './movie-image-upload-modal.component.html',
  styleUrl: './movie-image-upload-modal.component.css'
})
export class MovieImageUploadModalComponent {
  @Input() movieId: number | null = null;
  @Input() movieTitle: string = '';
  @Input() isOpen = false;
  @Output() close = new EventEmitter<void>();
  @Output() uploaded = new EventEmitter<string>();

  loading = false;
  error: string | null = null;
  preview: string | null = null;
  selectedFile: File | null = null;

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

    this.selectedFile = file;
    this.error = null;

    // Show preview
    const reader = new FileReader();
    reader.onload = (e: any) => {
      this.preview = e.target.result;
    };
    reader.readAsDataURL(file);
  }

  uploadImage(): void {
    if (!this.selectedFile || !this.movieId) {
      this.error = 'Please select an image first';
      return;
    }

    this.loading = true;
    this.error = null;

    const formData = new FormData();
    formData.append('file', this.selectedFile);

    this.movieService.uploadImageForMovie(this.movieId, this.selectedFile).subscribe({
      next: (response) => {
        this.loading = false;
        this.uploaded.emit(response.url);
        this.closeModal();
      },
      error: (err) => {
        this.loading = false;
        this.error = err.error?.message || 'Upload failed';
      }
    });
  }

  closeModal(): void {
    this.isOpen = false;
    this.preview = null;
    this.selectedFile = null;
    this.error = null;
    this.close.emit();
  }
}
