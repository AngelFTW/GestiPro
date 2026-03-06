import type { RelatorioTotaisPessoa, RelatorioTotaisCategoria } from '../types/totais';

const BASE_URL = '/api/totais';

// Retorna receitas, despesas e saldo por pessoa, mais os totais gerais
export async function obterTotaisPorPessoa(): Promise<RelatorioTotaisPessoa> {
  const res = await fetch(`${BASE_URL}/por-pessoa`);
  if (!res.ok) 
    throw new Error('Erro ao obter totais por pessoa');
  return res.json();
}

// Retorna receitas, despesas e saldo por categoria, mais os totais gerais
export async function obterTotaisPorCategoria(): Promise<RelatorioTotaisCategoria> {
  const res = await fetch(`${BASE_URL}/por-categoria`);
  if (!res.ok) 
    throw new Error('Erro ao obter totais por categoria');
  return res.json();
}
