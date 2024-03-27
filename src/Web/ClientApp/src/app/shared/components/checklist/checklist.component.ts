import { Component, Input, Output, EventEmitter, ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'app-checklist',
  templateUrl: './checklist.component.html',
  styleUrls: ['./checklist.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ChecklistComponent {
  @Input() items: any[];
  @Input() isEditable: boolean = false;

  @Output() stateChange = new EventEmitter<any>();
  @Output() noteChange = new EventEmitter<any>();

  onStateChange(item, newState: boolean): void {
    this.stateChange.emit({ id: item.id, newState });
  }

  onNoteChange(item, event: Event): void {
    var newNote = event.target['value'];
    this.noteChange.emit({ id: item.id, newNote });
  }
}
