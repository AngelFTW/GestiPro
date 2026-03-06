import type { Pessoa, PessoaRequest } from '../types/pessoa';

const BASE_URL = '/api/pessoa';

// Lista todas as pessoas cadastradas
export async function listarPessoas(): Promise<Pessoa[]> {
  const res = await fetch(BASE_URL);
  if (!res.ok) 
    throw new Error('Erro ao listar pessoas');
  return res.json();
}

// Cria uma nova pessoa
export async function criarPessoa(dados: PessoaRequest): Promise<Pessoa> {
  const res = await fetch(BASE_URL, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(dados),
  });
  if (!res.ok) 
    throw new Error('Erro ao criar pessoa');
  return res.json();
}

// Atualiza os dados de uma pessoa existente
export async function atualizarPessoa(id: number, dados: PessoaRequest): Promise<void> {
  const res = await fetch(`${BASE_URL}/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(dados),
  });
  if (!res.ok) 
    throw new Error('Erro ao atualizar pessoa');
}

// Deleta uma pessoa e todas as suas transações
export async function deletarPessoa(id: number): Promise<void> {
  const res = await fetch(`${BASE_URL}/${id}`, { method: 'DELETE' });
  if (!res.ok) 
    throw new Error('Erro ao deletar pessoa');
}
