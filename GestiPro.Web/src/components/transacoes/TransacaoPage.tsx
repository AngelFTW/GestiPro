import { useEffect, useState } from 'react';
import type { Transacao, TransacaoRequest } from '../../types/transacao';
import { listarTransacoes, criarTransacao } from '../../services/transacaoService';
import TransacaoTable from './TransacaoTable';
import TransacaoForm from './TransacaoForm';

// Página de cadastro de transações. Erros de validação são tratados no backend.
export default function TransacaoPage() {
  const [transacoes, setTransacoes] = useState<Transacao[]>([]);
  const [carregando, setCarregando] = useState(true);
  const [erro, setErro] = useState('');
  const [erroForm, setErroForm] = useState('');
  const [formAberto, setFormAberto] = useState(false);

  async function carregar() {
    try {
      setCarregando(true);
      setErro('');
      setTransacoes(await listarTransacoes());
    } catch {
      setErro('Não foi possível carregar as transações.');
    } finally {
      setCarregando(false);
    }
  }

  useEffect(() => { carregar(); }, []);

  async function handleSalvar(dados: TransacaoRequest) {
    try {
      setErroForm('');
      await criarTransacao(dados);
      setFormAberto(false);
      carregar();
    } catch (e: unknown) {
      setErroForm(e instanceof Error ? e.message : 'Erro ao criar transação.');
    }
  }

  function abrirForm() {
    setErroForm('');
    setFormAberto(true);
  }

  return (
    <>
      <div className="page-header">
        <h1>Transações</h1>
        <button className="btn btn-primary" onClick={abrirForm}>
          + Nova Transação
        </button>
      </div>

      {erro && <div className="alert-error">{erro}</div>}

      {carregando ? (
        <p style={{ color: 'var(--text-muted)' }}>Carregando...</p>
      ) : (
        <TransacaoTable transacoes={transacoes} />
      )}

      {formAberto && (
        <TransacaoForm
          onSalvar={handleSalvar}
          onCancelar={() => { setFormAberto(false); setErroForm(''); }}
          erroExterno={erroForm}
        />
      )}
    </>
  );
}
