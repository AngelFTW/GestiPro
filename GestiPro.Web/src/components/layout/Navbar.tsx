import { useTheme } from '../../context/ThemeContext';

type Page = 'home' | 'pessoas' | 'categorias' | 'transacoes' | 'totais';

interface Props {
  paginaAtiva: Page;
  onNavegar: (pagina: Page) => void;
}

const links: { pagina: Page; label: string }[] = [
  { pagina: 'home',       label: 'Início' },
  { pagina: 'pessoas',    label: 'Pessoas' },
  { pagina: 'categorias', label: 'Categorias' },
  { pagina: 'transacoes', label: 'Transações' },
  { pagina: 'totais',     label: 'Totais' },
];

// Barra de navegação com links de página e alternador de tema claro/escuro.
export default function Navbar({ paginaAtiva, onNavegar }: Props) {
  const { theme, toggleTheme } = useTheme();

  return (
    <nav className="navbar">
      <div className="navbar-inner">
        <span className="navbar-brand">GestiPro</span>

        <div className="navbar-links">
          {links.map(({ pagina, label }) => (
            <button
              key={pagina}
              className={`nav-link ${paginaAtiva === pagina ? 'active' : ''}`}
              onClick={() => onNavegar(pagina)}
            >
              {label}
            </button>
          ))}
        </div>

        <div className="navbar-actions">
          <button
            className="theme-toggle"
            onClick={toggleTheme}
            title={theme === 'light' ? 'Ativar modo escuro' : 'Ativar modo claro'}
          >
            {theme === 'light' ? '🌙' : '☀️'}
          </button>
        </div>
      </div>
    </nav>
  );
}
