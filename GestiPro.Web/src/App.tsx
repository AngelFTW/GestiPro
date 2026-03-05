import { useState } from 'react';
import { ThemeProvider } from './context/ThemeContext';
import Navbar from './components/layout/Navbar';
import HomePage from './components/layout/HomePage';
import PessoaPage from './components/pessoas/PessoaPage';
import './App.css';

type Page = 'home' | 'pessoas';


function AppContent() {
  const [paginaAtiva, setPaginaAtiva] = useState<Page>('home');

  function renderPagina() {
    switch (paginaAtiva) {
      case 'pessoas': return <PessoaPage />;
      default:        return <HomePage onNavegar={setPaginaAtiva} />;
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
