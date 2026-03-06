import { useEffect, useState } from 'react';
import type { Categoria, CategoriaRequest } from '../../types/categoria';
import { listarCategorias, criarCategoria } from '../../services/categoriaService';
import CategoriaTable from './CategoriaTable';
import CategoriaForm from './CategoriaForm';

// Página de cadastro de categorias
export default function CategoriaPage() {
  const [categorias, setCategorias] = useState<Categoria[]>([]);
  const [carregando, setCarregando] = useState(true);
  const [erro, setErro] = useState('');
  const [formAberto, setFormAberto] = useState(false);

  async function carregar() {
    try {
      setCarregando(true);
      setErro('');
      setCategorias(await listarCategorias());
    } catch {
      setErro('Não foi possível carregar as categorias.');
    } finally {
      setCarregando(false);
    }
  }

  useEffect(() => { carregar(); }, []);

  async function handleSalvar(dados: CategoriaRequest) {
    try {
      await criarCategoria(dados);
      setFormAberto(false);
      carregar();
    } catch {
      setErro('Erro ao criar categoria.');
    }
  }

  return (
    <>
      <div className="page-header">
        <h1>Categorias</h1>
        <button className="btn btn-primary" onClick={() => setFormAberto(true)}>
          + Nova Categoria
        </button>
      </div>

      {erro && <div className="alert-error">{erro}</div>}

      {carregando ? (
        <p style={{ color: 'var(--text-muted)' }}>Carregando...</p>
      ) : (
        <CategoriaTable categorias={categorias} />
      )}

      {formAberto && (
        <CategoriaForm
          onSalvar={handleSalvar}
          onCancelar={() => setFormAberto(false)}
        />
      )}
    </>
  );
}
