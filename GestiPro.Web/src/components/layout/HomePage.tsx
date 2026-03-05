type Page = 'home' | 'pessoas';

interface Props {
  onNavegar: (pagina: Page) => void;
}

const cards: { pagina: Page; icon: string; titulo: string; descricao: string }[] = [
  {
    pagina: 'pessoas',
    icon: '👤',
    titulo: 'Pessoas',
    descricao: 'Cadastro e administração de pessoas.',
  },
];

// Tela inicial com acesso rápido às seções do sistema.
export default function HomePage({ onNavegar }: Props) {
  return (
    <div>
      <div className="home-hero">
        <h1>Bem-vindo ao GestiPro</h1>
        <p>Sistema de controle de gastos residenciais.</p>
      </div>

      <div className="home-cards">
        {cards.map(card => (
          <div
            key={card.pagina}
            className="home-card"
            onClick={() => onNavegar(card.pagina)}
          >
            <div className="home-card-icon">{card.icon}</div>
            <h3>{card.titulo}</h3>
            <p>{card.descricao}</p>
          </div>
        ))}
      </div>
    </div>
  );
}
