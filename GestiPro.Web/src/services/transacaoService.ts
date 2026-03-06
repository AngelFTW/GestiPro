import type { Transacao, TransacaoRequest } from '../types/transacao';

const BASE_URL = '/api/transacoes';

// Retorna todas as transações com dados de pessoa e categoria
export async function listarTransacoes(): Promise<Transacao[]> {
  const res = await fetch(BASE_URL);
  if (!res.ok) 
    throw new Error('Erro ao listar transações');
  return res.json();
}

// Cria uma nova transação respeitando as regras de negócio do backend.
// Lança erro com a mensagem retornada pela API em caso de violação de regra.
export async function criarTransacao(dados: TransacaoRequest): Promise<Transacao> {
  const res = await fetch(BASE_URL, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(dados),
  });
  if (!res.ok) {
    const corpo = await res.json().catch(() => null);
    throw new Error(corpo?.mensagem ?? 'Erro ao criar transação');
  }
  return res.json();
}
