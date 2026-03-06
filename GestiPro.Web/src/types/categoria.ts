// Enum da finalidade categoria para o backend
export enum CategoriaFinalidade {
  Despesa = 0,
  Receita = 1,
  Ambas = 2,
}

export const CategoriaFinalidadeLabel: Record<CategoriaFinalidade, string> = {
  [CategoriaFinalidade.Despesa]: 'Despesa',
  [CategoriaFinalidade.Receita]: 'Receita',
  [CategoriaFinalidade.Ambas]: 'Ambas',
};

// Categoria retornada pela API
export interface Categoria {
  id: number;
  descricao: string;
  finalidade: CategoriaFinalidade;
  finalidadeDescricao: string;
}

// Dados enviados para criar uma categoria
export interface CategoriaRequest {
  descricao: string;
  finalidade: CategoriaFinalidade;
}
