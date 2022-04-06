import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Evento } from '../models/Evento';

@Injectable(/*{
  providedIn: 'root'
} Há duas formas de Injectable, esta que é feita no app.module
que centraliza todos os Injectables ou deixar na própria classe
como vem por padrão*/)
export class EventoService {

 tokenHeader = new HttpHeaders({'Authorization': `Bearer ${JSON.parse(localStorage.getItem('user')).token}`});

constructor(private http: HttpClient) { }

  public getEventos() : Observable<Evento[]>{
    return this.http.get<Evento[]>(environment.eventoBaseUrl);
  }

  public getEventosByTema(tema: string) : Observable<Evento[]>{
    return this.http.get<Evento[]>(`${environment.eventoBaseUrl}/${tema}/tema`);
  }

  public getEventoById(EventoId: number) : Observable<Evento>{
    return this.http.get<Evento>(`${environment.eventoBaseUrl}/${EventoId}`);
    //.pipe(take(1)); // Uma forma de realizar a chamada sem
    //inscrever-se (subscribe), pode ser utilizada em qualquer método.
  }

  public post(evento: Evento) : Observable<Evento>{
    return this.http.post<Evento>(environment.eventoBaseUrl, evento);
  }

  public put(evento: Evento) : Observable<Evento>{
    return this.http.put<Evento>(`${environment.eventoBaseUrl}/${evento.eventoId}`, evento);
  }

  public deleteEvento(eventoId: number) : Observable<any>{
    return this.http.delete(`${environment.eventoBaseUrl}/${eventoId}`);
  }

  public postUpLoad(eventoId: number, file: File): Observable<Evento>{
    const fileToUpload = file[0] as File;
    const formData = new FormData();
    formData.append('file', fileToUpload);

    return this.http.post<Evento>(`${environment.eventoBaseUrl}/upload-image/${eventoId}`, formData).pipe(take(1));
  }
}
