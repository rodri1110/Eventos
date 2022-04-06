import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ValidatorField } from '@app/helpers/ValidatorField';
import { User } from '@app/models/identity/User';
import { AccountService } from '@app/services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent implements OnInit {

  user = {} as User;

  formUserRegistration!: FormGroup;

  constructor(private fb: FormBuilder,
              private accountService: AccountService,
              private router: Router,
              private toastr: ToastrService) { }

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

  public register():void{
    this.user = { ...this.formUserRegistration.value};
    this.accountService.register(this.user).subscribe({
      next:() => this.router.navigateByUrl('/dashboard'),
      error:(error: any) => this.toastr.error('Erro ao registrar usuário', 'Error')
    })
  }
}
