import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Lote } from '@app/models/Lotes';
import { Observable, take } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoteService {

  baseUrl = 'https://localhost:5001/api/lote';

constructor(private http: HttpClient) { }

  public getLotesByEventoId(eventoId: number) : Observable<Lote[]>{
    return this.http.get<Lote[]>(`${this.baseUrl}/${eventoId}`)
    .pipe(take(1))// Uma forma de realizar a chamada sem
    //inscrever-se (subscribe), pode ser utilizada em qualquer método.;
  }

  public saveLotes(eventoId: number, lotes : Lote[]) : Observable<Lote[]>{
    return this.http.put<Lote[]>(`${this.baseUrl}/${eventoId}`, lotes);
  }

  public deleteLote(eventoId: number, loteId: number) : Observable<any>{
    return this.http.delete(`${this.baseUrl}/${eventoId}/${loteId}`);
  }

}
