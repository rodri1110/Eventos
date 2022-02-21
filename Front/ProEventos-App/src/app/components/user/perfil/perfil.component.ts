import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ValidatorField } from '@app/helpers/ValidatorField';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.scss']
})
export class PerfilComponent implements OnInit {

  formUserPerfil!: FormGroup;

  constructor(private fb: FormBuilder) { }

  ngOnInit() {
    this.validation();
  }

  get f(): any{
    return this.formUserPerfil.controls;
  };


  validation(): void {

    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch('senha', 'confirmaSenha')
    }

    this.formUserPerfil = this.fb.group({
      titulo: ['', Validators.required],
      nome: ['', Validators.required],
      sobreNome: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      telefone: ['', Validators.required],
      funcao: ['', Validators.required],
      descricao: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(200)]],
      senha: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(10)]],
      confirmaSenha: ['', Validators.required],
    }, formOptions);
  }

  onSubmit(): void{
    if(this.formUserPerfil.invalid) return;
  }

  public resetForm(event:any): void{
    event.preventDefault();
    this.formUserPerfil.reset();
  }
}
