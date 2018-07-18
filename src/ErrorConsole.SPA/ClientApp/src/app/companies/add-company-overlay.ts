import { Injectable, ComponentRef, Injector } from "@angular/core";
import { OverlayRefWrapper } from "../core/overlay-ref-wrapper";
import { PortalInjector, ComponentPortal } from "@angular/cdk/portal";
import { OverlayRefProvider } from "../core/overlay-ref-provider";
import { Observable } from "rxjs";
import { AddCompanyOverlayComponent } from "./add-company-overlay.component";

@Injectable()
export class AddCompanyOverlay {
  constructor(
    public _injector: Injector,
    public _overlayRefProvider: OverlayRefProvider
  ) { }

  public create(options: { companyId?: number } = {}): Observable<any> {
    const overlayRef = this._overlayRefProvider.create();
    const overlayRefWrapper = new OverlayRefWrapper(overlayRef);
    const overlayComponent = this.attachOverlayContainer(overlayRef, overlayRefWrapper);
    overlayComponent.companyId = options.companyId;
    return overlayRefWrapper.afterClosed();
  }

  public attachOverlayContainer(overlayRef, overlayRefWrapper) {
    const injectionTokens = new WeakMap();
    injectionTokens.set(OverlayRefWrapper, overlayRefWrapper);
    const injector = new PortalInjector(this._injector, injectionTokens);
    const overlayPortal = new ComponentPortal(AddCompanyOverlayComponent, null, injector);
    const overlayPortalRef: ComponentRef<AddCompanyOverlayComponent> = overlayRef.attach(overlayPortal);
    return overlayPortalRef.instance;
  }
}
