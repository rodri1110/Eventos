import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Evento } from 'src/app/models/Evento';
import { EventoService } from 'src/app/services/evento.service';

@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss']
})
export class EventoListaComponent implements OnInit {
  public eventosFiltrados: Evento[]=[];
  public eventos: Evento[]=[];
  public mostrar = true;
  public eventoId = 0;
  public tema = "";
  private _filtroLista: string = '';
  modalRef?: BsModalRef;
  message?: string;

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router)
  {
  }

  public get filtroLista(): string{
    return this._filtroLista;
  }

  public set filtroLista(value: string){
    this._filtroLista = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista) : this.eventos;
  }

  public filtrarEventos(filtrarPor: string): Evento[]{
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(
      (evento: {tema: string; local: string;}) =>
      evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1 ||
      evento.local.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    )
  }

  ngOnInit() {
    /** spinner starts on init */
    this.spinner.show();
    this.obterEventos();
  }

  public obterEventos(): void {
    this.eventoService.getEventos().subscribe({
      next: (_eventos: Evento [])=> {
        this.eventos = _eventos;
        this.eventosFiltrados = this.eventos;
      },
      error: (error: any) => {
        this.spinner.hide();
        this.toastr.error('Erro ao carregar eventos!', 'Error!');
      },
      complete: () =>{
        setTimeout(()=>
        {this.spinner.hide()}, 200)
      }
    });
  }

  openModal(event: any, template: TemplateRef<any>, eventoId: number, tema: string): void {
    event.stopPropagation();
    this.tema = tema;
    this.eventoId = eventoId;
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  confirmar(): void {
    this.spinner.show();
    this.modalRef?.hide();
    this.eventoService.deleteEvento(this.eventoId).subscribe({
      next: (result: string) => {
        console.log(result);
        this.toastr.info('Evento deletado com sucesso!', 'Evento deletado!');
        this.spinner.hide();
        this.obterEventos();
      },
      error: (error: any) => {
        console.error(error);
        this.toastr.error('Erro ao carregar eventos!', 'Error!');
        this.spinner.hide();
      },
      complete: () =>{
        setTimeout(()=>
        {this.spinner.hide()}, 200)
      }
    });
  }

  cancelar(): void {
    this.modalRef?.hide();
  }

  public exibirEvento(id: number):void{
    this.router.navigate([`eventos/detalhes/${id}`]);
  }

  // A outras formas de utilizar o subscribe
  // public obterEventos(): void {
  //   this.eventoService.getEventos().subscribe({
  //     next: (_eventos : Evento [])=>{
  //       this.eventos = _eventos;
  //       this.eventosFiltrados = this.eventos;
  //     },
  //     error: (error: any) => console.log(error)
  //   });
  // }

  // A outras formas de utilizar o subscribe
  // public obterEventos(): void {
  //   const observer ={
  //     next: (_eventos : Evento [])=>{
  //       this.eventos = _eventos;
  //       this.eventosFiltrados = this.eventos;
  //     },
  //     error: (error: any) => console.log(error)
  //   };
  //   this.eventoService.getEventos().subscribe(observer);
  // }
}
