import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {

  formEventoDetalhe!: FormGroup;

  constructor(private fb: FormBuilder) { }

    ngOnInit() {
  this.validation();
  }

  get f(): any{
    return this.formEventoDetalhe.controls;
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
