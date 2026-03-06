import type { Transacao } from '../../types/transacao';
import { TipoTransacao } from '../../types/transacao';

interface Props {
  transacoes: Transacao[];
}

const brl = (v: number) =>
  new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v);

// Tabela somente leitura de transações.
export default function TransacaoTable({ transacoes }: Props) {
  if (transacoes.length === 0) {
    return <p className="table-empty">Nenhuma transação cadastrada.</p>;
  }

  return (
    <div className="table-wrap">
      <table>
        <thead>
          <tr>
            <th>#</th>
            <th>Pessoa</th>
            <th>Descrição</th>
            <th>Categoria</th>
            <th>Tipo</th>
            <th>Valor</th>
          </tr>
        </thead>
        <tbody>
          {transacoes.map(t => (
            <tr key={t.id}>
              <td>{t.id}</td>
              <td>{t.pessoaNome}</td>
              <td>{t.descricao}</td>
              <td>{t.categoriaDescricao}</td>
              <td>
                <span className={`badge-tipo badge-tipo--${t.tipo === TipoTransacao.Receita ? 'receita' : 'despesa'}`}>
                  {t.tipoDescricao}
                </span>
              </td>
              <td style={{ fontWeight: 600, color: t.tipo === TipoTransacao.Receita ? 'var(--color-receita)' : 'var(--color-despesa)' }}>
                {brl(t.valor)}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
