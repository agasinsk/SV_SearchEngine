import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { SearchResultDTO } from './search-result';

@Injectable({ providedIn: 'root' })
export class SearchService {

  private apiUrl: string;

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') baseUrl: string) {
    this.apiUrl = `${baseUrl}search`;
  }

  search(searchQuery: string): Observable<SearchResultDTO[]> {
    // There's very little point in searching for thing when less than 2 charactes are provided
    if (!searchQuery.trim() || searchQuery.length < 2) {
      return of([]);
    }

    return this.http.get<SearchResultDTO[]>(`${this.apiUrl}?query=${searchQuery}`).pipe(
      catchError(this.handleError<SearchResultDTO[]>('search', []))
    );
  }

  /**
   * Handle failed request.
   * @param operation - name of the operation that failed
   * @param result - optional value to return as the observable result
   */
  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(`${operation}: ${error}`);
      return of(result as T);
    };
  }
}
