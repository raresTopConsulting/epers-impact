import { Component, ElementRef, Input, OnChanges, OnDestroy, ViewChild } from '@angular/core';
import { Chart, registerables } from 'chart.js';
import { EvaluareCreateModel } from 'src/app/models/evaluare/Evaluare';

Chart.register(...registerables);
@Component({
    selector: 'chart-istoric-evaluari',
    templateUrl: './chart-istoric-evaluari.component.html',
    standalone: false
})
export class ChartIstoricEvaluariComponent implements OnChanges, OnDestroy {
  @Input() chartData: EvaluareCreateModel[];
  @Input() chartName: string;
  @ViewChild('chartCanvas') chartCanvas: ElementRef;
  skills: string[];
  valFin: number[];
  ideal: number[];
  val: number[];
  valIndiv: number[];
  displayChart: boolean = false;
  istoricChart: any;

  constructor() { }

  ngOnChanges(): void {
    if (this.istoricChart) {
      this.istoricChart.destroy();
    }
    
    this.skills = this.chartData.map((x => x.denumireSkill));
    this.val = this.chartData.map((x => x.val));
    this.valIndiv = this.chartData.map((x => x.valIndiv));
    this.valFin = this.chartData.map((x => x.valFin));
    this.ideal = this.chartData.map((x => x.ideal));

    this.displayChart = !!(this.val || this.valIndiv);

    if (this.displayChart) {
      this.istoricChart = new Chart("istoricEvalChart", {
        type: 'line',
        data: {
          labels: this.skills,
          datasets: [{
            label: 'Evaluare Finală',
            data: this.valFin,
            fill: false,
            borderColor: 'rgb(75, 192, 192)',
            tension: 0.1
          },
          {
            label: 'Ideal',
            data: this.ideal,
            fill: false,
            borderColor: 'rgb(34,139,34)',
            tension: 0.1
          },
          {
            label: 'Evaluare Sef',
            data: this.val,
            fill: false,
            borderColor: 'rgb(25, 102, 216)',
            tension: 0.1
          },
          {
            label: 'AutoEvaluare',
            data: this.valIndiv,
            fill: false,
            borderColor: 'rgb(211,218,17)',
            tension: 0.1
          },
          ]
        },
        options: {
          scales: {
            y: {
              beginAtZero: true
            },
            x: {
              beginAtZero: true
            }
          }
        }
      });
      
      // Code to add chart to the PDF
      // if(data) {
      //   setTimeout(() => {
      //   this.chartIstEvalService.setChart(this.chartCanvas.nativeElement);
      //   }, 500);
      // }
    }
  }

  ngOnDestroy(): void {
      this.istoricChart.destroy();
  }

}
