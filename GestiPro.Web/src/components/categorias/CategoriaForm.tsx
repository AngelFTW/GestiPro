import { useState } from 'react';
import { CategoriaFinalidade, CategoriaFinalidadeLabel, type CategoriaRequest } from '../../types/categoria';

interface Props {
  onSalvar: (dados: CategoriaRequest) => void;
  onCancelar: () => void;
}

// Formulário de criação de categoria
export default function CategoriaForm({ onSalvar, onCancelar }: Props) {
  const [descricao, setDescricao] = useState('');
  const [finalidade, setFinalidade] = useState<CategoriaFinalidade>(CategoriaFinalidade.Despesa);
  const [erro, setErro] = useState('');

  function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setErro('');
    if (!descricao.trim()) return setErro('Descrição é obrigatória.');
    if (descricao.length > 400) return setErro('Descrição deve ter no máximo 400 caracteres.');
    onSalvar({ descricao: descricao.trim(), finalidade });
  }

  return (
    <div className="modal-overlay">
      <div className="modal">
        <h2>Nova Categoria</h2>
        <form onSubmit={handleSubmit}>
          <div className="form-field">
            <label>Descrição</label>
            <input
              value={descricao}
              onChange={e => setDescricao(e.target.value)}
              maxLength={400}
              placeholder="Ex: Alimentação, Salário..."
              autoFocus
            />
          </div>

          <div className="form-field">
            <label>Finalidade</label>
            <select
              value={finalidade}
              onChange={e => setFinalidade(Number(e.target.value) as CategoriaFinalidade)}
            >
              {Object.values(CategoriaFinalidade)
                .filter(v => typeof v === 'number')
                .map(v => (
                  <option key={v} value={v}>
                    {CategoriaFinalidadeLabel[v as CategoriaFinalidade]}
                  </option>
                ))}
            </select>
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
