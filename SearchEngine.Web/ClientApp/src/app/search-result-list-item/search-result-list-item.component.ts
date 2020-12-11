import { Component, Input } from '@angular/core';
import { SearchResultDTO } from '../search-result';

@Component({
  selector: 'app-search-result-list-item',
  templateUrl: './search-result-list-item.component.html',
  styleUrls: ['./search-result-list-item.component.css']
})
export class SearchResultListItemComponent {
  @Input() item: SearchResultDTO;
}
