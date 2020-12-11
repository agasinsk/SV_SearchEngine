import { AfterContentInit, Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
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
export class SearchEngineComponent implements OnInit {
  @ViewChild('searchBox') searchBox: ElementRef;
  @ViewChild('listBox') listBox: ElementRef;

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
      debounceTime(400),
      distinctUntilChanged(),
      switchMap((searchQuery: string) => this.searchService.search(searchQuery)),
    );
  }

  scrollToTop() {
    this.listBox.nativeElement.scrollTop = 0;
  }

  clearQuery() {
    this.searchBox.nativeElement.value = '';
    this.searchStrings.next('');
    this.scrollToTop();
  }
}
