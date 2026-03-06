import type { Categoria, CategoriaRequest } from '../types/categoria';
import type { TipoTransacao } from '../types/transacao';

const BASE_URL = '/api/categorias';

// Retorna todas as categorias cadastradas
export async function listarCategorias(): Promise<Categoria[]> {
  const res = await fetch(BASE_URL);
  if (!res.ok) 
    throw new Error('Erro ao listar categorias');
  return res.json();
}

// Cria uma nova categoria
export async function criarCategoria(dados: CategoriaRequest): Promise<Categoria> {
  const res = await fetch(BASE_URL, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(dados),
  });
  if (!res.ok) 
    throw new Error('Erro ao criar categoria');
  return res.json();
}

// Retorna categorias compatíveis com o tipo de transação informado
// Despesa → categorias com finalidade Despesa ou Ambas
// Receita → categorias com finalidade Receita ou Ambas
export async function listarCategoriasPorTipo(tipo: TipoTransacao): Promise<Categoria[]> {
  const res = await fetch(`${BASE_URL}/por-tipo/${tipo}`);
  if (!res.ok) 
    throw new Error('Erro ao listar categorias por tipo');
  return res.json();
}
