import { useEffect, useState } from 'react';
import type { Pessoa, PessoaRequest } from '../../types/pessoa';

interface Props {
  // quando a Pessoa vem undefined, é edição
  pessoaEditar?: Pessoa;
  onSalvar: (dados: PessoaRequest) => void;
  onCancelar: () => void;
}

// Form pra criação e edicção de pessoas
export default function PessoaForm({ pessoaEditar, onSalvar, onCancelar }: Props) {
  const [nome, setNome] = useState('');
  const [idade, setIdade] = useState('');
  const [erro, setErro] = useState('');

  // Preenche os inputs do editar
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
    <div style={styles.overlay}>
      <div style={styles.modal}>
        <h2 style={styles.titulo}>
          {pessoaEditar ? 'Editar Pessoa' : 'Nova Pessoa'}
        </h2>

        <form onSubmit={handleSubmit}>
          <div style={styles.campo}>
            <label style={styles.label}>Nome</label>
            <input
              style={styles.input}
              value={nome}
              onChange={e => setNome(e.target.value)}
              maxLength={200}
              placeholder="Nome completo"
              autoFocus
            />
          </div>

          <div style={styles.campo}>
            <label style={styles.label}>Idade</label>
            <input
              style={styles.input}
              type="number"
              value={idade}
              onChange={e => setIdade(e.target.value)}
              min={0}
              max={150}
              placeholder="Idade"
            />
          </div>

          {erro && <p style={styles.erro}>{erro}</p>}

          <div style={styles.botoes}>
            <button type="button" onClick={onCancelar} style={styles.btnCancelar}>
              Cancelar
            </button>
            <button type="submit" style={styles.btnSalvar}>
              Salvar
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}

const styles: Record<string, React.CSSProperties> = {
  overlay: {
    position: 'fixed', inset: 0,
    background: 'rgba(0,0,0,0.45)',
    display: 'flex', alignItems: 'center', justifyContent: 'center',
    zIndex: 100,
  },
  modal: {
    background: '#fff', borderRadius: 10,
    padding: '2rem', width: 400,
    boxShadow: '0 4px 24px rgba(0,0,0,0.18)',
  },
  titulo: { marginBottom: '1.5rem', fontSize: '1.2rem', fontWeight: 600 },
  campo: { marginBottom: '1rem', display: 'flex', flexDirection: 'column', gap: 6 },
  label: { fontWeight: 500, fontSize: '0.9rem', color: '#444' },
  input: {
    padding: '0.5rem 0.75rem', borderRadius: 6,
    border: '1px solid #ccc', fontSize: '1rem', outline: 'none',
  },
  erro: { color: '#d32f2f', fontSize: '0.88rem', marginBottom: '0.75rem' },
  botoes: { display: 'flex', justifyContent: 'flex-end', gap: 10, marginTop: '1.5rem' },
  btnCancelar: {
    padding: '0.5rem 1.2rem', borderRadius: 6,
    border: '1px solid #ccc', background: '#f5f5f5',
    cursor: 'pointer', fontSize: '0.95rem',
  },
  btnSalvar: {
    padding: '0.5rem 1.2rem', borderRadius: 6,
    border: 'none', background: '#1976d2', color: '#fff',
    cursor: 'pointer', fontSize: '0.95rem', fontWeight: 600,
  },
};
