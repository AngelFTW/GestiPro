import { useState } from 'react';
import { ThemeProvider } from './context/ThemeContext';
import Navbar from './components/layout/Navbar';
import HomePage from './components/layout/HomePage';
import PessoaPage from './components/pessoas/PessoaPage';
import CategoriaPage from './components/categorias/CategoriaPage';
import TransacaoPage from './components/transacoes/TransacaoPage';
import TotaisPage from './components/totais/TotaisPage';
import './App.css';

type Page = 'home' | 'pessoas' | 'categorias' | 'transacoes' | 'totais';

function AppContent() {
  const [paginaAtiva, setPaginaAtiva] = useState<Page>('home');

  function renderPagina() {
    switch (paginaAtiva) {
      case 'pessoas':    return <PessoaPage />;
      case 'categorias': return <CategoriaPage />;
      case 'transacoes': return <TransacaoPage />;
      case 'totais':     return <TotaisPage />;
      default:           return <HomePage onNavegar={setPaginaAtiva} />;
    }
  }

  return (
    <>
      <Navbar paginaAtiva={paginaAtiva} onNavegar={setPaginaAtiva} />
      <main className="page-content">
        {renderPagina()}
      </main>
    </>
  );
}

export default function App() {
  return (
    <ThemeProvider>
      <AppContent />
    </ThemeProvider>
  );
}
