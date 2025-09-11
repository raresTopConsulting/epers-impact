import { Component, EventEmitter, forwardRef, Input, Output } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'percentage-nr-input',
  templateUrl: './percentage-nr-input.component.html',
  standalone: false,
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => PercentageNrInputComponent),
    multi: true
  }]
})

export class PercentageNrInputComponent implements ControlValueAccessor {
  @Input() isPercent: boolean = false;
  @Input() classType: string = "form-control"
  @Output() isPercentChange = new EventEmitter<boolean>();
  private internalValue: number = 0;
  displayValue: string = null;

  constructor() { }

  onChange = (value: number) => { };
  onTouched = () => { };

  writeValue(value: number): void {
    this.internalValue = value;
    this.displayValue = this.formatValue(value);
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  onInputChange(input: string): void {
    const trimmed = input.trim();
    let parsedValue: number | null = null;
    let isInputValuePercent = false;

    if (trimmed.endsWith('%')) {
      isInputValuePercent = true;
      const num = parseFloat(trimmed.slice(0, -1));
      if (!isNaN(num)) parsedValue = num / 100;
    } else {
      const num = parseFloat(trimmed);
      if (!isNaN(num)) parsedValue = num;
    }

    if (parsedValue !== null) {
      this.internalValue = parsedValue;
      this.isPercent = isInputValuePercent;
      this.isPercentChange.emit(this.isPercent);
      this.onChange(parsedValue);
    }

    this.displayValue = input;
  }

  private formatValue(value: number | null): string {
    if (!value)
      return '';
    return this.isPercent ? `${value * 100}%` : `${value}`;
  }


}
