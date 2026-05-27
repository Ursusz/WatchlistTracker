import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { Review, ReviewService } from '../../services/review.service';
import { Comment, CommentService } from '../../services/comment.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-review-card',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './review-card.component.html',
  styleUrl: './review-card.component.css'
})
export class ReviewCardComponent implements OnInit {
  @Input() review!: Review;
  @Output() reviewDeleted = new EventEmitter<number>();

  comments: Comment[] = [];
  showComments = false;
  newComment = '';
  loadingComments = false;
  submittingComment = false;
  deletingReview = false;

  constructor(
    private commentService: CommentService,
    private reviewService: ReviewService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
  }

  toggleComments() {
    if (!this.showComments) {
      this.loadComments();
    }
    this.showComments = !this.showComments;
  }

  loadComments() {
    if (!this.review?.id) {
      return;
    }

    this.loadingComments = true;
    this.commentService.getCommentsByReview(this.review.id).subscribe({
      next: (comments) => {
        this.comments = comments;
        this.review.commentCount = comments.length;
        this.loadingComments = false;
      },
      error: (err) => {
        console.error('Error loading comments:', err);
        this.loadingComments = false;
      }
    });
  }

  submitComment() {
    if (!this.newComment.trim() || this.submittingComment) return;

    this.submittingComment = true;
    this.commentService.createComment({
      reviewId: this.review.id,
      text: this.newComment
    }).subscribe({
      next: () => {
        this.newComment = '';
        this.submittingComment = false;
        this.loadComments();
      },
      error: (err) => {
        console.error('Error submitting comment:', err);
        this.submittingComment = false;
      }
    });
  }

  deleteComment(commentId: number) {
    if (confirm('Are you sure you want to delete this comment?')) {
      this.commentService.deleteComment(commentId).subscribe({
        next: () => {
          this.loadComments();
        },
        error: (err) => {
          console.error('Error deleting comment:', err);
        }
      });
    }
  }

  canDeleteComment(comment: Comment): boolean {
    const userId = this.authService.getUserId();
    return userId === comment.authorId || this.authService.isAdmin();
  }

  canDeleteReview(): boolean {
    const userId = this.authService.getUserId();
    return userId === this.review.authorId || this.authService.isAdmin();
  }

  deleteReview(): void {
    if (!this.canDeleteReview() || this.deletingReview) return;
    if (!confirm('Delete this review?')) return;

    this.deletingReview = true;
    this.reviewService.deleteReview(this.review.id).subscribe({
      next: () => {
        this.deletingReview = false;
        this.reviewDeleted.emit(this.review.id);
      },
      error: (err) => {
        console.error('Error deleting review:', err);
        this.deletingReview = false;
      }
    });
  }

  getRating(rating: number): string {
    return '⭐'.repeat(rating);
  }

  goToAuthorProfile() {
    this.router.navigate(['/profile', this.review.authorId]);
  }
}
