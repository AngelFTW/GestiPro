import type { Categoria } from '../../types/categoria';

interface Props {
  categorias: Categoria[];
}

// Table de categorias
export default function CategoriaTable({ categorias }: Props) {
  if (categorias.length === 0) {
    return <p className="table-empty">Nenhuma categoria cadastrada.</p>;
  }

  return (
    <div className="table-wrap">
      <table>
        <thead>
          <tr>
            <th>#</th>
            <th>Descrição</th>
            <th>Finalidade</th>
          </tr>
        </thead>
        <tbody>
          {categorias.map(cat => (
            <tr key={cat.id}>
              <td>{cat.id}</td>
              <td>{cat.descricao}</td>
              <td>
                <span className={`badge-finalidade badge-finalidade--${cat.finalidade}`}>
                  {cat.finalidadeDescricao}
                </span>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
