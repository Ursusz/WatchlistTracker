import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Review } from '../../services/review.service';
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

  comments: Comment[] = [];
  showComments = false;
  newComment = '';
  loadingComments = false;
  submittingComment = false;

  constructor(
    private commentService: CommentService,
    private authService: AuthService
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
    this.loadingComments = true;
    this.commentService.getCommentsByReview(this.review.id).subscribe({
      next: (comments) => {
        this.comments = comments;
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
    return userId === comment.authorId;
  }

  getRating(rating: number): string {
    return '⭐'.repeat(rating);
  }
}
