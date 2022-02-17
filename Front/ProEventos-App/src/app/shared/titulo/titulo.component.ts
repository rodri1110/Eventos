import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-titulo',
  templateUrl: './titulo.component.html',
  styleUrls: ['./titulo.component.scss']
})

export class TituloComponent implements OnInit {

  @Input() titulo = 'Eventos';
  @Input() subTitulo = 'Desde 2010';
  @Input() icone = 'fas fa-calendar-alt fa-5x';
  @Input() botaoListar = false;

  constructor(private router: Router) { }

  ngOnInit() {
  }

  public listar(): void{
    this.router.navigate([`/${this.titulo.toLocaleLowerCase()}/lista`]);
  }
}
