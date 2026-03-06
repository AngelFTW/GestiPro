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

// Página principal do cadastro de pessoas.
// Gerencia o estado da lista, abertura do formulário e confirmação de deleção.
export default function PessoaPage() {
  const [pessoas, setPessoas] = useState<Pessoa[]>([]);
  const [carregando, setCarregando] = useState(true);
  const [erro, setErro] = useState('');

  // Pessoa sendo editada; undefined = formulário em modo criação
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

  // Salva (cria ou atualiza) conforme se há pessoa sendo editada
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
    if (!pessoaParaDeletar) 
      return;
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
    <>
      <div className="page-header">
        <h1>Pessoas</h1>
        <button className="btn btn-primary" onClick={abrirFormCriacao}>
          + Nova Pessoa
        </button>
      </div>

      {erro && <div className="alert-error">{erro}</div>}

      {carregando ? (
        <p style={{ color: 'var(--text-muted)' }}>Carregando...</p>
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
        <div className="modal-overlay">
          <div className="modal">
            <h2>Confirmar exclusão</h2>
            <p style={{ marginBottom: '0.5rem', lineHeight: 1.6 }}>
              Deletar <strong>{pessoaParaDeletar.nome}</strong>?
            </p>
            <p style={{ fontSize: '0.88rem', color: 'var(--text-muted)', marginBottom: '1.5rem' }}>
              Todas as transações desta pessoa também serão removidas.
            </p>
            <div className="modal-actions">
              <button
                className="btn btn-outline"
                onClick={() => setPessoaParaDeletar(null)}
              >
                Cancelar
              </button>
              <button className="btn btn-danger" onClick={handleDeletar}>
                Deletar
              </button>
            </div>
          </div>
        </div>
      )}
    </>
  );
}
