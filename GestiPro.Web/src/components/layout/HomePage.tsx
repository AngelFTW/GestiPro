type Page = 'home' | 'pessoas' | 'categorias' | 'transacoes' | 'totais';

interface Props {
  onNavegar: (pagina: Page) => void;
}

const cards: { pagina: Page; icon: string; titulo: string; descricao: string }[] = [
  { pagina: 'pessoas',    icon: '👤', titulo: 'Pessoas',    descricao: 'Cadastre e gerencie as pessoas do sistema.' },
  { pagina: 'categorias', icon: '🏷️', titulo: 'Categorias', descricao: 'Crie categorias para classificar as transações.' },
  { pagina: 'transacoes', icon: '💸', titulo: 'Transações', descricao: 'Registre receitas e despesas por pessoa.' },
  { pagina: 'totais',     icon: '📊', titulo: 'Totais',     descricao: 'Consulte saldos por pessoa e por categoria.' },
];

// Tela inicial com acesso rápido às seções do sistema.
export default function HomePage({ onNavegar }: Props) {
  return (
    <div>
      <div className="home-hero">
        <h1>Bem-vindo ao GestiPro</h1>
        <p>Sistema de controle de gastos residenciais. Gerencie pessoas, categorias e transações em um só lugar.</p>
      </div>

      <div className="home-cards">
        {cards.map(card => (
          <div key={card.pagina} className="home-card" onClick={() => onNavegar(card.pagina)}>
            <div className="home-card-icon">{card.icon}</div>
            <h3>{card.titulo}</h3>
            <p>{card.descricao}</p>
          </div>
        ))}
      </div>
    </div>
  );
}
