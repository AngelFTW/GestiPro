import type { Pessoa } from '../../types/pessoa';

interface Props {
  pessoas: Pessoa[];
  onEditar: (pessoa: Pessoa) => void;
  onDeletar: (pessoa: Pessoa) => void;
}

// Tabela que lista todas as pessoas cadastradas.
// Exibe badge "Menor de idade" quando aplicável.
export default function PessoaTable({ pessoas, onEditar, onDeletar }: Props) {
  if (pessoas.length === 0) {
    return <p style={styles.vazio}>Nenhuma pessoa cadastrada.</p>;
  }

  return (
    <table style={styles.tabela}>
      <thead>
        <tr>
          <th style={styles.th}>#</th>
          <th style={styles.th}>Nome</th>
          <th style={styles.th}>Idade</th>
          <th style={styles.th}>Ações</th>
        </tr>
      </thead>
      <tbody>
        {pessoas.map(pessoa => (
          <tr key={pessoa.id} style={styles.tr}>
            <td style={styles.td}>{pessoa.id}</td>
            <td style={styles.td}>
              {pessoa.nome}
              {pessoa.menorIdade && (
                <span style={styles.badge}>Menor de idade</span>
              )}
            </td>
            <td style={styles.td}>{pessoa.idade}</td>
            <td style={styles.td}>
              <button
                style={styles.btnEditar}
                onClick={() => onEditar(pessoa)}
              >
                Editar
              </button>
              <button
                style={styles.btnDeletar}
                onClick={() => onDeletar(pessoa)}
              >
                Deletar
              </button>
            </td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}

const styles: Record<string, React.CSSProperties> = {
  tabela: {
    width: '100%', borderCollapse: 'collapse',
    background: '#fff', borderRadius: 8, overflow: 'hidden',
    boxShadow: '0 1px 6px rgba(0,0,0,0.08)',
  },
  th: {
    background: '#1976d2', color: '#fff',
    padding: '0.75rem 1rem', textAlign: 'left',
    fontWeight: 600, fontSize: '0.9rem',
  },
  tr: { borderBottom: '1px solid #eee' },
  td: { padding: '0.75rem 1rem', fontSize: '0.95rem', verticalAlign: 'middle' },
  badge: {
    marginLeft: 8, background: '#fff3e0', color: '#e65100',
    borderRadius: 4, padding: '2px 7px', fontSize: '0.75rem', fontWeight: 600,
  },
  vazio: { color: '#888', fontStyle: 'italic', marginTop: '2rem' },
  btnEditar: {
    marginRight: 8, padding: '0.35rem 0.85rem',
    borderRadius: 5, border: '1px solid #1976d2',
    background: 'transparent', color: '#1976d2',
    cursor: 'pointer', fontSize: '0.88rem', fontWeight: 500,
  },
  btnDeletar: {
    padding: '0.35rem 0.85rem', borderRadius: 5,
    border: '1px solid #d32f2f', background: 'transparent',
    color: '#d32f2f', cursor: 'pointer', fontSize: '0.88rem', fontWeight: 500,
  },
};
