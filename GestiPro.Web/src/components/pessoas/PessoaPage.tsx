import { useEffect, useState } from 'react';
import type { Pessoa, PessoaRequest } from '../../types/pessoa';
import {
  listarPessoas,
  criarPessoa,
  atualizarPessoa,
  deletarPessoa,
} from '../../services/pessoaService';
import PessoaTable from './PessoaTable';
import PessoaForm from './PessoaForm';

export default function PessoaPage() {
  const [pessoas, setPessoas] = useState<Pessoa[]>([]);
  const [carregando, setCarregando] = useState(true);
  const [erro, setErro] = useState('');

  // Edicao pessoa, undefined = criação
  const [pessoaEditando, setPessoaEditando] = useState<Pessoa | undefined>();
  const [formAberto, setFormAberto] = useState(false);

  // Pessoa aguardando confirmação de deleção
  const [pessoaParaDeletar, setPessoaParaDeletar] = useState<Pessoa | null>(null);

  async function carregarPessoas() {
    try {
      setCarregando(true);
      setErro('');
      const dados = await listarPessoas();
      setPessoas(dados);
    } catch {
      setErro('Não foi possível carregar as pessoas.');
    } finally {
      setCarregando(false);
    }
  }

  useEffect(() => { carregarPessoas(); }, []);

  // Cria ou atualiza uma pessoa
  async function handleSalvar(dados: PessoaRequest) {
    try {
      if (pessoaEditando) {
        await atualizarPessoa(pessoaEditando.id, dados);
      } else {
        await criarPessoa(dados);
      }
      fecharForm();
      carregarPessoas();
    } catch {
      setErro('Erro ao salvar pessoa.');
    }
  }

  async function handleDeletar() {
    if (!pessoaParaDeletar) return;
    try {
      await deletarPessoa(pessoaParaDeletar.id);
      setPessoaParaDeletar(null);
      carregarPessoas();
    } catch {
      setErro('Erro ao deletar pessoa.');
    }
  }

  function abrirFormCriacao() {
    setPessoaEditando(undefined);
    setFormAberto(true);
  }

  function abrirFormEdicao(pessoa: Pessoa) {
    setPessoaEditando(pessoa);
    setFormAberto(true);
  }

  function fecharForm() {
    setFormAberto(false);
    setPessoaEditando(undefined);
  }

  return (
    <div style={styles.pagina}>
      <div style={styles.cabecalho}>
        <h1 style={styles.titulo}>Pessoas</h1>
        <button style={styles.btnNovo} onClick={abrirFormCriacao}>
          + Nova Pessoa
        </button>
      </div>

      {erro && <p style={styles.erro}>{erro}</p>}

      {carregando ? (
        <p style={styles.info}>Carregando...</p>
      ) : (
        <PessoaTable
          pessoas={pessoas}
          onEditar={abrirFormEdicao}
          onDeletar={setPessoaParaDeletar}
        />
      )}

      {/* Modal criação / edição */}
      {formAberto && (
        <PessoaForm
          pessoaEditar={pessoaEditando}
          onSalvar={handleSalvar}
          onCancelar={fecharForm}
        />
      )}

      {/* Modal confirmação de deleção */}
      {pessoaParaDeletar && (
        <div style={styles.overlay}>
          <div style={styles.modalConfirm}>
            <p style={styles.confirmTexto}>
              Deletar <strong>{pessoaParaDeletar.nome}</strong>?<br />
              <span style={styles.confirmAviso}>
                Todas as transações desta pessoa também serão removidas.
              </span>
            </p>
            <div style={styles.confirmBotoes}>
              <button
                style={styles.btnCancelar}
                onClick={() => setPessoaParaDeletar(null)}
              >
                Cancelar
              </button>
              <button style={styles.btnDeletar} onClick={handleDeletar}>
                Deletar
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}

const styles: Record<string, React.CSSProperties> = {
  pagina: { maxWidth: 860, margin: '0 auto', padding: '2rem 1.5rem' },
  cabecalho: {
    display: 'flex', justifyContent: 'space-between',
    alignItems: 'center', marginBottom: '1.5rem',
  },
  titulo: { fontSize: '1.6rem', fontWeight: 700, color: '#1a1a2e' },
  btnNovo: {
    padding: '0.55rem 1.25rem', borderRadius: 7,
    border: 'none', background: '#1976d2', color: '#fff',
    cursor: 'pointer', fontSize: '0.95rem', fontWeight: 600,
  },
  erro: { color: '#d32f2f', marginBottom: '1rem' },
  info: { color: '#888' },
  overlay: {
    position: 'fixed', inset: 0, background: 'rgba(0,0,0,0.45)',
    display: 'flex', alignItems: 'center', justifyContent: 'center', zIndex: 100,
  },
  modalConfirm: {
    background: '#fff', borderRadius: 10, padding: '2rem',
    width: 380, boxShadow: '0 4px 24px rgba(0,0,0,0.18)',
  },
  confirmTexto: { fontSize: '1rem', marginBottom: '1.5rem', lineHeight: 1.6 },
  confirmAviso: { fontSize: '0.88rem', color: '#888' },
  confirmBotoes: { display: 'flex', justifyContent: 'flex-end', gap: 10 },
  btnCancelar: {
    padding: '0.5rem 1.2rem', borderRadius: 6,
    border: '1px solid #ccc', background: '#f5f5f5', cursor: 'pointer',
  },
  btnDeletar: {
    padding: '0.5rem 1.2rem', borderRadius: 6,
    border: 'none', background: '#d32f2f', color: '#fff',
    cursor: 'pointer', fontWeight: 600,
  },
};
