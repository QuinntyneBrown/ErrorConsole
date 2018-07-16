import { Component, Input, EventEmitter, Output } from "@angular/core";
import { Subject, BehaviorSubject } from "rxjs";
import { tap } from "rxjs/operators";

@Component({
  templateUrl: "./error-console.component.html",
  styleUrls: ["./error-console.component.css"],
  selector: "app-error-console"
})
export class ErrorConsoleComponent { 

  public onDestroy: Subject<void> = new Subject<void>();

  @Input()
  public errors$: BehaviorSubject<any[]> = new BehaviorSubject([]);

  ngOnDestroy() {
    this.onDestroy.next();	
  }

  @Output()
  public close: EventEmitter<any> = new EventEmitter();

  @Output()
  public clear: EventEmitter<any> = new EventEmitter();
}
