import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Lote } from '@app/models/Lotes';
import { Observable, take } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class LoteService {

constructor(private http: HttpClient) { }

  public getLotesByEventoId(eventoId: number) : Observable<Lote[]>{
    return this.http.get<Lote[]>(`${environment.baseLoteUrl}/${eventoId}`)
    .pipe(take(1))// Uma forma de realizar a chamada sem
    //inscrever-se (subscribe), pode ser utilizada em qualquer método.;
  }

  public saveLotes(eventoId: number, lotes : Lote[]) : Observable<Lote[]>{
    return this.http.put<Lote[]>(`${environment.baseLoteUrl}/${eventoId}`, lotes);
  }

  public deleteLote(eventoId: number, loteId: number) : Observable<any>{
    return this.http.delete(`${environment.baseLoteUrl}/${eventoId}/${loteId}`);
  }

}
