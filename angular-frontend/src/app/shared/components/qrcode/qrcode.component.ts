import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-qrcode',
  templateUrl: './qrcode.component.html',
  styleUrls: ['./qrcode.component.scss']
})
export class QRCodeComponent {
  @Input() public base64QRCode: string;
}
