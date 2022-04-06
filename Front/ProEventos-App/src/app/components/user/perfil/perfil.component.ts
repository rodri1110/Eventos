import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ValidatorField } from '@app/helpers/ValidatorField';
import { UserUpdate } from '@app/models/identity/UserUpdate';
import { AccountService } from '@app/services/account.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.scss']
})
export class PerfilComponent implements OnInit {

  userUpdate = {} as UserUpdate;

  formUserPerfil!: FormGroup;

  constructor(private fb: FormBuilder,
              public accountService: AccountService,
              private router: Router,
              private toastr: ToastrService,
              private spinner: NgxSpinnerService) { }

  ngOnInit() {
    this.validation();
    this.carregarUsuario();
  }

  carregarUsuario():void{
    this.spinner.show();
    this.accountService.getUser().subscribe({
      next: (userRetorno: UserUpdate) => {
        this.userUpdate = userRetorno;
        this.formUserPerfil.patchValue(this.userUpdate);
        this.toastr.success('Usuário Carregado.', 'Sucesso')
      },
      error: (error) => {
        console.log(error);
        this.toastr.error('Usuário não carregado.', 'Erro');
        this.router.navigate(['/dashboard']);
      },
      complete: () => {
        this.spinner.hide();
      }
    })
  }

  get f(): any{
    return this.formUserPerfil.controls;
  };

  validation(): void {

    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch('password', 'confirmaPassword')
    }

    this.formUserPerfil = this.fb.group({
      userName: [''],
      titulo: ['NaoInformado', Validators.required],
      primeiroNome: ['', Validators.required],
      ultimoNome: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', Validators.required],
      funcao: ['NaoInformado', Validators.required],
      descricao: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(200)]],
      password: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(10)]],
      confirmaPassword: ['', Validators.required],
    }, formOptions);
  }

  onSubmit(): void{
    this.atualizarUsuario();

  }

  public atualizarUsuario(){
    this.userUpdate = { ... this.formUserPerfil.value }
    if(this.userUpdate.password !== null){
      if(this.userUpdate.password !== this.formUserPerfil.value.confirmaPassword)
      {
        this.toastr.error('Senhas devem ser iguais.')
        return console.error();
      }
    }

    this.accountService.updateUser(this.userUpdate).subscribe({
      next:() =>{
        this.toastr.success('Usuário atualizado com sucesso', 'Sucesso')
      },
      error:(error) => {
        this.toastr.error(error.error, 'Error')
      },
      complete:() => {
        this.spinner.hide();
      }
    })
  }

  public resetForm(event:any): void{
    event.preventDefault();
    this.formUserPerfil.reset();
  }
}
