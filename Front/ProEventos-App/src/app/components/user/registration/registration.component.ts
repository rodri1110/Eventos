import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ValidatorField } from '@app/helpers/ValidatorField';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent implements OnInit {

  formUserRegistration!: FormGroup;

  constructor(private fb: FormBuilder) { }

  ngOnInit() {
    this.validation();
  }

  get f(): any{
    return this.formUserRegistration.controls;
  };

  public validation(): void{

    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch('senha', 'confirmaSenha')
    }

    this.formUserRegistration = this.fb.group({
      nome: ['', Validators.required],
      sobreNome: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      usuario: ['', Validators.required],
      senha: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(10)]],
      confirmaSenha: ['', Validators.required],
    }, formOptions);
  }

}
