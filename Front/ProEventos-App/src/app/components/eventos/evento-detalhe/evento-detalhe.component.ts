import { ToastrService } from 'ngx-toastr';
import { Evento } from '@app/models/Evento';
import { NgxSpinnerService } from 'ngx-spinner';
import { ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { EventoService } from '@app/services/evento.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {

  evento = {} as Evento;
  formEventoDetalhe!: FormGroup;
  statusSalvar : any = 'post';

  constructor(private fb: FormBuilder,
    private localeService: BsLocaleService,
    private route: ActivatedRoute,
    private eventoService: EventoService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService)
    {
      this.localeService.use('pt-br');
    }

    public carregarEvento(): void{
      const eventoIdParam = this.route.snapshot.paramMap.get('id');

      if(eventoIdParam !== null){
        this.spinner.show();

        this.statusSalvar = 'put';
        this.eventoService.getEventoById(+eventoIdParam).subscribe({

          next:(evento: Evento) => {
            this.evento = {... evento};
            this.formEventoDetalhe.patchValue(this.evento);
          },
          error:(error: any) => {
            this.toastr.error('Erro ao tentar carregar evento', 'Erro'),
            console.error(error)
          },
          complete:() => {
            this.spinner.hide(),
            this.toastr.success('Evento carregado com sucesso', 'Sucesso')
          },
        })
      }
    }

    ngOnInit() {
      this.carregarEvento();
      this.validation();
    }

  get f(): any{
    return this.formEventoDetalhe.controls;
  }

  get bsConfig(): any{
    return { isAnimated: true,
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY - HH:mm:ss',
      showWeekNumbers: false,
      withTimepicker: true,
      containerClass: 'theme-dark-blue',
      showClearButton: true

    }
  }

  public salvarAlteracao(): void{
    this.spinner.show();

    if(this.formEventoDetalhe.valid){

      this.evento = this.statusSalvar === 'post' ? {... this.formEventoDetalhe.value} :
        {eventoId: this.evento.eventoId, ... this.formEventoDetalhe.value};

        this.eventoService[this.statusSalvar](this.evento).subscribe(
          () => this.toastr.success('Evento salvo com sucesso!', 'Salvo'),
          (error: any) => {
            console.error(error),
            this.toastr.error('Erro ao salvar o evento.','Erro')
          }
        ).add(this.spinner.hide());
    }
  }

  public validation(): void{
    this.formEventoDetalhe = this.fb.group({
  tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],

  local:['',Validators.required],
  dataEvento:['', Validators.required],
  qtdPessoas:['', [Validators.required, Validators.max(500)]],
  imagemURL:['', Validators.required],
  telefone:['', Validators.required],
  email:['', [Validators.required, Validators.email]]
    });
  }

  limparCampos(): void{
    this.formEventoDetalhe.reset();
  }

}
