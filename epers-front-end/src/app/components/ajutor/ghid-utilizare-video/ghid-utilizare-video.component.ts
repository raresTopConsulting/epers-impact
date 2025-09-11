import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'app-ghid-utilizare-video',
    templateUrl: './ghid-utilizare-video.component.html',
    styleUrls: ["./ghid-utilizare-video.component.scss"],
    standalone: false
})
export class GhidUtilizareVideoComponent implements OnInit {

  videos: {
    denumire: string,
    downloadLink: string,
    link: string
  }[] = [];

  constructor() { }

  ngOnInit() {
    this.videos = [
      {
        downloadLink: "Epers Evaluare Competente si Obiective.mov",
        link: "assets/documents/Epers Evaluare Competente si Obiective.mov",
        denumire: "Tutorial Evaluare Competențe și Setare si evaluarea Obiectivelor"
      },
      {
        downloadLink: "Epers_Admin_Firme_Nomenclatoare_Useri.mov",
        link: "assets/documents/Epers_Admin_Firme_Nomenclatoare_Useri.mov",
        denumire: "Tutorial Administrator și Resurse Umane - Setarea Nomenclatoarelor și a Utilizatoriilor"
      },
      {
        downloadLink: "PIP.mov",
        link: "assets/documents/PIP.mov",
        denumire: "Tutorial Plan de Îmbunătățire a performanțelor"
      },
      {
        downloadLink: "Concluzii.mov",
        link: "assets/documents/Concluzii.mov",
        denumire: "Tutorial Concluzii Evaluare Cantitativă, Calitativă și Obiective"
      }
    ]
  }

  playVideo(video: HTMLVideoElement) {
    video.play();
  }

  pauseVideo(video: HTMLVideoElement) {
    video.pause();
  }

}
