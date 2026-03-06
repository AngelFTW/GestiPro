// Enum dos tipos de transacao pro backend
export enum TipoTransacao {
  Despesa = 0,
  Receita = 1,
}

export const TipoTransacaoLabel: Record<TipoTransacao, string> = {
  [TipoTransacao.Despesa]: 'Despesa',
  [TipoTransacao.Receita]: 'Receita',
};

// Retorno transacao
export interface Transacao {
  id: number;
  descricao: string;
  valor: number;
  tipo: TipoTransacao;
  tipoDescricao: string;
  idCategoria: number;
  categoriaDescricao: string;
  idPessoa: number;
  pessoaNome: string;
}

// Dados enviados para a api da transacao
export interface TransacaoRequest {
  descricao: string;
  valor: number;
  tipo: TipoTransacao;
  idCategoria: number;
  idPessoa: number;
}
