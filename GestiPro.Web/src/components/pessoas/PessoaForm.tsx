import { useEffect, useState } from 'react';
import type { Pessoa, PessoaRequest } from '../../types/pessoa';

interface Props {
  // quando a Pessoa vem undefined, é criação
  pessoaEditar?: Pessoa;
  onSalvar: (dados: PessoaRequest) => void;
  onCancelar: () => void;
}

// Form pra criação e edição de pessoas
export default function PessoaForm({ pessoaEditar, onSalvar, onCancelar }: Props) {
  const [nome, setNome] = useState('');
  const [idade, setIdade] = useState('');
  const [erro, setErro] = useState('');

  // Preenche os inputs ao editar
  useEffect(() => {
    if (pessoaEditar) {
      setNome(pessoaEditar.nome);
      setIdade(String(pessoaEditar.idade));
    }
  }, [pessoaEditar]);

  function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setErro('');

    const idadeNum = Number(idade);

    if (!nome.trim()) return setErro('Nome é obrigatório.');
    if (nome.length > 200) return setErro('Nome deve ter no máximo 200 caracteres.');
    if (!idade || isNaN(idadeNum) || idadeNum < 0 || idadeNum > 150)
      return setErro('Idade deve ser um número entre 0 e 150.');

    onSalvar({ nome: nome.trim(), idade: idadeNum });
  }

  return (
    <div className="modal-overlay">
      <div className="modal">
        <h2>{pessoaEditar ? 'Editar Pessoa' : 'Nova Pessoa'}</h2>

        <form onSubmit={handleSubmit}>
          <div className="form-field">
            <label>Nome</label>
            <input
              value={nome}
              onChange={e => setNome(e.target.value)}
              maxLength={200}
              placeholder="Nome completo"
              autoFocus
            />
          </div>

          <div className="form-field">
            <label>Idade</label>
            <input
              type="number"
              value={idade}
              onChange={e => setIdade(e.target.value)}
              min={0}
              max={150}
              placeholder="Idade"
            />
          </div>

          {erro && <p className="form-error">{erro}</p>}

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
