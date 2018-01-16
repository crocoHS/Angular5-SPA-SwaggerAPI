import { Component } from '@angular/core';
import { GlobalService } from '../../services/global.service';
import { PlainText } from './../../models/plain-text.type';

@Component({
    selector: 'crypto',
    templateUrl: './crypto.component.html'
})

export class CryptoComponent {
    public plainText: PlainText = new PlainText();
    public encrypted: PlainText = new PlainText();
    public original: string = '';

    constructor(private service: GlobalService) { }

    public encryptText() {
        this.service.encryptText(this.plainText)
            .subscribe(result => {
                this.encrypted = result;
            });
    }

    public decryptText() {
        this.service.decryptText(this.encrypted)
            .subscribe(result => {
                this.original = result.text;
            });
    }
}