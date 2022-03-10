import { ToastrService } from 'ngx-toastr';
import { Evento } from '@app/models/Evento';
import { NgxSpinnerService } from 'ngx-spinner';
import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { EventoService } from '@app/services/evento.service';
import { LoteService } from '@app/services/lote.service';
import {
  AbstractControl,
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Lote } from '@app/models/Lotes';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss'],
})
export class EventoDetalheComponent implements OnInit {
  eventoId: number;
  evento = {} as Evento;
  lote = {} as Lote;
  form!: FormGroup;
  statusSalvar: string = 'post';
  modalRef?: BsModalRef;
  loteAtual: any = { id: 0, nome: '', indice: 0 };
  imagemURL: string = 'assets/imagens/upLoad.png';
  file: File;

  constructor(
    private modalService: BsModalService,
    private fb: FormBuilder,
    private localeService: BsLocaleService,
    private activatedRoute: ActivatedRoute,
    private eventoService: EventoService,
    private loteService: LoteService,
    private spinner: NgxSpinnerService,
    private router: Router,
    private toastr: ToastrService
  ) {
    this.localeService.use('pt-br');
  }

  ngOnInit() {
    this.carregarEvento();
    this.validation();
  }

  public exibirLote(): boolean {
    if (this.statusSalvar === 'put') {
      const showLotes = true;
      return showLotes;
    }
    return false;
  }

  public carregarEvento(): void {
    this.eventoId = +this.activatedRoute.snapshot.paramMap.get('id');
    if (this.eventoId !== null && this.eventoId !== 0) {
      this.spinner.show();

      this.statusSalvar = 'put';

      this.eventoService.getEventoById(this.eventoId).subscribe({
        next: (eventoRetorno: Evento) => {
          this.evento = { ...eventoRetorno };
          this.form.patchValue(this.evento);
          this.evento.lotes.forEach(
            (
              lote // esta chamada substitui a chamada do carregarLotes()
            ) => this.lotes.push(this.criarLotes(lote))
          );
          //this.carregarLotes()
        },
        error: (error: any) => {
          this.toastr.error('Erro ao tentar carregar evento', 'Erro'),
            console.error(error);
        },
        complete: () => {
          this.spinner.hide(),
            this.toastr.success('Evento carregado com sucesso', 'Sucesso');
        },
      });
    }
  }

  get lotes(): FormArray {
    return this.form.get('lotes') as FormArray;
  }

  get f(): any {
    return this.form.controls;
  }

  get bsConfig(): any {
    return {
      isAnimated: true,
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY - HH:mm:ss',
      showWeekNumbers: false,
      withTimepicker: true,
      containerClass: 'theme-dark-blue',
      showClearButton: true,
    };
  }

  public salvarEvento(): void {
    if (this.form.valid) {
      this.spinner.show();
      this.evento =
        this.statusSalvar === 'post'
          ? { ...this.form.value }
          : { eventoId: this.evento.eventoId, ...this.form.value };

      this.eventoService[this.statusSalvar](this.evento)
        .subscribe(
          (eventoRetorno: Evento) => {
            this.toastr.success('Evento salvo com sucesso!', 'Salvo'),
              this.router.navigate([
                `eventos/detalhes/${eventoRetorno.eventoId}`,
              ]);
          },
          (error: any) => {
            console.error(error),
              this.toastr.error('Erro ao salvar o evento.', 'Erro');
          }
        )
        .add(this.spinner.hide());
    }
  }

  public validation(): void {
    this.form = this.fb.group({
      tema: [
        '',
        [
          Validators.required,
          Validators.minLength(4),
          Validators.maxLength(50),
        ],
      ],
      local: ['', Validators.required],
      dataEvento: ['', Validators.required],
      qtdPessoas: ['', [Validators.required, Validators.max(500)]],
      imagemURL: ['', Validators.required],
      telefone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      lotes: this.fb.array([]),
    });
  }

  adicionarLote(): void {
    this.lotes.push(this.criarLotes({ id: 0 } as Lote));
  }

  criarLotes(lote: Lote): FormGroup {
    return this.fb.group({
      id: [lote.id],
      nome: [lote.nome, Validators.required],
      quantidade: [
        lote.quantidade,
        [Validators.required, Validators.min(99), Validators.max(500)],
      ],
      preco: [lote.preco, Validators.required],
      dataInicio: [lote.dataInicio, Validators.required],
      dataFim: [lote.dataFim, Validators.required],
    });
  }

  // carregarLotes(): void{
  //   this.loteService.getLotesByEventoId(this.eventoId).subscribe({
  //     next: (loteRetorno: Lote[]) => {
  //       loteRetorno.forEach( lote => {
  //         this.lotes.push(this.criarLotes(lote));
  //       })
  //     },
  //     error: (error: any) => {
  //       this.toastr.error('Erro ao carregar lote.','Erro'),
  //       console.error(error)
  //     }
  //   }).add(() => this.spinner.hide());
  // }

  limparCampos(): void {
    this.form.reset();
  }

  public cssValidator(campoForm: FormControl | AbstractControl | any): any {
    return { 'is-invalid': campoForm.errors && campoForm.touched };
  }

  public salvarLote(): void {
    this.spinner.show();

    if (this.form.controls['lotes'].valid) {
      this.loteService
        .saveLotes(this.eventoId, this.form.value.lotes)
        .subscribe({
          next: () => {
            this.toastr.success('Lote salvo com sucesso!', 'Sucesso');
          },
          error: (error: any) => {
            this.toastr.error('Erro ao salvar o lote.', 'Erro'),
              console.error(error);
          },
        })
        .add(() => this.spinner.hide());
    }
  }

  public removerLote(template: TemplateRef<any>, indice: number): void {
    this.loteAtual.id = this.lotes.get(indice + '.id').value;
    this.loteAtual.nome = this.lotes.get(indice + '.nome').value;
    this.loteAtual.indice = indice;
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }

  confirmeDelete(): void {
    this.modalRef.hide();
    this.spinner.show();

    this.loteService
      .deleteLote(this.eventoId, this.loteAtual.id)
      .subscribe({
        next: (result: string) => {
          this.toastr.success(
            'Evento deletado com sucesso!',
            'Evento deletado!'
          );
          this.lotes.removeAt(this.loteAtual.indice);
        },
        error: (error: any) => {
          console.error(error);
          this.toastr.error('Erro ao carregar eventos!', 'Error!');
        },
      })
      .add(() => this.spinner.hide());
  }

  cancelar(): void {
    this.modalRef?.hide();
  }

  retornaNomeLote(nome: string): string {
    return (nome = nome === null || nome === '' ? 'Nome do Lote' : nome);
  }

  onFileChange(ev: any): void {
    const reader = new FileReader();
    this.file = ev.target.files;

    reader.onload = (event: any) => this.imagemURL = event.target.result;

    reader.readAsDataURL(this.file[0]);
  }
}
