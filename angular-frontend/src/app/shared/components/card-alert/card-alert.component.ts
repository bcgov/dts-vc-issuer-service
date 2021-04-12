import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-card-alert',
  templateUrl: './card-alert.component.html',
  styleUrls: ['./card-alert.component.scss']
})
export class CardAlertComponent {
  @Input() public icon: string;
  @Input() public title: string;
  @Input() public subtitle: string;
  @Input() public alertType: 'info' | 'success' | 'warn' | 'danger';
  @Input() public alertIcon: string;
  @Input() public alertMessage: string;
  @Output() public selected: EventEmitter<void>;

  public constructor() {
    this.icon = '';
    this.title = '';
    this.subtitle = '';
    this.alertType = 'info';
    this.alertIcon = 'info';
    this.alertMessage = '';
    this.selected = new EventEmitter<void>();
  }

  public onAction() {
    this.selected.emit();
  }
}
