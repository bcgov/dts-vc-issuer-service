import { Injectable } from '@angular/core';

function getWindow(): any {
  return window;
}

@Injectable({
  providedIn: 'root'
})
export class WindowRefService {
  /**
   * @description
   * Get a reference to the native window object.
   */
  public get nativeWindow(): Window {
    return getWindow();
  }
}
