import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
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
  @ViewChild('searchBox', { static: false }) searchBox: ElementRef;
  @ViewChild('listBox', { static: false }) listBox: ElementRef;

  searchResults$: Observable<SearchResultDTO[]>;
  private searchStrings = new Subject<string>();

  constructor(private searchService: SearchService, private spinner: NgxSpinnerService) {
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

    this.searchResults$.subscribe(_ => {
      this.scrollToTop();
      this.spinner.hide();
    });
  }

  scrollToTop() {
    this.listBox.nativeElement.scrollTop = 0;
  }

  clearQuery() {
    this.searchBox.nativeElement.value = '';
    this.searchStrings.next('');
  }
}
