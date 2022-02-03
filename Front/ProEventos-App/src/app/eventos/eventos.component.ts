import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss']
})
export class EventosComponent implements OnInit {
  eventosFiltrados: any =[];
  public eventos: any;
  mostrar = true;
  private _filtroLista: string = '';

  constructor(private http: HttpClient) { }

  public get filtroLista(): string{
    return this._filtroLista;
  }

  public set filtroLista(value: string){
    this._filtroLista = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista) : this.eventos;
  }

  filtrarEventos(filtrarPor: string): any{
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(
      (evento: {tema: string; local: string;}) => evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1 ||
      evento.local.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    )
  }

  ngOnInit() {
    this.getEventos();
  }

  public getEventos(): void {
    this.http.get('https://localhost:5001/api/evento').subscribe(
      response => {
        this.eventosFiltrados = response
        this.eventos = response
      },
      error => console.log(error)
      );
  }
}
