import { RedeSocial } from "./RedeSocial";

export interface User {
  userId: number;
  nome: string;
  sobreNome: string;
  email: string;
  usuario: string;
  senha: string;
  redesSociais: RedeSocial[];
}
