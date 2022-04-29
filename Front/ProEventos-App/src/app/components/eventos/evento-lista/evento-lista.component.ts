import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { PaginatedResult, Pagination } from '@app/models/Pagination';
import { environment } from '@environments/environment';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { debounceTime, Subject } from 'rxjs';
import { Evento } from 'src/app/models/Evento';
import { EventoService } from 'src/app/services/evento.service';

@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss'],
})
export class EventoListaComponent implements OnInit {
  public eventos: Evento[] = [];
  public mostrar = true;
  public eventoId = 0;
  public tema = '';
  public pagination = {} as Pagination;

  modalRef?: BsModalRef;
  message?: string;
  imagemURL = 'assets/imagens/upLoad.png';

  termoBuscarChanged: Subject<string> = new Subject<string>();

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router
  ) {}


  public filtrarEventos(event: any): void {
    if(this.termoBuscarChanged.observers.length == 0){

      this.termoBuscarChanged
      .pipe(debounceTime(1500)).subscribe(
        (filtrarPor) => {
          this.spinner.show();
          this.eventoService.getEventos(
            this.pagination.currentPage,
            this.pagination.itemsPerPage,
            filtrarPor
          ).subscribe({
            next: (paginatedResponse: PaginatedResult<Evento[]>) => {
              this.eventos = paginatedResponse.result;
              this.pagination = paginatedResponse.pagination;
            },
            error: (error: any) => {
              this.spinner.hide();
              this.toastr.error('Erro ao carregar eventos!', 'Error!');
            },
            complete: () => {
              setTimeout(() => {
                this.spinner.hide();
              }, 200);
            },
          })
        }
      )
    }
    this.termoBuscarChanged.next(event.value);
  }

  ngOnInit() {
    this.pagination = {
      currentPage: 1,
      itemsPerPage: 3,
      totalItems: 1,
    } as Pagination;
    this.obterEventos();
  }

  public mostraImagem(imagemURL: string): string {
    return imagemURL !== ''
      ? `${environment.baseApiURL}resources/images/${imagemURL}`
      : this.imagemURL;
  }

  public obterEventos(): void {
    this.spinner.show();

    this.eventoService.getEventos(this.pagination.currentPage,
                                  this.pagination.itemsPerPage).subscribe({
      next: (paginatedResponse: PaginatedResult<Evento[]>) => {
        this.eventos = paginatedResponse.result;
        this.pagination = paginatedResponse.pagination;
      },
      error: (error: any) => {
        this.spinner.hide();
        this.toastr.error('Erro ao carregar eventos!', 'Error!');
      },
      complete: () => {
        setTimeout(() => {
          this.spinner.hide();
        }, 200);
      },
    });
  }

  openModal(
    event: any,
    template: TemplateRef<any>,
    eventoId: number,
    tema: string
  ): void {
    event.stopPropagation();
    this.tema = tema;
    this.eventoId = eventoId;
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }

  public pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.obterEventos();
  }

  confirmar(): void {
    this.spinner.show();
    this.modalRef?.hide();
    this.eventoService.deleteEvento(this.eventoId).subscribe({
      next: (result: string) => {
        this.toastr.info('Evento deletado com sucesso!', 'Evento deletado');
        this.obterEventos();
      },
      error: (error: any) => {
        console.error(error);
        this.toastr.error('Erro ao deletar evento!', 'Error!');
        this.spinner.hide();
      },
      complete: () => {
        setTimeout(() => {
          this.spinner.hide();
        }, 200);
      },
    });
  }

  cancelar(): void {
    this.modalRef?.hide();
  }

  public exibirEvento(id: number): void {
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
