
import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

import { SeatService } from '../../services/seat.service';
import { Seat } from '../../models/seat';

@Component({
  selector: 'app-seat-selection',
  imports: [CommonModule],
  templateUrl: './seat-selection.html',
  styleUrl: './seat-selection.css'
})
export class SeatSelection implements OnInit {

  seats = signal<Seat[]>([]);
  errorMessage = signal('');
  showId = 0;

  constructor(
    private route: ActivatedRoute,
    private seatService: SeatService
  ) { }

  ngOnInit(): void {

    this.showId = Number(this.route.snapshot.paramMap.get('id'));

    if (!this.showId || this.showId <= 0) {
      this.errorMessage.set('Invalid show ID.');
      return;
    }

    this.loadSeats();
  }

  // Load seats for the selected show
  loadSeats(): void {

    this.seatService.getSeatsByShowId(this.showId).subscribe({
      next: (data) => {
        this.seats.set(data);
        console.log('Seats loaded:', data);
      },
      error: (error) => {
        console.error('Error loading seats:', error);
        this.errorMessage.set('Unable to load seats.');
      }
    });
  }
}
