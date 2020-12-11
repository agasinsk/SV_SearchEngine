import { AfterContentInit, Component, OnInit } from '@angular/core';
import { Observable, Subject } from 'rxjs';

import {
  debounceTime, distinctUntilChanged, switchMap
} from 'rxjs/operators';

import { SearchResultDTO } from '../search-result';
import { SearchService } from '../search.service';

@Component({
  selector: 'app-search-engine',
  templateUrl: './search-engine.component.html',
  styleUrls: ['./search-engine.component.css']
})
export class SearchEngineComponent implements OnInit, AfterContentInit {
  searchResults$: Observable<SearchResultDTO[]>;
  private searchStrings = new Subject<string>();

  constructor(private searchService: SearchService) {
  }

  // Push a search query into the observable stream.
  search(searchQuery: string): void {
    this.searchStrings.next(searchQuery);
  }

  ngOnInit(): void {
    this.searchResults$ = this.searchStrings.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      switchMap((searchQuery: string) => this.searchService.search(searchQuery)),
    );
  }

  ngAfterContentInit(): void {
    this.searchStrings.next(' ');
  }

  clearQuery() {
    this.searchStrings.next(' ');
  }
}
