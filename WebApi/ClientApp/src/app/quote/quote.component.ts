import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-quote',
  templateUrl: './quote.component.html',
  styleUrls: ['./quote.component.css']
})
export class QuoteComponent implements OnInit {


  public currencyRates: any[] = [];

  constructor(private http: HttpClient, private spinner: NgxSpinnerService) { }

  currencyRatesLoadHandler() {
    this.spinner.show();
    while (this.currencyRates.length > 0) {
      this.currencyRates.pop();
    }
    this.http.get("/api/exchangeRate").subscribe((data: Array<any>) => {
      
      data.forEach(cr => this.currencyRates.push(cr));
      this.spinner.hide();

    })

  }

  ngOnInit() {
    this.currencyRatesLoadHandler();
  }

}
