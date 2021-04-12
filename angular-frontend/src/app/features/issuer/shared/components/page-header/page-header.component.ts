import { Component, ContentChildren, Input, QueryList } from '@angular/core';

import { PageHeaderSummaryDirective } from './page-header-summary.directive';

@Component({
  selector: 'app-page-header',
  templateUrl: './page-header.component.html',
  styleUrls: ['./page-header.component.scss']
})
export class PageHeaderComponent {
  @Input() public title: string;

  @ContentChildren(PageHeaderSummaryDirective, { descendants: true })
  public pageHeaderSummary: QueryList<PageHeaderSummaryDirective> | null;

  // @ContentChildren(ContextualTitleDirective, { descendants: true })
  // public contextualTitleChildren: QueryList<ContextualTitleDirective>;

  public constructor() {
    this.title = '';
    this.pageHeaderSummary = null;
  }

  public get hasPageHeaderSummary(): boolean {
    return !!this.pageHeaderSummary?.length;
  }
}
