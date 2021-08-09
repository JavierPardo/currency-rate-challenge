import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { NgxSpinnerService } from 'ngx-spinner';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';

const purchaseModel: any = {
  userId: "",
  currencyCode: "",
  amount: 0
}
@Component({
  selector: 'app-purchase',
  templateUrl: './purchase.component.html',
  styleUrls: ['./purchase.component.css']
})
export class PurchaseComponent implements OnInit {

  constructor(private ngxSpinnerService: NgxSpinnerService,
    private formBuilder: FormBuilder,
    private modalService: NgbModal,
    private httpClient: HttpClient) { }
  private currencies: any[] = []
  private purchaseForm: FormGroup = null;

  currencyLoadHandler() {
    while (this.currencies.length) {
      this.currencies.pop();
    }
    this.httpClient.get("/api/exchangeRate")
      .subscribe((data: any[]) => {
        data.forEach(rate => {
          this.currencies.push(rate.currencyCode)
        });
      })
  }

  handleError(err: HttpErrorResponse){
    alert(`Error!!!\n${err.error}`);
    this.ngxSpinnerService.hide();
    return throwError(err);
  }

  onSubmit(): void {
    //this.modalService.open("Message")
    this.ngxSpinnerService.show();
    const purchase = {
      userId: this.purchaseForm.value.userId,
      amountInput: parseFloat(this.purchaseForm.value.amount),
      currencyCodeOutput: this.purchaseForm.value.currencyCode
    }

    this.httpClient.post("/api/exchangetransaction", { ...purchase })
    .pipe(catchError(this.handleError.bind(this)))
      .subscribe(data => {
        alert(`Success!!!\n You get ${data.amountOutput} ${data.currencyCodeOutput}`);
        this.ngxSpinnerService.hide();
        this.purchaseForm.reset();
      })
  }

  ngOnInit() {
    this.currencyLoadHandler();
    this.purchaseForm = new FormGroup({
      userId: new FormControl(purchaseModel.userId, [Validators.required, Validators.minLength(1)]),
      currencyCode: new FormControl(purchaseModel.currencyCode, [Validators.required, Validators.minLength(1)]),
      amount: new FormControl(purchaseModel.amount, [Validators.required, Validators.minLength(1)])
    });
  }

}
