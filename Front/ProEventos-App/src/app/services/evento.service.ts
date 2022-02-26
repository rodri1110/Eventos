import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, take } from 'rxjs';
import { Evento } from '../models/Evento';

@Injectable(/*{
  providedIn: 'root'
}*/)
export class EventoService {

  baseUrl = 'https://localhost:5001/api/evento';

constructor(private http: HttpClient) { }

  public getEventos() : Observable<Evento[]>{
    return this.http.get<Evento[]>(this.baseUrl);
  }

  public getEventosByTema(tema: string) : Observable<Evento[]>{
    return this.http.get<Evento[]>(`${this.baseUrl}/${tema}/tema`);
  }

  public getEventoById(EventoId: number) : Observable<Evento>{
    return this.http.get<Evento>(`${this.baseUrl}/${EventoId}`)
    .pipe(take(1)); // Uma forma de realizar a chamada sem inscrever-se (subscribe)
  }

  public post(evento: Evento) : Observable<Evento>{
    return this.http.post<Evento>(this.baseUrl, evento);
  }

  public put(evento: Evento) : Observable<Evento>{
    return this.http.put<Evento>(`${this.baseUrl}/${evento.eventoId}`, evento);
  }

  public deleteEvento(eventoId: number) : Observable<any>{
    return this.http.delete(`${this.baseUrl}/${eventoId}`);
  }
}
