// Pessoa retornada pela API
export interface Pessoa {
  id: number;
  nome: string;
  idade: number;
  menorIdade: boolean;
}

// Dados enviados para criar ou editar uma pessoa
export interface PessoaRequest {
  nome: string;
  idade: number;
}
