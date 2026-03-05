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
    return <p className="table-empty">Nenhuma pessoa cadastrada.</p>;
  }

  return (
    <div className="table-wrap">
      <table>
        <thead>
          <tr>
            <th>#</th>
            <th>Nome</th>
            <th>Idade</th>
            <th>Ações</th>
          </tr>
        </thead>
        <tbody>
          {pessoas.map(pessoa => (
            <tr key={pessoa.id}>
              <td>{pessoa.id}</td>
              <td>
                {pessoa.nome}
                {pessoa.menorIdade && (
                  <span className="badge-warn">Menor de idade</span>
                )}
              </td>
              <td>{pessoa.idade}</td>
              <td style={{ display: 'flex', gap: 8 }}>
                <button
                  className="btn btn-outline-primary"
                  onClick={() => onEditar(pessoa)}
                >
                  Editar
                </button>
                <button
                  className="btn btn-outline-danger"
                  onClick={() => onDeletar(pessoa)}
                >
                  Deletar
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
