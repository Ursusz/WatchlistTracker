import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface FriendRequest {
  friendshipId: number;
  requesterName: string;
  status: string;
}

@Injectable({
  providedIn: 'root'
})
export class FriendshipService {
  private apiUrl = `${environment.apiUrl}/friendships`;

  constructor(private http: HttpClient) {}

  getPendingRequests(): Observable<FriendRequest[]> {
    return this.http.get<FriendRequest[]>(`${this.apiUrl}/pending`);
  }

  getPendingRequestCount(): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/pending/count`);
  }

  sendFriendRequest(receiverId: string): Observable<number> {
    return this.http.post<number>(`${this.apiUrl}/request/${receiverId}`, {});
  }

  acceptFriendRequest(friendshipId: number): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/request/${friendshipId}/accept`, {});
  }

  rejectFriendRequest(friendshipId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/request/${friendshipId}`);
  }
}
