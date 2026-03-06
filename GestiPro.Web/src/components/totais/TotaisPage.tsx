import { useEffect, useState } from 'react';
import type { RelatorioTotaisPessoa, RelatorioTotaisCategoria } from '../../types/totais';
import { obterTotaisPorPessoa, obterTotaisPorCategoria } from '../../services/totaisService';

type Aba = 'pessoa' | 'categoria';

const brl = (v: number) =>
  new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v);

// Página de totais com duas abas: pessoa e categoria.
export default function TotaisPage() {
  const [abaAtiva, setAbaAtiva] = useState<Aba>('pessoa');
  const [relatorioPessoa, setRelatorioPessoa] = useState<RelatorioTotaisPessoa | null>(null);
  const [relatorioCategoria, setRelatorioCategoria] = useState<RelatorioTotaisCategoria | null>(null);
  const [carregando, setCarregando] = useState(true);
  const [erro, setErro] = useState('');

  useEffect(() => {
    async function carregar() {
      try {
        setCarregando(true);
        setErro('');
        const [pessoa, categoria] = await Promise.all([
          obterTotaisPorPessoa(),
          obterTotaisPorCategoria(),
        ]);
        setRelatorioPessoa(pessoa);
        setRelatorioCategoria(categoria);
      } catch {
        setErro('Não foi possível carregar os totais.');
      } finally {
        setCarregando(false);
      }
    }
    carregar();
  }, []);

  return (
    <>
      <div className="page-header">
        <h1>Totais</h1>
      </div>

      {/* Abas */}
      <div className="tabs">
        <button
          className={`tab ${abaAtiva === 'pessoa' ? 'tab--active' : ''}`}
          onClick={() => setAbaAtiva('pessoa')}
        >
          Por Pessoa
        </button>
        <button
          className={`tab ${abaAtiva === 'categoria' ? 'tab--active' : ''}`}
          onClick={() => setAbaAtiva('categoria')}
        >
          Por Categoria
        </button>
      </div>

      {erro && <div className="alert-error">{erro}</div>}
      {carregando && <p style={{ color: 'var(--text-muted)' }}>Carregando...</p>}

      {/* tab pessoa */}
      {!carregando && abaAtiva === 'pessoa' && relatorioPessoa && (
        <div className="table-wrap">
          <table>
            <thead>
              <tr>
                <th>Pessoa</th>
                <th className="col-num">Receitas</th>
                <th className="col-num">Despesas</th>
                <th className="col-num">Saldo</th>
              </tr>
            </thead>
            <tbody>
              {relatorioPessoa.totaisPorPessoa.length === 0 ? (
                <tr><td colSpan={4} style={{ textAlign: 'center', color: 'var(--text-muted)', padding: '1.5rem' }}>Nenhuma pessoa cadastrada.</td></tr>
              ) : (
                relatorioPessoa.totaisPorPessoa.map(p => (
                  <tr key={p.pessoaId}>
                    <td>{p.pessoaNome}</td>
                    <td className="col-num color-receita">{brl(p.totalReceitas)}</td>
                    <td className="col-num color-despesa">{brl(p.totalDespesas)}</td>
                    <td className={`col-num ${p.saldo >= 0 ? 'color-receita' : 'color-despesa'}`} style={{ fontWeight: 700 }}>
                      {brl(p.saldo)}
                    </td>
                  </tr>
                ))
              )}
            </tbody>
            <tfoot>
              <tr className="tfoot-total">
                <td><strong>Total Geral</strong></td>
                <td className="col-num color-receita"><strong>{brl(relatorioPessoa.totalGeralReceitas)}</strong></td>
                <td className="col-num color-despesa"><strong>{brl(relatorioPessoa.totalGeralDespesas)}</strong></td>
                <td className={`col-num ${relatorioPessoa.saldoLiquido >= 0 ? 'color-receita' : 'color-despesa'}`}>
                  <strong>{brl(relatorioPessoa.saldoLiquido)}</strong>
                </td>
              </tr>
            </tfoot>
          </table>
        </div>
      )}

      {/* tab categoria */}
      {!carregando && abaAtiva === 'categoria' && relatorioCategoria && (
        <div className="table-wrap">
          <table>
            <thead>
              <tr>
                <th>Categoria</th>
                <th className="col-num">Receitas</th>
                <th className="col-num">Despesas</th>
                <th className="col-num">Saldo</th>
              </tr>
            </thead>
            <tbody>
              {relatorioCategoria.totaisPorCategoria.length === 0 ? (
                <tr><td colSpan={4} style={{ textAlign: 'center', color: 'var(--text-muted)', padding: '1.5rem' }}>Nenhuma categoria cadastrada.</td></tr>
              ) : (
                relatorioCategoria.totaisPorCategoria.map(c => (
                  <tr key={c.categoriaId}>
                    <td>{c.categoriaDescricao}</td>
                    <td className="col-num color-receita">{brl(c.totalReceitas)}</td>
                    <td className="col-num color-despesa">{brl(c.totalDespesas)}</td>
                    <td className={`col-num ${c.saldo >= 0 ? 'color-receita' : 'color-despesa'}`} style={{ fontWeight: 700 }}>
                      {brl(c.saldo)}
                    </td>
                  </tr>
                ))
              )}
            </tbody>
            <tfoot>
              <tr className="tfoot-total">
                <td><strong>Total Geral</strong></td>
                <td className="col-num color-receita"><strong>{brl(relatorioCategoria.totalGeralReceitas)}</strong></td>
                <td className="col-num color-despesa"><strong>{brl(relatorioCategoria.totalGeralDespesas)}</strong></td>
                <td className={`col-num ${relatorioCategoria.saldoLiquido >= 0 ? 'color-receita' : 'color-despesa'}`}>
                  <strong>{brl(relatorioCategoria.saldoLiquido)}</strong>
                </td>
              </tr>
            </tfoot>
          </table>
        </div>
      )}
    </>
  );
}
