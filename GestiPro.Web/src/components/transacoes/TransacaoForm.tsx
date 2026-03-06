import { useEffect, useState } from 'react';
import { TipoTransacao, TipoTransacaoLabel, type TransacaoRequest } from '../../types/transacao';
import type { Categoria } from '../../types/categoria';
import type { Pessoa } from '../../types/pessoa';
import { listarPessoas } from '../../services/pessoaService';
import { listarCategoriasPorTipo } from '../../services/categoriaService';

interface Props {
  onSalvar: (dados: TransacaoRequest) => void;
  onCancelar: () => void;
  erroExterno?: string; // erro de regra de negócio vindo do backend
}

// Formulário de criação de transação. Respeitando as regras de negocio no backend
export default function TransacaoForm({ onSalvar, onCancelar, erroExterno }: Props) {
  const [descricao, setDescricao] = useState('');
  const [valor, setValor] = useState('');
  const [tipo, setTipo] = useState<TipoTransacao>(TipoTransacao.Despesa);
  const [idPessoa, setIdPessoa] = useState('');
  const [idCategoria, setIdCategoria] = useState('');

  const [pessoas, setPessoas] = useState<Pessoa[]>([]);
  const [categorias, setCategorias] = useState<Categoria[]>([]);
  const [erro, setErro] = useState('');

  // Carrega lista de pessoas ao abrir o form
  useEffect(() => {
    listarPessoas().then(setPessoas).catch(() => setErro('Erro ao carregar pessoas.'));
  }, []);

  // Recarrega categorias sempre que o tipo muda
  useEffect(() => {
    setIdCategoria('');
    listarCategoriasPorTipo(tipo).then(setCategorias).catch(() => setErro('Erro ao carregar categorias.'));
  }, [tipo]);

  function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setErro('');
    const valorNum = Number(valor);
    if (!descricao.trim()) return setErro('Descrição é obrigatória.');
    if (descricao.length > 400) return setErro('Descrição deve ter no máximo 400 caracteres.');
    if (!valor || isNaN(valorNum) || valorNum <= 0) return setErro('Valor deve ser positivo.');
    if (!idPessoa) return setErro('Selecione uma pessoa.');
    if (!idCategoria) return setErro('Selecione uma categoria.');
    onSalvar({ descricao: descricao.trim(), valor: valorNum, tipo, idPessoa: Number(idPessoa), idCategoria: Number(idCategoria) });
  }

  const erroVisivel = erroExterno || erro;

  return (
    <div className="modal-overlay">
      <div className="modal">
        <h2>Nova Transação</h2>
        <form onSubmit={handleSubmit}>

          <div className="form-field">
            <label>Pessoa</label>
            <select value={idPessoa} onChange={e => setIdPessoa(e.target.value)}>
              <option value="">Selecione...</option>
              {pessoas.map(p => (
                <option key={p.id} value={p.id}>{p.nome}</option>
              ))}
            </select>
          </div>

          <div className="form-field">
            <label>Tipo</label>
            <select value={tipo} onChange={e => setTipo(Number(e.target.value) as TipoTransacao)}>
              {[TipoTransacao.Despesa, TipoTransacao.Receita].map(t => (
                <option key={t} value={t}>{TipoTransacaoLabel[t]}</option>
              ))}
            </select>
          </div>

          <div className="form-field">
            <label>Categoria</label>
            <select value={idCategoria} onChange={e => setIdCategoria(e.target.value)}>
              <option value="">Selecione...</option>
              {categorias.map(c => (
                <option key={c.id} value={c.id}>{c.descricao}</option>
              ))}
            </select>
          </div>

          <div className="form-field">
            <label>Descrição</label>
            <input
              value={descricao}
              onChange={e => setDescricao(e.target.value)}
              maxLength={400}
              placeholder="Ex: Mercado, Salário..."
              autoFocus
            />
          </div>

          <div className="form-field">
            <label>Valor (R$)</label>
            <input
              type="number"
              value={valor}
              onChange={e => setValor(e.target.value)}
              min="0.01"
              step="0.01"
              placeholder="0,00"
            />
          </div>

          {erroVisivel && <p className="form-error">{erroVisivel}</p>}

          <div className="modal-actions">
            <button type="button" className="btn btn-outline" onClick={onCancelar}>
              Cancelar
            </button>
            <button type="submit" className="btn btn-primary">
              Salvar
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
