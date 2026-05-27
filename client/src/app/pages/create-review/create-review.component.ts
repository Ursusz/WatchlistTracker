import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { Movie, MovieService } from '../../services/movie.service';
import { ReviewService } from '../../services/review.service';

@Component({
  selector: 'app-create-review',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, NavbarComponent],
  templateUrl: './create-review.component.html',
  styleUrl: './create-review.component.css'
})
export class CreateReviewComponent implements OnInit {
  form: FormGroup;
  movies: Movie[] = [];
  loading = true;
  submitting = false;
  error: string | null = null;
  submitted = false;

  constructor(
    private formBuilder: FormBuilder,
    private movieService: MovieService,
    private reviewService: ReviewService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.form = this.formBuilder.group({
      movieId: ['', Validators.required],
      rating: [5, [Validators.required, Validators.min(1), Validators.max(10)]],
      content: ['', [Validators.required, Validators.minLength(10)]],
      visibility: [0, Validators.required]
    });
  }

  get f() {
    return this.form.controls;
  }

  ngOnInit() {
    this.movieService.getMovies().subscribe({
      next: (movies) => {
        this.movies = movies;
        this.loading = false;

        const movieId = this.route.snapshot.queryParamMap.get('movieId');
        if (movieId) {
          this.form.patchValue({ movieId: +movieId });
        }
      },
      error: () => {
        this.error = 'Failed to load movies';
        this.loading = false;
      }
    });
  }

  onSubmit() {
    this.submitted = true;
    if (this.form.invalid) return;

    this.submitting = true;
    this.error = null;

    this.reviewService.createReview({
      movieId: +this.form.value.movieId,
      rating: +this.form.value.rating,
      content: this.form.value.content,
      visibility: +this.form.value.visibility
    }).subscribe({
      next: () => {
        this.router.navigate(['/my-reviews']);
      },
      error: (err) => {
        this.error = err.error?.message || 'Failed to create review';
        this.submitting = false;
      }
    });
  }

  goBack() {
    this.router.navigate(['/feed']);
  }
}
