import { Component, Input } from "@angular/core";
import { Subject, BehaviorSubject } from "rxjs";
import { tap } from "rxjs/operators";

@Component({
  templateUrl: "./error-console.component.html",
  styleUrls: ["./error-console.component.css"],
  selector: "app-error-console"
})
export class ErrorConsoleComponent { 

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnInit() {
    this.errors$.asObservable()
      .pipe(tap(x => this.isOpen = true))
      .subscribe();
  }
  @Input()
  public errors$: BehaviorSubject<any[]> = new BehaviorSubject([]);

  ngOnDestroy() {
    this.onDestroy.next();	
  }

  public isOpen: boolean;
}
