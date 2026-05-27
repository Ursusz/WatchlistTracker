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
  @Output() uploaded = new EventEmitter<string | null>();

  loading = false;
  error: string | null = null;
  preview: string | null = null;
  selectedFile: File | null = null;
  isDragOver = false;

  constructor(private movieService: MovieService) {}

  openFilePicker(fileInput: HTMLInputElement): void {
    if (this.loading) return;
    fileInput.click();
  }

  onDragOver(event: DragEvent): void {
    event.preventDefault();
    if (this.loading) return;
    this.isDragOver = true;
  }

  onDragLeave(event: DragEvent): void {
    event.preventDefault();
    this.isDragOver = false;
  }

  onDrop(event: DragEvent): void {
    event.preventDefault();
    this.isDragOver = false;
    if (this.loading) return;

    const file = event.dataTransfer?.files?.[0];
    if (!file) return;
    this.handleSelectedFile(file);
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];
    if (!file) return;

    this.handleSelectedFile(file);
  }

  private handleSelectedFile(file: File): void {
    this.selectedFile = null;
    this.preview = null;

    if (file.type && !file.type.toLowerCase().startsWith('image/')) {
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
    if (!this.movieId) {
      this.error = 'Movie not selected';
      return;
    }

    if (!this.selectedFile) {
      this.error = 'Please select an image first';
      return;
    }

    this.loading = true;
    this.error = null;

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
